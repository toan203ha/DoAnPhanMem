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
    public class DonHangAdminController : Controller
    {
        private QL_CHDTEntities db = new QL_CHDTEntities();

        // GET: DonHangAdmin
        public ActionResult Index()
        {
            var dONDATHANGs = db.DONDATHANGs.Include(d => d.KhachHang);
            return View(dONDATHANGs.ToList());
        }

        public ActionResult Xacnhan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONDATHANG donhang = db.DONDATHANGs.Find(id);
            //donhang.Dagiao = true;
            if (donhang.Dagiao == true)
            {
                donhang.Dagiao = false;
            }
            else if(donhang.Dagiao == false)
            {
                donhang.Dagiao = true;
            }
            db.SaveChanges();
            if (donhang == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }
        // GET: DonHangAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONDATHANG dONDATHANG = db.DONDATHANGs.Find(id);
            if (dONDATHANG == null)
            {
                return HttpNotFound();
            }
            return View(dONDATHANG);
        }

        //public ActionResult DonHangChiTiet(int iddonhang)
        //{
        //     var cTDATHANGs = db.CTDATHANGs
        //            .Include(c => c.DONDATHANG)   
        //            .Include(c => c.SanPham)
        //            .Where(c => c.SODH == iddonhang)
        //            .ToList();
        //        return View(cTDATHANGs); 
        //}

        public ActionResult DonHangChiTiet(int? id)
        {
            // Thực hiện truy vấn cơ sở dữ liệu để lấy danh sách chi tiết đơn hàng dựa vào SODH

            var cTDATHANGs = db.CTDATHANGs.Include(c => c.DONDATHANG).Include(c => c.SanPham).Where(c => c.SODH == id);
            return View(cTDATHANGs.ToList());
        }

        // GET: DonHangAdmin/Create
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH");
            return View();
        }

        // POST: DonHangAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SODH,MaKH,NgayDH,Dagiao,Ngaygiaohang,Tennguoinhan,Diachinhan,Trigia,Dienthoainhan,HTThanhtoan,HTGiaohang")] DONDATHANG dONDATHANG)
        {
            if (ModelState.IsValid)
            {
                db.DONDATHANGs.Add(dONDATHANG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH", dONDATHANG.MaKH);
            return View(dONDATHANG);
        }

        // GET: DonHangAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONDATHANG dONDATHANG = db.DONDATHANGs.Find(id);
            if (dONDATHANG == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH", dONDATHANG.MaKH);
            return View(dONDATHANG);
        }

        // POST: DonHangAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SODH,MaKH,NgayDH,Dagiao,Ngaygiaohang,Tennguoinhan,Diachinhan,Trigia,Dienthoainhan,HTThanhtoan,HTGiaohang")] DONDATHANG dONDATHANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dONDATHANG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH", dONDATHANG.MaKH);
            return View(dONDATHANG);
        }

        // GET: DonHangAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONDATHANG dONDATHANG = db.DONDATHANGs.Find(id);
            if (dONDATHANG == null)
            {
                return HttpNotFound();
            }
            return View(dONDATHANG);
        }

        // POST: DonHangAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DONDATHANG dONDATHANG = db.DONDATHANGs.Find(id);
            db.DONDATHANGs.Remove(dONDATHANG);
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
