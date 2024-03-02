using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Doanphanmem.Models;
using Doanphanmem.Pattern;
using Microsoft.Owin.Security.Infrastructure;
using PagedList;
using static Doanphanmem.Pattern.composite;
using static Doanphanmem.Pattern.Prototype;
using static Doanphanmem.Pattern.Responsibility;

namespace Doanphanmem.Controllers
{
    public class SanPhamsController : Controller
    {
        private QL_CHDTEntities db = new QL_CHDTEntities();
        private readonly IValidationHandler _validationHandler;
 
        public SanPhamsController()
        {
            //Chain of responsibility
            var requiredFieldsValidation = new RequiredFieldsValidation();
            var lengthValidation = new FormatValidation();
            var existValidation = new ExistValidation(db);
            existValidation.NextHandler = requiredFieldsValidation; // Thiết lập requiredFieldsValidation là NextHandler của existValidation
            requiredFieldsValidation.NextHandler = lengthValidation;
            lengthValidation.NextHandler = existValidation;
            _validationHandler = existValidation; // Thiết lập existValidation là handler cuối cùng trong chuỗi
        }
        public ActionResult Create()
        {
            ViewBag.MaMau = new SelectList(db.Mau, "Mamau", "Tenmau");
            ViewBag.MaLoai = new SelectList(db.PhanLoai, "MaLoai", "Tenloai");
            return View();
        }
        //mau chain of responsibility ------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSP,TenSP,GiaSp,GiaGiam,Hinh1,Hinh2,Hinh3,Hinh4,Mota,Thongso,Soluongton,MaLoai,MaMau")] SanPham sanPham,
            HttpPostedFileBase Hinh1)
        {
            string errorMessage;
            if (_validationHandler.Validate(sanPham, out errorMessage))
            {
                if (Hinh1 != null)
                {
                    var fileName = Path.GetFileName(Hinh1.FileName);
                    var path = Path.Combine(Server.MapPath("~/Image"), fileName);
                    //Lưu tên
                    sanPham.Hinh1 = fileName;
                    Hinh1.SaveAs(path);
                }
                db.SanPham.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("IndexAdmin");
            }

            ViewBag.MaMau = new SelectList(db.Mau, "Mamau", "Tenmau", sanPham.Mamau);
            ViewBag.MaLoai = new SelectList(db.PhanLoai, "MaLoai", "Tenloai", sanPham.MaLoai);
            ModelState.AddModelError("", errorMessage);
            return View(sanPham);
        }
        //--------------------------------------------------------------------------------------------

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
        public ActionResult HoanThanhDH(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONDATHANG donhang = db.DONDATHANG.Find(id);
            //donhang.Dagiao = true;
             if (donhang.GiaoHang == false)
            {
                donhang.GiaoHang = true;
            }
            db.SaveChanges();
            if (donhang == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }
        public ActionResult getCus(int ID)
        {
            var cus = db.SanPham
                  .Where(s => s.MaSP == ID)
                  .Select(s => s.TenSP)
                  .FirstOrDefault();
            return View(cus);
        }
        public ActionResult getDanhsachLoaiSP(int ID)
        {
            var orders = db.SanPham
                .Where(o => o.MaLoai == ID)
                .ToList();
            return View(orders);
        }
        // giảm giá sản phẩm
        // thông tin chi tiết sản phẩm
        public ActionResult SP_giamgia(int? id)
        {
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham != null)
            {
                Vourcher voucher = db.Vourcher.FirstOrDefault(v => v.MaSP == id);
                if (voucher != null)
                {
                    int giamGia = (int)(voucher.Uudai * 0.01 * sanPham.GiaSp);
                    int giaSauGiamGia = (int)(sanPham.GiaSp - giamGia);
                    ViewBag.GiaSauGiamGia = giaSauGiamGia;
                }

                return View(sanPham);
            }
            return RedirectToAction("Index");
        }
        //mau composite ------------------------------------------------------------------
        //public ActionResult Index(string searchString, int? page)
        //{
        //    var sanPham = db.SanPham.Include(s => s.Mau).Include(s => s.PhanLoai);
        //    var compositeSearch = new CompositeSearch();
        //    compositeSearch.AddComponent(new TenSanPhamSearch());
        //    compositeSearch.AddComponent(new MaSanPhamSearch());
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        sanPham = compositeSearch.ApplySearch(sanPham, searchString);
        //    }
        //    int pageSize = 8;
        //    int pageNumber = (page ?? 1);
        //    ViewBag.MaMau = new SelectList(db.Mau, "Mamau", "Tenmau");
        //    ViewBag.MaLoai = new SelectList(db.PhanLoai, "MaLoai", "Tenloai");
        //    return View(sanPham.OrderBy(donhang => donhang.MaSP).ToPagedList(pageNumber, pageSize));
        //}
        //mau composite ------------------------------------------------------------------
        //mau builder   ------------------------------------------------------------------
        public ActionResult Index(string searchString, int? minPrice, int? maxPrice, int? colorId, int? categoryId, string name, int? page)
        {
            //
            // Khởi tạo builder và truyền IQueryable<SanPham> từ DbContext vào constructor
            var builder = new Builder(db.SanPham);
            // Tiếp tục logic của phương thức Index
            IQueryable<SanPham> query = db.SanPham.Include(s => s.Mau).Include(s => s.PhanLoai);
            // Khai báo biến queryBuilder kiểu IProductQueryBuilder
            query = query.GroupBy(s => new { s.TenSP, s.GiaSp, s.Mamau, s.MaLoai })
                              .Select(group => group.FirstOrDefault());
            IProductQueryBuilder queryBuilder = builder;
            // Apply filters
            if (builder != null)
            {
                queryBuilder = queryBuilder.FilterByName(name);

                if (minPrice.HasValue || maxPrice.HasValue)
                {
                    queryBuilder = queryBuilder.FilterByPrice(minPrice, maxPrice);
                }

                if (colorId.HasValue)
                {
                    queryBuilder = queryBuilder.FilterByColor(colorId);
                }

                if (categoryId.HasValue)
                {
                    queryBuilder = queryBuilder.FilterByCategory(categoryId);
                }
                // Sử dụng phương thức Build để nhận được kết quả kiểu IQueryable<SanPham>
                query = queryBuilder.Build();
            }
            //composite -----------------------------------------------------------------------
            // Apply search
            var compositeSearch = new CompositeSearch();
            compositeSearch.AddComponent(new TenSanPhamSearch());
            compositeSearch.AddComponent(new MaSanPhamSearch());
            if (!String.IsNullOrEmpty(searchString))
            {
                query = compositeSearch.ApplySearch(query, searchString);
            }
            // Pagination
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            ViewBag.MaMau = new SelectList(db.Mau, "Mamau", "Tenmau");
            ViewBag.MaLoai = new SelectList(db.PhanLoai, "MaLoai", "Tenloai");
            return View(query.OrderBy(donhang => donhang.MaSP).ToPagedList(pageNumber, pageSize));
        }
        //mau Prototype  ---------------------------------------------------------------------------
        public ActionResult CloneProduct(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham originalProduct = db.SanPham.Find(id);
            if (originalProduct == null)
            {
                return HttpNotFound();
            }
            Prototype.TaoBanSao prototype = new Prototype.TaoBanSao(originalProduct);
            int maxMaSP = db.SanPham.Max(p => p.MaSP);

            IPrototype clonedPrototype = prototype.Clone();
            SanPham clonedProduct = new SanPham();
            clonedProduct.MaSP = maxMaSP + 1;
            clonedProduct.TenSP = originalProduct.TenSP;
            clonedProduct.GiaSp = originalProduct.GiaSp;
            clonedProduct.GiaGiam = originalProduct.GiaGiam;
            clonedProduct.Hinh1 = originalProduct.Hinh1;
            clonedProduct.Mota = originalProduct.Mota;
            clonedProduct.Thongso = originalProduct.Thongso;
            clonedProduct.Soluongton = originalProduct.Soluongton;
            clonedProduct.MaLoai = originalProduct.MaLoai;
            clonedProduct.Mamau = originalProduct.Mamau;

            db.SanPham.Add(clonedProduct);
            db.SaveChanges();
            return RedirectToAction("Edit", "SanPhams", new { id = clonedProduct.MaSP });
        }
        //--------------------------------------------------------------------------------------------
        public ActionResult DonHangChiTietKH(int? id)
        {
            // Thực hiện truy vấn cơ sở dữ liệu để lấy danh sách chi tiết đơn hàng dựa vào SODH          
            var cTDATHANGs = db.CTDATHANG.Include(c => c.DONDATHANG).Include(c => c.SanPham).Where(c => c.SODH == id);
            return View(cTDATHANGs.ToList());
        }
        // xác nhận đơn hang
        public ActionResult Xacnhan_dh(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONDATHANG donhang = db.DONDATHANG.Find(id);
            if (donhang.GiaoHang == false)
            {
                donhang.GiaoHang = true;
            }
            db.SaveChanges();
            if (donhang == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }
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
        //mau chain of responsibility ----------------------------------------------------------------
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
            ViewBag.MaMau = new SelectList(db.Mau, "Mamau", "Tenmau", sanPham.Mamau);
            ViewBag.MaLoai = new SelectList(db.PhanLoai, "MaLoai", "Tenloai", sanPham.MaLoai);
            return View(sanPham);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSP,TenSP,GiaSp,GiaGiam,Hinh1,Hinh2,Hinh3,Hinh4,Mota,Thongso, Soluongton,MaLoai,MaMau")] SanPham sanPham,
             HttpPostedFileBase Hinh1)
        {
            string errorMessage;
            if (_validationHandler.Validate(sanPham, out errorMessage))
            {
                if (Hinh1 != null)
                {
                    //Lấy tên file của hình được up lên
                    var fileName = Path.GetFileName(Hinh1.FileName);
                    //Tạo đường dẫn tới file
                    var path = Path.Combine(Server.MapPath("~/Image"), fileName);
                    //Lưu tên
                    sanPham.Hinh1 = fileName;
                    //Save vào Images Folder
                    Hinh1.SaveAs(path);
                }
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexAdmin");
            }
            ViewBag.MaMau = new SelectList(db.Mau, "Mamau", "Tenmau", sanPham.Mamau);
            ViewBag.MaLoai = new SelectList(db.PhanLoai, "MaLoai", "Tenloai", sanPham.MaLoai);
            ModelState.AddModelError("", errorMessage);
            return View(sanPham);
        }
        //--------------------------------------------------------------------------------------------

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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPham.Find(id);
            db.SanPham.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("IndexAdmin");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        // top 5 sản phẩm bán chạy nhất 
        public ActionResult laysanphamtt()
        {
            var sanpham = db.SanPham
                .OrderBy(p => p.Soluongton)  
                .Take(5)  
                .ToList();
            return PartialView(sanpham);
        }
        public ActionResult layloaisp()
        {
            var loaisp = db.PhanLoai.ToList();
            return PartialView(loaisp);
        }
        public ActionResult layloaisp_layout()
        {
            var loaisp = db.PhanLoai.ToList();
            return PartialView(loaisp);
        }
        public ActionResult locgia(int gia)
        {
            var sp = db.SanPham.Where(s => s.GiaSp > 0 && s.GiaSp <= gia).ToList();

            if (sp != null)
            {
                return View(sp);
            }
            else
            {
                return View("NoProductsFound");
            }
        }
        public ActionResult IndexAdmin(String SearchString, int? page)
        {
            var sanPham = db.SanPham.Include(s => s.Mau).Include(s => s.PhanLoai);
            if (!String.IsNullOrEmpty(SearchString))
            {
                sanPham = sanPham.Where(s => s.TenSP.Contains(SearchString));
            }
            {
                Console.WriteLine("Không tìm thấy sản phẩm nào");
            }

           
            sanPham = db.SanPham;
            var dsSach = sanPham.ToList();
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(dsSach.OrderBy(donhang => donhang.MaSP).ToPagedList(pageNum, pageSize));
        }
    }
}

