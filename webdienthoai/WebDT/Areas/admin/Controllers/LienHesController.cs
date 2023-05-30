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
    public class LienHesController : Controller
    {
        
        private WebMayTinhEntities db = new WebMayTinhEntities();
        // GET: admin/LienHes
        public ActionResult Index()
        {
            return View(db.Contacts.ToList());
        }

        // GET: admin/LienHes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact lienHe = db.Contacts.Find(id);
            if (lienHe == null)
            {
                return HttpNotFound();
            }
            return View(lienHe);
        }

        // GET: admin/LienHes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admin/LienHes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "id,HoTen,Email,NoiDung,NgayLap")] Contact lienHe)
        {
            if (ModelState.IsValid)
            {
                lienHe.CreatedDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                db.Contacts.Add(lienHe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lienHe);
        }

        // GET: admin/LienHes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact lienHe = db.Contacts.Find(id);
            if (lienHe == null)
            {
                return HttpNotFound();
            }
            return View(lienHe);
        }

        // POST: admin/LienHes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "id,HoTen,Email,NoiDung,NgayLap")] Contact lienHe)
        {
            if (ModelState.IsValid)
            {
                lienHe.CreatedDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                db.Entry(lienHe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lienHe);
        }

        // GET: admin/LienHes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact lienHe = db.Contacts.Find(id);
            if (lienHe == null)
            {
                return HttpNotFound();
            }
            return View(lienHe);
        }

        // POST: admin/LienHes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact lienHe = db.Contacts.Find(id);
            db.Contacts.Remove(lienHe);
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
