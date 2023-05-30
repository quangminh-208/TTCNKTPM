using Facebook;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebDT.Models;
using WebDT.Models.EF;

namespace WebDT.Controllers
{
    public class TaiKhoanController : Controller
    {
        //jk
        WebMayTinhEntities _db = new WebMayTinhEntities();

        // GET: TaiKhoan
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {

            return View();
        }
        [HttpPost]
        public ActionResult DangKy(RegisterModel model)
        {
           
                var us = new UserDAO();
                if (us.CheckUserName(model.username))
                {
                    ViewBag.Success = "Tên đăng nhập đã tồn tại!";
                }
                else if (us.CheckEmail(model.email))
                {
                    ViewBag.Success = "Email đã tồn tại!";
                }
                else
                {
                    var user = new User();
                    user.username = model.username;
                    user.password = model.password;
                    user.name = model.name;
                    user.address = model.address;
                    user.email = model.email;
                    user.phone = model.phone;
                    user.countOrder = 0;
                    user.amountSpent = 0;
                    user.status = true;
                    var result = us.Insert(user);
                    if (result > 0)
                    {
                        
                        TempData["Success"] = "Đăng ký thành công, bạn vui lòng đăng nhập lại.";
                        model = new RegisterModel();
                        return Redirect("/dang-nhap");
                    }
                    else
                    {
                        ViewBag.Success = "Đăng ký không thành công!";
                    }
                }
            return View(model);
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(ModelLogin model , String username )
        {
            if (ModelState.IsValid)
            {
               
                var ad = new UserDAO();
                var result = ad.Login(model.UserName, model.Password);
                var obj = _db.Users.Where(a=> a.username.Equals(username)).FirstOrDefault();

                if (result == 1)
                {
                    var user = ad.getById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.username;                 
                    userSession.UserID = user.id;               
                    Session["UserID"] = user.id;
                    Session["UserName"] = model.UserName.ToString();
                    Session["ten"] = obj.name.ToString();
                    Session["Admin"] = obj.Quyenadmin;
                    Session["User"] = obj.Quyenuser;
                    Session["phone"] = obj.phone.ToString();
                    Session["DiaChi"] = obj.address.ToString();
                    Session["Email"] = obj.email.ToString();

                    //var listCredential = ad.GetListCredential(model.UserName);
                    //Session.Add(CommonConstants.SESSION_CREDENTIALS, listCredential);
                    //Session.Add(CommonConstants.USER_SESSION, userSession);
                    //var t = 'admin';
                   if (Session["Admin"] !=   null  )
                    {
                        return Redirect("/admin/HoaDon/ThongKe");
    
                    }
                    else
                    if( Session["User"]!=null)
                    {
                        return Redirect("/");
                        
                    }
                   else
                    {
                        return Redirect("/admin");
                    }
                   
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoảng không tồn tại!");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoảng đang bị khóa!");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng!");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập không thành công!");
                }
            }
            return View(model);

        }

        
        public JsonResult LoginFacebook(string user, string ID)
        {
            Session["UserName"] = user;
            Session["ten"] = user;

            //Kiểm tra xem user đã lưu trong csdl chưa?
            var entity = _db.Users.Where(x => x.username == ID).ToList();
            if(entity.Count > 0)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                var userfb = new User();
                userfb.username = ID;
                userfb.name = user;
                userfb.countOrder = 0;
                userfb.amountSpent = 0;
                userfb.status = true;
                _db.Users.Add(userfb);
                _db.SaveChanges();

                return Json(new
                {
                    status = true
                });
            }

            
        }

       


        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        

        public ActionResult QuyenMatKhau(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var TaiKhoan = _db.Users.Find(id);
            if (TaiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(TaiKhoan);
        }
      

        //Danh sách hóa đơn đã đặt hàng của khách hàng.
        public ActionResult DanhSachHoaDon(int page = 1, int pagesize = 10)
        {
           
            string user = Session["UserName"].ToString();
            
                var models = from sp in _db.Products
                          join chitietgh in _db.Order_Detail on sp.id equals chitietgh.Product_id
                          join giohangs in _db.Orders on chitietgh.Order_id equals giohangs.id
                          join taikhoankh in _db.Users on giohangs.User_id equals taikhoankh.username
                          where giohangs.User_id == user
                          select new DsSP()
                          {                            
                              TaiKhoan = taikhoankh.username,
                              Tensp = sp.name,
                              Hinh = sp.img,
                              Tien = chitietgh.Amount,
                              SoLuong = chitietgh.Quantity,
                              NgayDat = giohangs.CreatedDate,
                          };
            return View(models.OrderByDescending(x => x.NgayDat).ToPagedList(page, pagesize));
        }       
    }
}

