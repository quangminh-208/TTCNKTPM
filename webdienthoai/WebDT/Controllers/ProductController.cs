using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebDT.Models;
using WebDT.Models.EF;

namespace WebDT.Controllers
{
    public class ProductController : Controller
    {
        WebMayTinhEntities _db = new WebMayTinhEntities();
        // GET: Product
        //tìm kiếm nâng cao
        public ActionResult Index(int category = 0, int toprice = 0, int fromprice = 0)
        {
            var model = new List<Product>();
            if (toprice != -1 && fromprice != 0)
            {
                if (toprice == 0)
                    ViewBag.tag = "Dưới 2 triệu";
                else if (toprice == 2)
                    ViewBag.tag = "Từ 2 - 4 triệu";
                else if (toprice == 4)
                    ViewBag.tag = "Từ 4 - 7 triệu";
                else if (toprice == 7)
                    ViewBag.tag = "Từ 7 - 13 triệu";
                else
                    ViewBag.tag = "Trên 13 triệu";

                toprice = toprice * 1000000;
                fromprice = fromprice * 1000000;
                if (category != 0)//tìm kiếm theo loại máy
                {
                    foreach(var item in _db.Categories.ToList())
                    {
                        if(category == item.id)
                            ViewBag.cate = item.name;
                    }

                    var product = from p in _db.Products
                                  where p.price >= toprice && p.price <= fromprice && p.categoryid == category
                                  select p;
                    model = product.ToList();
                }
                else
                {
                    var product = from p in _db.Products
                                  where p.price >= toprice && p.price <= fromprice
                                  select p;
                    model = product.ToList();
                }

            }
            //Nếu có tìm kiếm theo loại
            else if (category != 0)
            {
                foreach (var item in _db.Categories.ToList())
                {
                    if (category == item.id)
                        ViewBag.cate = item.name;
                }

                var product = from p in _db.Products
                              where p.categoryid == category
                              select p;
                model = product.ToList();
            }
            ViewBag.dtdd = _db.Categories.ToList();
            return View(model);
        }

        List<Product> product_seen = new List<Product>();
        public ActionResult Detail(long id)
        {
            if(Session["ProductSeen"] != null)
                product_seen = (List<Product>)Session["ProductSeen"];

            var v = _db.Products.Single(x => x.id == id);

            //Đễ xuất sản phẩm mới bằng giá tiền chênh lệch là 5 triệu
            var dexuat = _db.Products.Where(x => x.price >= (v.price - 5000000) && x.price <= (v.price + 5000000)).ToList();
            if (dexuat.Exists(x => x.id == id))//Nếu sản phẩm đề xuất trùng với sản phẩm đang xem
            {
                var pro = dexuat.Single(x => x.id == id);
                dexuat.Remove(pro);
            }
                
            //Sản phẩm đã xem
            if(product_seen.Count != 0)
            {
                var item = v;
                //nếu sản phẩm đã xem trước đó
                if (product_seen.Exists(x => x.id == item.id)) {
                    product_seen.Add(item);
                    product_seen.Remove(item);
                }
                else                    
                    product_seen.Add(item);
            }
            else
            {
                product_seen.Add(v);
            }
            
            Session["ProductSeen"] = product_seen;
            ViewBag.dexuat = dexuat;
            ViewBag.lstReview = _db.Reviews.Where(x => x.Product_id == id).OrderByDescending(x => x.CreatedDate).ToList();
            return View(v);
        }
        public ActionResult sanphamcungloai(int id)
        {
            var spcl = from sp in _db.Products
                       join lsp in _db.Categories 
                       on sp.categoryid equals lsp.id 

                       where sp.categoryid == lsp.id && sp.categoryid == id
                       select sp;
            return PartialView(spcl);
        }

        public ActionResult GetProduct(int id , int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 12;
            var i = from t in _db.Products
                    where t.categoryid == id && t.hide == true orderby t.order ascending
                    select t;
          //  return PartialView(i.ToList());
            return PartialView(i.ToPagedList(pageNumber, pageSize));
        }

        public JsonResult addReview(string json_review)
        {
            var JsonReview = new JavaScriptSerializer().Deserialize<List<ReviewDTO>>(json_review);
            var review = new Review();
            foreach (var item in JsonReview)
            {
                review.Content = item.Content;
                review.Rating = item.Rating;
                review.CreatedDate = DateTime.Now;
                review.User_id = item.User_id;
                review.Product_id = item.Product_id;
                review.Status = true;
            }
            _db.Reviews.Add(review);
            _db.SaveChanges();
            return Json(new
            {
                status = true
            });

        }

    }
}

