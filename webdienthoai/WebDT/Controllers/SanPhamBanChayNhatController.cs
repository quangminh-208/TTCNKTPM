using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDT.Models;
using WebDT.Models.EF;

namespace WebDT.Controllers
{
    public class SanPhamBanChayNhatController : Controller
    {
        WebMayTinhEntities db = new WebMayTinhEntities();
        // GET: SanPhamBanChayNhat
        public ActionResult Index()
        {

            List<BanChay> models = null;
            models = (
                          from SPham  in db.Products
                          join CTiet in db.Order_Detail
                          on  SPham.id equals CTiet.Product_id
                          group SPham
                          by new
                          {
                              SPham.id,
                              SPham.name,
                              SPham.meta,
                              SPham.img,
                              SPham.newprice,
                              SPham.price
                          } into grb
                          select new BanChay()
                          {
                              Id = grb.Key.id,
                              HinhAnh = grb.Key.img,
                              Ten_sp = grb.Key.name,
                              meta = grb.Key.meta,
                              Price = grb.Key.price,
                              NewPrice = grb.Key.newprice,
                              Tong_SL = db.Order_Detail.Where(m => m.Product_id == grb.Key.id).Sum(i => i.Quantity)
                          }
                  ).OrderByDescending(m => m.Tong_SL).ToList();
              return View(models);
        }
    }
}