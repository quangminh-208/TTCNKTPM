using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDT.Models;

namespace WebDT.Areas.admin.Controllers
{
    public class DefaultsController : Controller
    {
        // GET: admin/Defaults
        public ActionResult Index()
        {
            if (Session[CommonConstants.USER_SESSION] == null)
            {
                return Redirect("/dang-nhap");
            }
            else
            {
                return View();
            }
        }
    }
}