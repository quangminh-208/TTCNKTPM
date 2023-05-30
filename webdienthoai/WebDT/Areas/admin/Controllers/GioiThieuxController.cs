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
    public class GioiThieuxController : Controller
    {
        
        private WebMayTinhEntities db = new WebMayTinhEntities();
        // GET: admin/GioiThieux
        public ActionResult Index()
        {
            return View(db.Introduces.ToList());
        }

        // GET: admin/GioiThieux/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Introduce gioiThieu = db.Introduces.Find(id);
            if (gioiThieu == null)
            {
                return HttpNotFound();
            }
            return View(gioiThieu);
        }

        // GET: admin/GioiThieux/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admin/GioiThieux/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "id,NoiDung,order,meta,datebegin")] Introduce gioiThieu)
        {
          
            if (ModelState.IsValid)
            {
                gioiThieu.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                db.Introduces.Add(gioiThieu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gioiThieu);
        }

        // GET: admin/GioiThieux/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Introduce gioiThieu = db.Introduces.Find(id);
            if (gioiThieu == null)
            {
                return HttpNotFound();
            }
            return View(gioiThieu);
        }

        // POST: admin/GioiThieux/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "id,NoiDung,order,meta,datebegin")] Introduce gioiThieu)
        {
            if (ModelState.IsValid)
            {
                gioiThieu.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                db.Entry(gioiThieu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gioiThieu);
        }

        // GET: admin/GioiThieux/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Introduce gioiThieu = db.Introduces.Find(id);
            if (gioiThieu == null)
            {
                return HttpNotFound();
            }
            return View(gioiThieu);
        }

        // POST: admin/GioiThieux/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Introduce gioiThieu = db.Introduces.Find(id);
            db.Introduces.Remove(gioiThieu);
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
