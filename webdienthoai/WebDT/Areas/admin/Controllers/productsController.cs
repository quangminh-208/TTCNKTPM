using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebDT.Models;
using WebDT.Models.EF;

namespace WebDT.Areas.admin.Controllers
{
    public class ProductsController : Controller
    {
       
        private WebMayTinhEntities db = new WebMayTinhEntities();

        // GET: admin/Products
        public ActionResult Index(int page = 1, int pagesize = 10)
        {
            var model = db.Products.OrderBy(x => x.order).ToPagedList(page, pagesize);
            return View(model);
        }

   
        public void getCategory(long? selectedId = null)
        {
            ViewBag.Category = new SelectList(db.Categories.Where(x => x.hide == true).OrderBy(x => x.order), "id", "name", selectedId);
        }

        public JsonResult ListName(string q)
        {
            var query = db.Products.Where(x => x.name.Contains(q)).Select(x => x.name);
            return Json(new
            {
                data = query,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search(string keyword, int page = 1, int pagesize = 10)
        {
            string[] key = keyword.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            var product_name = new List<Product>();//Tìm theo tên sản phẩm
            foreach (var item in key)
            {
                product_name = (from b in db.Products
                                where b.name.Contains(item)
                                select b).ToList();
            }
            ViewBag.KeyWord = keyword;
            return View(product_name.ToPagedList(page, pagesize));
        }

        public ActionResult getProduct(long? id)
        {
            if (id == null)
            {
                var v = db.Products.OrderBy(x => x.order).ToList();
                return PartialView(v);
            }
            var m = db.Products.Where(x => x.categoryid == id).OrderBy(x => x.order).ToList();
            return PartialView(m);
        }
        // GET: admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product Product = db.Products.Find(id);
            if (Product == null)
            {
                return HttpNotFound();
            }
            return View(Product);
        }

        // GET: admin/Products/Create
        public ActionResult Create()
        {
            getCategory();
            return View();
        }

        // POST: admin/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "id,name,price,newprice,img,description,meta,sale,percent,hdie,order,datebegin,categoryid,quantity")] Product Product, HttpPostedFileBase img)
        {
            if (ModelState.IsValid)
            {
                var path = Path.Combine(Server.MapPath("~/Content/img"), img.FileName);
                if (System.IO.File.Exists(path))
                {
                    string extensionName = Path.GetExtension(img.FileName);
                    string filename = img.FileName + DateTime.Now.ToString("ddMMyyyy") + extensionName;
                    path = Path.Combine(Server.MapPath("~/Content/img"), filename);
                    img.SaveAs(path);
                    Product.img = filename;
                }
                else
                {
                    img.SaveAs(path);
                    Product.img = img.FileName;
                }
                

                Product.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                Product.meta = WebDT.Help.Functions.ConvertToUnSign(Product.name);
                Product.order = getMaxOrder(Product.categoryid);
                Product.hide = true;


                db.Products.Add(Product);
                db.SaveChanges();
                return RedirectToAction("Index", "Products", new { id = Product.categoryid });
            }

            return View(Product);
        }
        public int getMaxOrder(long? CategoryId)
        {
            return db.Products.Where(x => x.categoryid == CategoryId).Count();
        }


        // GET: admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product Product = db.Products.Find(id);
            if (Product == null)
            {
                return HttpNotFound();
            }
            getCategory(Product.categoryid);
            ViewBag.listCategory = db.Categories.ToList();
            return View(Product);
        }

        // POST: admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "id,name,price,newprice,img,description,meta,hdie,order,datebegin,categoryid,quantity")] Product Product, HttpPostedFileBase img)
        {
            if (ModelState.IsValid)
            {
                var model = db.Products.Find(Product.id);
                if (img != null && model.img != img.FileName)
                {
                    //Xóa file cũ
                    System.IO.File.Delete(Path.Combine(Server.MapPath("~/Content/img"), model.img));
                    //Thêm hình ảnh
                    var path = Path.Combine(Server.MapPath("~/Content/img"), img.FileName);
                    if (System.IO.File.Exists(path))
                    {
                        string extensionName = Path.GetExtension(img.FileName);
                        string filename = img.FileName + DateTime.Now.ToString("ddMMyyyy") + extensionName;
                        path = Path.Combine(Server.MapPath("~/Content/img"), filename);
                        img.SaveAs(path);
                        model.img = filename;
                    }
                    else
                    {
                        img.SaveAs(path);
                        model.img = img.FileName;
                    }
                }
                
                model.name = Product.name;
                model.price = Product.price;
                model.newprice = Product.newprice;
                model.description = Product.description;
                model.meta = WebDT.Help.Functions.ConvertToUnSign(Product.name);
                model.hide = Product.hide;
                model.order = Product.order;
                model.categoryid = Product.categoryid;
                model.quantity = Product.quantity;
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Product);
        }

        // GET: admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product Product = db.Products.Find(id);
            if (Product == null)
            {
                return HttpNotFound();
            }
            return View(Product);
        }

        // POST: admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product Product = db.Products.Find(id);
            System.IO.File.Delete(Path.Combine(Server.MapPath("~/Content/img"), Product.img));
            db.Products.Remove(Product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        
    }
}
