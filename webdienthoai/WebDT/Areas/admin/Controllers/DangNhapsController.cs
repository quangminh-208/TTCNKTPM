using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebDT.Models;
using WebDT.Models.EF;

namespace WebDT.Areas.admin.Controllers
{
    public class DangNhapsController : Controller
    {
       
        private WebMayTinhEntities db = new WebMayTinhEntities();
        // GET: admin/DangNhaps
        public ActionResult Index()
        {
                var list = from DSKhachHang in db.Users
                           where DSKhachHang.Quyenadmin == null
                           select DSKhachHang;
                return View(list.ToList());
        }

        public JsonResult ChangeStatus(int id)
        {
            var user = db.Users.Find(id);
            if (user.status == true)
                user.status = false;
            else
                user.status = true;

            db.SaveChanges();
            return Json(new
            {
                status = true
            });
        }

        // GET: admin/DangNhaps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User dangNhap = db.Users.Find(id);
            if (dangNhap == null)
            {
                return HttpNotFound();
            }
            return View(dangNhap);
        }

        //Chart detail 
        public ActionResult UserChart()
        {
            var model = db.Users.Where(x => x.buyLastDate != null).ToList();
            var lstUser = new List<userChart>();

            //Tính ngày mua hàng gần nhất
            foreach(var item in model)
            {
                var userChart = new userChart();
                TimeSpan time = DateTime.Now - item.buyLastDate.Value;
                int Tongngay = time.Days;
                int soDonHang = int.Parse(item.countOrder.ToString());
                float soTienMua = float.Parse(item.amountSpent.ToString());
                userChart.Tongngay = Tongngay;
                userChart.soDonHang = soDonHang;
                userChart.soTienMua = soTienMua / 10000000;
                userChart.name = item.name;
                
                lstUser.Add(userChart);
            }
            return Json(lstUser, JsonRequestBehavior.AllowGet);
        }

        // GET: admin/DangNhaps/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admin/DangNhaps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,username,password,name,address,email,phone,status")] User dangNhap)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(dangNhap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dangNhap);
        }

        // GET: admin/DangNhaps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User dangNhap = db.Users.Find(id);
            if (dangNhap == null)
            {
                return HttpNotFound();
            }
            return View(dangNhap);
        }

        // POST: admin/DangNhaps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,username,password,name,address,email,phone,status")] User dangNhap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dangNhap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dangNhap);
        }

        // GET: admin/DangNhaps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User dangNhap = db.Users.Find(id);
            if (dangNhap == null)
            {
                return HttpNotFound();
            }
            return View(dangNhap);
        }

        // POST: admin/DangNhaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User dangNhap = db.Users.Find(id);
            db.Users.Remove(dangNhap);
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
        public ActionResult Logout()
        {
            Session.Abandon();
            return Redirect("http://localhost:54398");
        }
    }
}
