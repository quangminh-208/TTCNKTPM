using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDT.Areas.admin.Models;
using WebDT.Models;
using WebDT.Models.DTO;
using WebDT.Models.EF;

namespace WebDT.Areas.admin.Controllers
{
    public class HoaDonController : Controller
    {
        private WebMayTinhEntities db = new WebMayTinhEntities();
        // GET: admin/HoaDon
        public ActionResult Index(int page = 1, int pagesize = 10)
        {
            var model = db.Orders.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pagesize);
            //var model = from gh in db.Orders
            //            join ct in db.Order_Detail on gh.id equals ct.Order_id
            //            select new HoaDon
            //            {
            //                gioHangId = gh.id,
            //                TenKhachHang = gh.CustomerName,
            //                DiaChi = gh.CustomerAddress,
            //                SDTKhachHang = gh.CustomerPhone,
            //                Email = gh.CustomerEmail,
            //                NgayTao = gh.CreatedDate,
            //                status = gh.status
            //            };
            return View(model);
        }

        public ActionResult DetailOrder(int gioHangId)
        {
            var khachhang = db.Orders.Find(gioHangId);
            ViewBag.TenKhachHang = khachhang.User_id;

            var model = from pr in db.Products
                        join ct in db.Order_Detail on pr.id equals ct.Product_id
                        where ct.Order_id == gioHangId
                        select new HoaDon
                        {
                            name = pr.name,
                            img = pr.img,
                            Tien = pr.price,
                            SoLuong = ct.Quantity
                        };
            ViewBag.gioHangID = gioHangId;
            return View(model.Distinct().ToList());
        }

        public JsonResult ProcessOrder(int id)
        {
            var giohang = db.Orders.Find(id);
            giohang.status = true;

            new ProductModel().UpdateSoLuong(id);
            db.SaveChanges();
            return Json(new
            {
                status = true
            });
        }


        public ActionResult TotalSale()
        {
            int[] month = new int[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var lstTotal = new List<TotalSale>();
            for(int i = 1; i <= 12; i++)
            {
                var model = (from ct in db.Order_Detail
                            join gh in db.Orders on ct.Order_id equals gh.id
                            where gh.CreatedDate.Value.Month == i
                            select new TotalSale
                            {
                                total = ct.Amount
                            }).Sum(x => x.total);
                var totalsale = new TotalSale();
                var mon = i;
                var total = model;
                totalsale.month = mon;
                if (total != null)
                    totalsale.total = total;
                else
                    totalsale.total = 0;
                lstTotal.Add(totalsale);
            }
            return Json(lstTotal, JsonRequestBehavior.AllowGet);
            
        }

        public ActionResult SaleChart()
        {
            return View();
        }

        public ActionResult ThongKe()
        {
            //thống kê sp bán ra
            var sell = from detail in db.Order_Detail
                       join order in db.Orders on detail.Order_id equals order.id
                       where order.status == true
                       select new
                       {
                           detail.Quantity,
                           detail.Amount
                       };
            ViewBag.Count_sell = sell.Select(x => x.Quantity).Sum();

            //Thống kê doanh thu
            ViewBag.Count_money = sell.Select(x => x.Amount).Sum();

            //Thống kê đơn đặt hàng
            ViewBag.Count_Order = db.Orders.ToList().Count();


            //Thống kê đơn hàng đã thanh toán
            ViewBag.Count_OrderSuccess = db.Orders.Where(x => x.status == true).ToList().Count();


            //Thống kê lượng hàng bán ra
            var lstproduct_id = db.Order_Detail.Select(x => x.Product_id).Distinct();
            var listProduct_sell = new List<Order_DetailDTO>();
            foreach (var item in lstproduct_id)
            {
                var product = db.Products.Find(item);
                var productsell = new Order_DetailDTO();
                productsell.Product_Name = product.name;
                productsell.Quantity = 0;
                productsell.Amount = 0;
                foreach (var jtem in db.Order_Detail.Where(x => x.Product_id == item))
                {
                    productsell.Quantity += jtem.Quantity;
                    productsell.Amount += (int)jtem.Amount;
                }
                listProduct_sell.Add(productsell);
            }

            ViewBag.product_sell = listProduct_sell.OrderByDescending(x => x.Quantity).Take(10).ToList();

            //Thống kê lượng hàng tồn kho
            ViewBag.QuantityProduct = db.Products.OrderByDescending(x => x.quantity).Take(10).ToList();

            //Thống kê đánh giá hôm nay
            ViewBag.Review_today = db.Reviews.Where(x => DbFunctions.TruncateTime(x.CreatedDate) == DbFunctions.TruncateTime(DateTime.Now)).Count();

            //Thống kê đơn đạt hàng hôm nay
            ViewBag.Order_today = db.Orders.Where(x => DbFunctions.TruncateTime(x.CreatedDate) == DbFunctions.TruncateTime(DateTime.Now)).Count();

            //Thống kê user đã đăng ký
            ViewBag.user = db.Users.Count();

            //Đơn hàng đã thanh toán
            ViewBag.OrderPait = db.Orders.Where(x => x.status == true).Count();

            //Đơn hàng đang chờ thanh toán
            ViewBag.Ordering = db.Orders.Where(x => x.status == false).Count();
            return View();
        }
    }
}