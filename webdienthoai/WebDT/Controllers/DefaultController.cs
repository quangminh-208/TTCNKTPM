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
    public class DefaultController : Controller
    {
        WebMayTinhEntities _db = new WebMayTinhEntities();
        // GET: Default
        public ActionResult Index()
        {
            ViewBag.getHinhChieu = _db.Slides.OrderByDescending(x => x.order).ToList();
            return View();
        }
        public ActionResult getHinhChieu()
        {
            var v = _db.Slides.OrderByDescending(x => x.order);
            return PartialView(v.ToList());
        }
        public ActionResult getSPMoi()
        {
            var v = _db.Products.OrderByDescending(x =>x.datebegin);
            return PartialView(v.ToList());
        }
        public ActionResult getThuongHieu()
        {
            var v = from t in _db.Categories
                    where t.hide == true
                    orderby t.order ascending
                    select t;
            return PartialView(v.ToList());
        }

        public ActionResult getGioiThieu()
        {
            ViewBag.meta = "gioi-thieu";
            var v = from t in _db.Introduces
                    where t.meta == "gioi-thieu"
                    orderby t.order ascending
                    select t;
            return PartialView(v.ToList());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

 
        public ActionResult getCategory()
        {
            ViewBag.meta = "san-pham";
            var v = from t in _db.Categories
                    where t.hide == true
                    orderby t.datebegin ascending
                    select t;
            return PartialView(v.ToList());            
        }

      

        public ActionResult getTinTuc()
        {
            ViewBag.meta = "tin-tuc-su-kien";
            var v = from t in _db.News
                    where t.meta != null
                    orderby t.order ascending
                    select t;
            return PartialView(v.ToList());
        }
        public ActionResult getKhuyenMai()
        {
            var v = from t in _db.Products
                    where t.hide == true && t.price != null
                    orderby t.datebegin ascending
                    select t;
            return PartialView(v.ToList());
        }
        public ActionResult getXemNhanhSP()
        {
            var list = from sp in _db.Products
                       where sp.hide == true
                       select sp;
            return PartialView(list.ToList());
        }
        

        [HttpGet]
        public JsonResult getTimKiemGia(double toprice, double fromprice)
        {
            if (fromprice == 0)
            {
                toprice = toprice * 1000000;
                var product = from p in _db.Products
                              where p.price >= toprice
                              orderby p.datebegin ascending
                              select p;
                return Json(product, JsonRequestBehavior.AllowGet);
            }
            else
            {
                toprice = toprice * 1000000;
                fromprice = fromprice * 1000000;
                var product = from p in _db.Products
                              where p.price >= toprice && p.price <= fromprice
                              orderby p.datebegin ascending
                              select p;
                return Json( new { data = product.ToList() }, JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}