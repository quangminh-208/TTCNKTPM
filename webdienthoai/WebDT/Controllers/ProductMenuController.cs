using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDT.Models;
using WebDT.Models.EF;

namespace WebDT.Controllers
{
    public class ProductMenuController : Controller
    {
        WebMayTinhEntities _db = new WebMayTinhEntities();
        // GET: ProductMenu
        public ActionResult Index(string meta)
        {
            var i = from t in _db.Categories where t.meta == meta && t.hide == true select t;
            return View(i.FirstOrDefault());
        }
    }
}