using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebDT.Models;
using WebDT.Models.EF;

namespace WebDT.Controllers
{
    public class GioHangController : Controller
    {
        WebMayTinhEntities _db = new WebMayTinhEntities();
        private const string CartSession = "CartSession";

        // GET: GioHang
        public ActionResult Index()
        {

            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);


        }
        public ActionResult ThemVaoGio(int productId, int quantity)
        {
            var cart = Session[CartSession];
            var product = _db.Products.Find(productId);
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.Product.id == productId))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.id == productId)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    var item = new CartItem();
                    item.Product = _db.Products.Find(productId);
                    item.Quantity = quantity;
                    item.actual_number = (int)product.quantity;
                    list.Add(item);
                }
                Session[CartSession] = list;
            }
            else
            {
                var item = new CartItem();
                item.Product = _db.Products.Find(productId);
                item.Quantity = quantity;
                item.actual_number = (int)product.quantity;
                var list = new List<CartItem>();
                list.Add(item);

                Session[CartSession] = list;
            }
            return RedirectToAction("Index");
        }

        public JsonResult Edit(string cartModel)
        {
            var JsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var cartSec = (List<CartItem>)Session[CartSession];

            foreach (var item in cartSec)
            {
                var jsonItem = JsonCart.SingleOrDefault(x => x.Product.id == item.Product.id);
                if (jsonItem != null && item.actual_number >= jsonItem.Quantity)
                {
                    item.Quantity = jsonItem.Quantity;
                }
            }

            Session[CartSession] = cartSec;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult Delete(int id)
        {
            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.RemoveAll(x => x.Product.id == id);
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }
        public ActionResult ThanhToan()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        [HttpPost]
        public ActionResult ThanhToan(Order entity, long totalPrice)
        {

            var order = new Order();
            order.CreatedDate = DateTime.Now;
            order.User_id = Session["UserName"].ToString();  
            order.CustomerAddress = entity.CustomerAddress;
            order.CustomerEmail = entity.CustomerEmail;
            order.CustomerName = entity.CustomerName;
            order.CustomerPhone = entity.CustomerPhone;
            order.Descriptions = entity.Descriptions;
            order.status = false;
            _db.Orders.Add(order);
            _db.SaveChanges();
            //if(Session[CartSession] == null)
            //{
            //    var item = new CartItem();
            //    item.Product = _db.products.Find(productId);
            //    item.Quantity = 1;
            //    var list = new List<CartItem>();
            //    list.Add(item);

            //    Session[CartSession] = list;
            //}
            //SendEmail(email);

            //Lưu ngày đặt hàng, thêm tổng đơn hàng, thêm tổng tiền đã chi
            var user = _db.Users.Single(x => x.name == entity.CustomerName);
            user.buyLastDate = DateTime.Now;
            user.countOrder += 1;
            user.amountSpent += totalPrice;
            _db.SaveChanges();
            //try
            //{

                var cart = (List<CartItem>)Session[CartSession];
                foreach (var i in cart)
                {
                    var orderDetail = new Order_Detail();
                    orderDetail.Product_id = i.Product.id;
                    orderDetail.Order_id = order.id;
                    if (i.Product.newprice != null)
                    {
                      
                        orderDetail.Amount = i.Product.newprice * i.Quantity;
                    }
                    else
                    {
                        orderDetail.Amount = i.Product.price * i.Quantity;
                    }
                    
                    orderDetail.Quantity = i.Quantity;

                    _db.Order_Detail.Add(orderDetail);
                    _db.SaveChanges();
                }
                //thanh toán thành công thì cho giỏ hàng bằng null
                Session[CartSession] = null;
            //}
            //catch
            //{
            //    return Redirect("/loi-thanh-toan");
            //}

            return Redirect("/hoan-thanh");
        }
        public ActionResult Success()
        {
            
            return View();
            
        }
        public PartialViewResult HeaderCart()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return PartialView(list);
        }

        //Thanh toán online
        public ActionResult PaymentOnline(long id)
        {
            if(@Session["ten"] == null)
            {
                return Redirect("/dang-nhap");
            }
            else
            {
                var v = _db.Products.SingleOrDefault(x => x.id == id);
                ViewBag.PaymentOnline = v;
                return View();
            }
        }

        public ActionResult ValidateCommand()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ValidateCommand(string product, string totalPrice, string quantity, string name, string phone, string adress)
        {
            bool useSandbox = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSandbox"]);
            var paypal = new PayPalModel(useSandbox);

            //Thêm vào csdl
            var order = new Order();
            order.User_id = Session["UserName"].ToString();
            order.CustomerAddress = adress;
            order.CustomerName = name;
            order.CustomerPhone = phone;
            order.Descriptions = "OK";
            _db.Orders.Add(order);
            _db.SaveChanges();

            //Lưu ngày đặt hàng, thêm tổng đơn hàng, thêm tổng tiền đã chi
            var user = _db.Users.Single(x => x.name == name);
            user.buyLastDate = DateTime.Now;
            user.countOrder += 1;
            user.amountSpent += float.Parse(totalPrice);
            _db.SaveChanges();


            int price = int.Parse(totalPrice.ToString());
            price = price / 22000;
            paypal.item_name = product;
            paypal.amount = price.ToString();
            paypal.item_quantity = quantity;
            return View(paypal);
        }
        public ActionResult RedirectFromPaypal()
        {
            return View();
        }

        public ActionResult CancelFromPaypal()
        {
            return View();
        }

        public ActionResult NotifyFromPaypal()
        {
            return View();
        }

        //send email
        public void SendEmail(string address)
        {
            string email = "superjunior25251325@gmail.com";
            string password = "";
            var loginInfo = new NetworkCredential(email, password);
            var msg = new MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);

            var cart = (List<CartItem>)Session[CartSession];
            string productName = "";
            foreach(var item in cart)
            {
                productName += item.Product.name + ", ";
            }
            msg.From = new MailAddress(email);
            msg.To.Add(new MailAddress(address));
            msg.Subject = "Website bán điện thoại xác nhận.";
            msg.Body = "Bạn đã đặt hàng <a><u>" + productName + "</u></a> từ hệ thống website của chúng tôi. </br>";
            msg.Body += "Hàng của bạn sẽ được nhân viên chăm sóc khách hàng xác nhận địa chỉ và giao trong thời gian ngắn nhất cho bạn. </br>";
            msg.Body += "Chi tiết liên hệ hotline: 1900 1000 .Cám ơn quý khách đã ủng hộ cửa hàng.";
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);
        }



    }
}