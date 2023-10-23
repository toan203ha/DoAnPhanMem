using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Doanphanmem.Models;

namespace Doanphanmem.Controllers
{
    public class VourchersController : Controller
    {
        private QL_CHDTEntities db = new QL_CHDTEntities();

        // GET: Vourchers
        public ActionResult Index()
        {
            var vourcher = db.Vourcher.Include(v => v.SanPham);
            return View(vourcher.ToList());
        }

        // GET: Vourchers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vourcher vourcher = db.Vourcher.Find(id);
            if (vourcher == null)
            {
                return HttpNotFound();
            }
            return View(vourcher);
        }

        // GET: Vourchers/Create
        public ActionResult Create()
        {
            ViewBag.MaSP = new SelectList(db.SanPham, "MaSP", "TenSP");
            return View();
        }

        // POST: Vourchers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKM,Uudai,ThongTinUuDai,MaSP")] Vourcher vourcher)
        {
            if (ModelState.IsValid)
            {
                db.Vourcher.Add(vourcher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaSP = new SelectList(db.SanPham, "MaSP", "TenSP", vourcher.MaSP);
            return View(vourcher);
        }

        // GET: Vourchers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vourcher vourcher = db.Vourcher.Find(id);
            if (vourcher == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaSP = new SelectList(db.SanPham, "MaSP", "TenSP", vourcher.MaSP);
            return View(vourcher);
        }

        // POST: Vourchers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKM,Uudai,ThongTinUuDai,MaSP")] Vourcher vourcher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vourcher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaSP = new SelectList(db.SanPham, "MaSP", "TenSP", vourcher.MaSP);
            return View(vourcher);
        }

        // GET: Vourchers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vourcher vourcher = db.Vourcher.Find(id);
            if (vourcher == null)
            {
                return HttpNotFound();
            }
            return View(vourcher);
        }

        // POST: Vourchers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vourcher vourcher = db.Vourcher.Find(id);
            db.Vourcher.Remove(vourcher);
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
