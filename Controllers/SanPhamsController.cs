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
    public class SanPhamsController : Controller
    {
        private QL_CHDTEntities db = new QL_CHDTEntities();


        //Bộ lọc
        //public ActionResult Filter(string ten,int gia)
        //{
 
        //}

        // danh sách đơn hàng của khách hàng
        public ActionResult DonHangKH(int IDCus)
        {
            // Truy vấn dữ liệu từ cơ sở dữ liệu dựa trên IDCus
            IDCus = (int)Session["UserID"];
            var orders = db.DONDATHANG
                .Where(o => o.MaKH == IDCus)
                .Include(o => o.CTDATHANG)  
                .ToList();
            return View(orders);
        }
 
        public ActionResult getCus(int ID)
        {
            var cus = db.SanPham
                  .Where(s => s.MaSP == ID)
                  .Select(s => s.TenSP)
                  .FirstOrDefault();
            return View(cus);
        }


        //giảm giá sản phẩm
        public ActionResult SP_giamgia(int? id)
        {
            // Lấy thông tin sản phẩm từ bảng SanPham
            //var sanPham = db.SanPham.FirstOrDefault(s => s.MaSP == maSanPham);
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham != null)
            {
                // Kiểm tra xem sản phẩm có trong bảng Voucher không
                Vourcher voucher = db.Vourcher.Find(id);

                if (voucher != null)
                {
                    // Lấy thông tin giảm giá từ voucher
                    int giamGia = (int)(sanPham.GiaSp * voucher.Uudai /100);
                    // Tính giá sản phẩm với giảm giá
                    int giaSauGiamGia = (int)(sanPham.GiaSp - giamGia);
                    // Truyền giá sản phẩm đã giảm giá vào view để hiển thị
                    ViewBag.GiaSauGiamGia = giaSauGiamGia;
                }

                return View(sanPham);
            }
            // Xử lý khi sản phẩm không tồn tại
            return RedirectToAction("Index");
        }


        // GET: SanPhams
        public ActionResult Index()
        {
            var sanPham = db.SanPham.Include(s => s.Mau).Include(s => s.PhanLoai);
            return View(sanPham.ToList());
        }

        // GET: SanPhams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: SanPhams/Create
        public ActionResult Create()
        {
            ViewBag.MaMau = new SelectList(db.Mau, "Mamau", "Tenmau");
            ViewBag.MaLoai = new SelectList(db.PhanLoai, "MaLoai", "Tenloai");
            return View();
        }

        // POST: SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSP,TenSP,GiaSp,GiaGiam,Hinh1,Hinh2,Hinh3,Hinh4,Mota,Thongso,MaLoai,MaMau")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.SanPham.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaMau = new SelectList(db.Mau, "Mamau", "Tenmau", sanPham.MaMau);
            ViewBag.MaLoai = new SelectList(db.PhanLoai, "MaLoai", "Tenloai", sanPham.MaLoai);
            return View(sanPham);
        }

        // GET: SanPhams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaMau = new SelectList(db.Mau, "Mamau", "Tenmau", sanPham.MaMau);
            ViewBag.MaLoai = new SelectList(db.PhanLoai, "MaLoai", "Tenloai", sanPham.MaLoai);
            return View(sanPham);
        }

        // POST: SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSP,TenSP,GiaSp,GiaGiam,Hinh1,Hinh2,Hinh3,Hinh4,Mota,Thongso,MaLoai,MaMau")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaMau = new SelectList(db.Mau, "Mamau", "Tenmau", sanPham.MaMau);
            ViewBag.MaLoai = new SelectList(db.PhanLoai, "MaLoai", "Tenloai", sanPham.MaLoai);
            return View(sanPham);
        }

        // GET: SanPhams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPham.Find(id);
            db.SanPham.Remove(sanPham);
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
