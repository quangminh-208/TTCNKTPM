using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDT.Models;
using PagedList;
using PagedList.Mvc;
using WebDT.Models.EF;

namespace WebDT.Controllers
{
    public class HomeController : Controller
    {
        WebMayTinhEntities _db = new WebMayTinhEntities();

    
        public ActionResult Index(int ? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 12;
            var v = from t in _db.Products
                    where t.meta != null
                    orderby t.order ascending
                    select t;
            return PartialView(v.ToPagedList(pageNumber, pageSize));
            

        }

        public PartialViewResult newProduct()
        {
            var v = _db.Products.OrderByDescending(x => x.datebegin);
            return PartialView(v.ToList());
        }

        public ActionResult SeeAll(string filter = null, int page = 1, int pagesize = 20)
        {
            IPagedList<Product> models;
            if(filter == "san-pham-moi")
            {
                var v = _db.Products.OrderByDescending(x => x.datebegin);
                models = v.ToPagedList(page, pagesize);
                ViewBag.Alert = "Sản phẩm mới";
            }else
            {
                var order_detail = _db.Order_Detail.ToList();
                var lstProduct = new List<Product>();
                foreach(var item in order_detail)
                {
                    var pro = _db.Products.Find(item.Product_id);

                    if (!lstProduct.Exists(x => x.id == item.Product_id))
                    {
                        lstProduct.Add(pro);
                    }
                }

                models = lstProduct.OrderByDescending(x => x.id).ToPagedList(page, pagesize);
                
                ViewBag.Alert = "Sản phẩm bán chạy";
            }
            return View(models);
        }
    
        
        public ActionResult About()
        {
            ViewBag.Message = "Your contact page12.";

            return View();
        }
            public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page13.";

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Contact([Bind(Include = "id,Fullname,Email,Descriptions,CreatedDate")] Contact lienhe)
        {
     
            if (ModelState.IsValid)
            {
                lienhe.CreatedDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                
                
                _db.Contacts.Add(lienhe);
                _db.SaveChanges();
                return RedirectToAction("Success", "Home");
            }

            return View(lienhe);
        }
        public ActionResult Success()
        {
            return View();
        }
        public ActionResult TinTuc()
        {
            ViewBag.Message = "Your contact page32.";

            return View();
        }

        public ActionResult DangNhap()
        {
            ViewBag.Message = "Your contact page33.";

            return View();
        }
    }
}