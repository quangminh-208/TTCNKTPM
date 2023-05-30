using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDT.Models;
using WebDT.Models.EF;

namespace WebDT.Controllers
{
    public class TempController : Controller
    {
        WebMayTinhEntities _db = new WebMayTinhEntities();
        // GET: Temp
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult getMenu()
        {
            var v = from t in _db.Menus
                    where t.hide == true
                    orderby t.order ascending
                    select t;
            ViewBag.Category = _db.Categories.ToList();
            return PartialView(v.ToList());
        }
        
    }
}