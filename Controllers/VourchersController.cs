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
        //public ActionResult Index()
        //{
        //    var vourcher = db.Vourchers.Include(v => v.SanPham);
        //    return View(vourcher.ToList());
        //}

        public ActionResult Index(String SearchString)
        {
            if (Session["taikhoan"] == null)
                return RedirectToAction("Login", "Account");
            var vourcher = db.Vourchers.Include(v => v.SanPham);
            if (!String.IsNullOrEmpty(SearchString))
            {
                vourcher = vourcher.Where(s => s.SanPham.TenSP.Contains(SearchString));
            }


            {
                Console.WriteLine("Không tìm thấy sản phẩm nào");
            }
            return View(vourcher.ToList());
        }

        // GET: Vourchers/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["taikhoan"] == null)
                return RedirectToAction("Login", "Account");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vourcher vourcher = db.Vourchers.Find(id);
            if (vourcher == null)
            {
                return HttpNotFound();
            }
            return View(vourcher);
        }

        // GET: Vourchers/Create
        public ActionResult Create()
        {
            if (Session["taikhoan"] == null)
                return RedirectToAction("Login", "Account");
            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP");
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
                db.Vourchers.Add(vourcher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", vourcher.MaSP);
            return View(vourcher);
        }

        // GET: Vourchers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["taikhoan"] == null)
                return RedirectToAction("Login", "Account");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vourcher vourcher = db.Vourchers.Find(id);
            if (vourcher == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", vourcher.MaSP);
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
            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", vourcher.MaSP);
            return View(vourcher);
        }

        // GET: Vourchers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["taikhoan"] == null)
                return RedirectToAction("Login", "Account");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vourcher vourcher = db.Vourchers.Find(id);
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
            Vourcher vourcher = db.Vourchers.Find(id);
            db.Vourchers.Remove(vourcher);
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
