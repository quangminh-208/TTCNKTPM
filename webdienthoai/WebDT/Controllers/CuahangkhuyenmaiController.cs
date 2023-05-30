using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDT.Models;
using WebDT.Models.EF;

namespace WebDT.Controllers
{
    public class CuahangkhuyenmaiController : Controller
    {
        WebMayTinhEntities _db = new WebMayTinhEntities();
        // GET: Cuahangkhuyenmai
        public ActionResult Index(int? page)
        {

            int pageNumber = (page ?? 1);
            int pageSize = 12;
            var v = from t in _db.Products
                    where t.hide == true && t.newprice != null
                    orderby t.datebegin ascending
                    select t;
            return PartialView(v.ToPagedList(pageNumber, pageSize));
        }
        
    }
}