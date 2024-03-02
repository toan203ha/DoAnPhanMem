using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Doanphanmem.Models;

namespace Doanphanmem.Admin.Controllers
{
    public class MauAdminController : Controller
    {
        private QL_CHDTEntities db = new QL_CHDTEntities();

        // GET: MauAdmin
        public ActionResult Index()
        {
            return View(db.Mau.ToList());
        }

        // GET: MauAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mau mau = db.Mau.Find(id);
            if (mau == null)
            {
                return HttpNotFound();
            }
            return View(mau);
        }

        // GET: MauAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MauAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Tenmau,Mamau")] Mau mau)
        {
            if (ModelState.IsValid)
            {
                db.Mau.Add(mau);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mau);
        }

        // GET: MauAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mau mau = db.Mau.Find(id);
            if (mau == null)
            {
                return HttpNotFound();
            }
            return View(mau);
        }

        // POST: MauAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Tenmau,Mamau")] Mau mau)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mau).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mau);
        }

        // GET: MauAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mau mau = db.Mau.Find(id);
            if (mau == null)
            {
                return HttpNotFound();
            }
            return View(mau);
        }

        // POST: MauAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mau mau = db.Mau.Find(id);
            db.Mau.Remove(mau);
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
