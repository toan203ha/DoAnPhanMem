using Doanphanmem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Doanphanmem.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        QL_CHDTEntities db = new QL_CHDTEntities();

        public ActionResult GiohangPartial()
        {
            ViewBag.TongSl = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }


        // GET: Cart
        public List<MatHangMua> Index()
        {
            List<MatHangMua> gioHang = Session["GioHang"] as List<MatHangMua>;
            if (gioHang == null || gioHang.Count == 0)
            {
                gioHang = new List<MatHangMua>();
                Session["GioHang"] = gioHang;
                //return View("CartNoProduct");
            }
            //ViewBag.TongSL = TinhTongSL();
            //ViewBag.TongTien = TinhTongTien();
            return gioHang;
        }
        public ActionResult HienThiGioHang()
        {
            List<MatHangMua> gioHang = Index();
            if (gioHang == null || gioHang.Count == 0)
            {
                Session["totalCart"] = 0;
                //return View("CartNoProduct");
                return RedirectToAction("Index", "SanPhams");
            }
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();

            //Session["totalCart"] = (Session["GioHang"] as List<MatHangMua>).Count;

            Session["totalCart"] = gioHang.Count;
            return View(gioHang);
        }


        public ActionResult DeleteProduct(int MaSP)
        {

            List<MatHangMua> gioHang = Index();
            var sanpham = gioHang.FirstOrDefault(s => s.MaDT == MaSP);
            if (sanpham != null)
            {
                gioHang.RemoveAll(s => s.MaDT == MaSP);
                return RedirectToAction("HienThiGioHang");
            }
            if (gioHang.Count == 0)
                return RedirectToAction("Index", "SanPhams");
            return RedirectToAction("HienThiGioHang");
        }
        public ActionResult AddProduct(int MaSP)
        {
            List<MatHangMua> giohang = Index();
            MatHangMua sanpham = giohang.FirstOrDefault(s => s.MaDT == MaSP);
            Vourcher exit = db.Vourcher.Find(MaSP);
            if (sanpham == null)
            {
                sanpham = new MatHangMua(MaSP);
                if (exit != null)
                {
                    // Lấy thông tin giảm giá từ voucher
                    int giamGia = (int)(sanpham.Dongia * exit.Uudai / 100);
                    // Tính giá sản phẩm với giảm giá
                    int giaSauGiamGia = (int)(sanpham.Dongia - giamGia);
                    // Truyền giá sản phẩm đã giảm giá vào view để hiển thị
                    ViewBag.GiaSauGiamGia = giaSauGiamGia;
                    sanpham.Dongia = giaSauGiamGia;
                }             
                    giohang.Add(sanpham);
                
            }
            else
            {
                sanpham.Soluong++;
            }
            return RedirectToAction("Index", "SanPhams", new { id = MaSP });
        }

        public ActionResult UpdateCart(int MaSP, int SoLuong)
        {
            List<MatHangMua> gioHang = Index();
            var sanpham = gioHang.FirstOrDefault(s => s.MaDT == MaSP);
            if (sanpham != null)
            {
                sanpham.Soluong = SoLuong;
           
            }
            return RedirectToAction("HienThiGioHang"); ;
        }

        private double TinhTongTien()
        {
            double TongTien = 0;
            List<MatHangMua> gioHang = Index();
            if (gioHang != null)
            {
                TongTien = gioHang.Sum(sp => sp.Total());
            }
            return TongTien;
        }

        private int TinhTongSL()
        {
            int tongSL = 0;
            List<MatHangMua> gioHang = Index();
            if (gioHang != null)
                tongSL = gioHang.Sum(sp => sp.Soluong);
            return tongSL;
        }
        //dat hang
        public ActionResult DatHang()
        {
            if (Session["taikhoan"] == null)
                return RedirectToAction("Login", "Account");
            List<MatHangMua> giohang = Index();
            if (giohang == null || giohang.Count == 0)
                return RedirectToAction("Index", "SanPhams");
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return View(giohang);
        }
        // xử lý đặt hàng
        public ActionResult DongYDatHang()
        {

            KhachHang kh = Session["taikhoan"] as KhachHang;
            List<MatHangMua> giohang = Index();

            // thêm dữ liệu vào đơn hàng
            DONDATHANG donhang = new DONDATHANG();
            donhang.MaKH = kh.MaKH;
            donhang.NgayDH = DateTime.Now;
            donhang.Trigia = (Decimal)TinhTongTien();
            donhang.Dagiao = false;
            donhang.Tennguoinhan = kh.TenKH;
            donhang.Diachinhan = kh.DiaChi;
            donhang.Dienthoainhan = kh.sdt.ToString();
            donhang.HTThanhtoan = false;
            donhang.HTGiaohang = false;
            db.DONDATHANG.Add(donhang);
            db.SaveChanges();
  

            // thêm vào chi tiết đơn hàng
            foreach (var sanpham in giohang)
            {
                CTDATHANG ct = new CTDATHANG();
                ct.SODH = donhang.SODH;
                ct.MaSP = sanpham.MaDT;
                ct.Soluong = sanpham.Soluong;
                ct.Dongia = (decimal)TinhTongTien();
                db.CTDATHANG.Add(ct);
            }
            db.SaveChanges();
            //xóa giỏ hàng
            Session["GioHang"] = null;
            return RedirectToAction("HoanThanhDonHang");
        }

        public ActionResult HoanThanhDonHang()
        {
            return View();
        }
    }

}