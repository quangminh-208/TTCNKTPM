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
    public class SlideController : Controller
    {
        
        private WebMayTinhEntities db = new WebMayTinhEntities();
        // GET: admin/Slide
        public ActionResult Index()
        {
            return View(db.Slides.ToList());
        }

        // GET: admin/Slide/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slide Slide = db.Slides.Find(id);
            if (Slide == null)
            {
                return HttpNotFound();
            }
            return View(Slide);
        }

        // GET: admin/Slide/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admin/Slide/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "id,img,meta,hide,order,datebegin")] Slide Slide)
        {
            if (ModelState.IsValid)
            {
                Slide.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                db.Slides.Add(Slide);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(Slide);
        }

        // GET: admin/Slide/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slide Slide = db.Slides.Find(id);
            if (Slide == null)
            {
                return HttpNotFound();
            }
            return View(Slide);
        }

        // POST: admin/Slide/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "id,img,meta,hide,order,datebegin")] Slide Slide)
        {
            if (ModelState.IsValid)
            {
                Slide.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                db.Entry(Slide).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Slide);
        }

        // GET: admin/Slide/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slide Slide = db.Slides.Find(id);
            if (Slide == null)
            {
                return HttpNotFound();
            }
            return View(Slide);
        }

        // POST: admin/Slide/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Slide Slide = db.Slides.Find(id);
            db.Slides.Remove(Slide);
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
