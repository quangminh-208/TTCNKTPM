using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDT.Models;
using WebDT.Models.EF;

namespace WebDT.Controllers
{
    public class CategoryController : Controller
    {
        WebMayTinhEntities _db = new WebMayTinhEntities();
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult getCategory1()
        {
            ViewBag.meta = "san-pham";
            var v = from t in _db.Categories
                    where t.hide == true
                    orderby t.order ascending
                    select t;
            return PartialView(v.ToList());
        }
    }
}