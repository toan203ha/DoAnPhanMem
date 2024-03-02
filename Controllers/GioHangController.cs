using Doanphanmem.Models;
using Microsoft.AspNet.SignalR;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using static Doanphanmem.Pattern.Strategy;

namespace Doanphanmem.Controllers
{
    //chain
    //public interface IThanhToanHandler
    //{
    //    void XuLyThanhToan(HttpContextBase httpContext, List<MatHangMua> gioHang);
    //}

    //public class ThanhToanBangPayPalHandler : IThanhToanHandler
    //{
    //    QL_CHDTEntities db = new QL_CHDTEntities();
    //    private readonly Func<double> _tinhTongTienvnd;
    //    private readonly Func<double> _tinhTongTien;

    //    public ThanhToanBangPayPalHandler(Func<double> tinhTongTien, Func<double> tinhTongTienvnd)
    //    {
    //        _tinhTongTien = tinhTongTien;
    //        _tinhTongTienvnd = tinhTongTienvnd;
    //    }

    //    public void XuLyThanhToan(HttpContextBase httpContext, List<MatHangMua> gioHang)
    //    {
    //        KhachHang kh = httpContext.Session["taikhoan"] as KhachHang;
    //        if (kh == null || gioHang == null || gioHang.Count == 0)
    //        {
    //            return;
    //        }
    //        // Các bước xử lý thanh toán PayPal
    //        var apiContext = PaypalConfiguration.GetAPIContext();
    //        string paymentId = null;
    //        string payerId = httpContext.Request.Params["PayerID"];

    //        // Xử lý khi không có thông tin PayerID
    //        if (string.IsNullOrEmpty(payerId))
    //        {
    //            string baseURI = httpContext.Request.Url.Scheme + "://" + httpContext.Request.Url.Authority + "/GioHang/PaymentWithPayPal?";
    //            var guid = Convert.ToString((new Random()).Next(100000));
    //            var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid);
    //            var links = createdPayment.links.GetEnumerator();
    //            string paypalRedirectUrl = null;

    //            // Lấy đường dẫn redirect từ PayPal
    //            while (links.MoveNext())
    //            {
    //                Links lnk = links.Current;
    //                if (lnk.rel.ToLower().Trim().Equals("approval_url"))
    //                {
    //                    paypalRedirectUrl = lnk.href;
    //                    break;
    //                }
    //            }

    //            // Lưu paymentID vào session
    //            httpContext.Session.Add(guid, createdPayment.id);
    //            // Redirect đến PayPal
    //            httpContext.Response.Redirect(paypalRedirectUrl, true);
    //        }
    //        else
    //        {
    //            // Xử lý thanh toán khi có thông tin PayerID
    //            var guid = httpContext.Request.Params["guid"];
    //            var executedPayment = ExecutePayment(apiContext, payerId, httpContext.Session[guid] as string);
    //            if (executedPayment.state.ToLower() != "approved")
    //            {
    //                return;
    //            }
    //            else
    //            {

    //            }
    //        }

    //        // Các bước xử lý đơn hàng và giỏ hàng
    //        // Thêm dữ liệu vào đơn hàng

    //    }

    //    private Payment CreatePayment(APIContext apiContext, string redirectUrl)
    //    {
    //        var itemList = new ItemList
    //        {
    //            items = new List<Item>
    //            {
    //                new Item
    //                {
    //                    name = "Item Name",
    //                    currency = "USD",
    //                    price = _tinhTongTienvnd().ToString(),
    //                    quantity = "1",
    //                    sku = "sku"
    //                }
    //            }
    //        };

    //        var payer = new Payer
    //        {
    //            payment_method = "paypal"
    //        };

    //        var redirUrls = new RedirectUrls
    //        {
    //            cancel_url = redirectUrl + "&Cancel=true",
    //            return_url = redirectUrl
    //        };

    //        var details = new Details
    //        {
    //            tax = "0",
    //            shipping = "0",
    //            subtotal = _tinhTongTienvnd().ToString()
    //        };

    //        var amount = new Amount
    //        {
    //            currency = "USD",
    //            total = _tinhTongTienvnd().ToString(), // Tổng giá trị thanh toán trong USD
    //            details = details
    //        };

    //        var transactionList = new List<Transaction>
    //        {
    //            new Transaction
    //            {
    //                description = "Invoice",
    //                invoice_number = Guid.NewGuid().ToString(),
    //                amount = amount,
    //                item_list = itemList
    //            }
    //        };

    //        var payment = new Payment
    //        {
    //            intent = "sale",
    //            payer = payer,
    //            transactions = transactionList,
    //            redirect_urls = redirUrls
    //        };
    //        return payment.Create(apiContext);
    //    }

    //    private PayPal.Api.Payment payment;

    //    private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
    //    {
    //        var paymentExecution = new PaymentExecution()
    //        {
    //            payer_id = payerId
    //        };
    //        this.payment = new Payment()
    //        {
    //            id = paymentId
    //        };
    //        return this.payment.Execute(apiContext, paymentExecution);
    //    }

    //}


    //public class ThanhToanTrucTiepHandler : IThanhToanHandler
    //{
    //    QL_CHDTEntities db = new QL_CHDTEntities();

    //    private readonly Func<double> _tinhTongTien;

    //    public ThanhToanTrucTiepHandler(Func<double> tinhTongTien)
    //    {
    //        _tinhTongTien = tinhTongTien;
    //    }
    //    public void XuLyThanhToan(HttpContextBase httpContext, List<MatHangMua> gioHang)
    //    {
    //        // Lấy thông tin khách hàng từ Session
    //        KhachHang kh = httpContext.Session["taikhoan"] as KhachHang;

    //        // Kiểm tra nếu khách hàng không tồn tại hoặc giỏ hàng trống, thoát khỏi hàm
    //        if (kh == null || gioHang == null || gioHang.Count == 0)
    //        {
    //            return;
    //        }

    //        // Thêm dữ liệu vào đơn hàng
    //        DONDATHANG donhang = new DONDATHANG
    //        {
    //            MaKH = kh.MaKH,
    //            NgayDH = DateTime.Now,
    //            Trigia = (decimal)_tinhTongTien(),
    //            Dagiao = false,
    //            Tennguoinhan = kh.TenKH,
    //            Diachinhan = kh.DiaChi,
    //            Dienthoainhan = kh.sdt.ToString(),
    //            HTThanhtoan = true,
    //            HTGiaohang = false,
    //            GiaoHang = false
    //        };

    //        // Lưu đơn hàng vào cơ sở dữ liệu
    //        db.DONDATHANG.Add(donhang);

    //        // Thêm vào chi tiết đơn hàng
    //        foreach (var sanpham in gioHang)
    //        {
    //            CTDATHANG ct = new CTDATHANG
    //            {
    //                SODH = donhang.SODH,
    //                MaSP = sanpham.MaDT,
    //                Soluong = sanpham.Soluong,
    //                Dongia = (decimal)_tinhTongTien()
    //            };
    //            db.CTDATHANG.Add(ct);
    //        }
    //        db.SaveChanges();
    //        // Xóa giỏ hàng
    //        httpContext.Session["GioHang"] = null;

    //    }
    //}

    //public class ChuoiXuLyThanhToan
    //{
    //    private readonly List<IThanhToanHandler> _handlers = new List<IThanhToanHandler>();

    //    public void ThemHandler(IThanhToanHandler handler)
    //    {
    //        _handlers.Add(handler);
    //    }
    //    public void XuLy(HttpContextBase httpContext, List<MatHangMua> gioHang)
    //    {
    //        foreach (var handler in _handlers)
    //        {
    //            handler.XuLyThanhToan(httpContext, gioHang);
    //            // Kiểm tra xem yêu cầu đã được xử lý chưa, nếu có thể thoát ra khỏi vòng lặp
    //            if (ThanhToanDaXuLy(httpContext))
    //            {
    //                break;
    //            }
    //        }
    //    }

    //    private bool ThanhToanDaXuLy(HttpContextBase httpContext)
    //    {
    //        // Kiểm tra điều kiện nào đó để xác định liệu thanh toán đã được xử lý hay không
    //        // Ví dụ: kiểm tra xem đơn hàng đã được đặt hàng thành công hay không
    //        return false;
    //    }
    //}
    public class GioHangController : Controller
    {
        //private readonly ChuoiXuLyThanhToan _chuoiXuLyThanhToan = new ChuoiXuLyThanhToan();

        private ThanhToanContext _thanhToanContext;

        // GET: GioHang
        QL_CHDTEntities db = new QL_CHDTEntities();
        public GioHangController()
        {
            // chain
            Func<double> tinhTongTien = TinhTongTien;
            Func<double> tinhTongTienvnd = TinhTongTienvnd;
            //// Thêm các handler vào chuỗi xử lý thanh toán
            //_chuoiXuLyThanhToan.ThemHandler(new ThanhToanBangPayPalHandler(tinhTongTien, tinhTongTienvnd));
            //_chuoiXuLyThanhToan.ThemHandler(new ThanhToanTrucTiepHandler(tinhTongTien));
            // Strategy
            _thanhToanContext = new ThanhToanContext(new ThanhToanTrucTiepStrategy(TinhTongTien));
        }
        public ActionResult DongYDatHang(bool isDirectPayment)
        {
            // Lựa chọn chiến lược thanh toán dựa vào đầu vào
            if (isDirectPayment)
            {
                _thanhToanContext.SetThanhToanStrategy(new ThanhToanTrucTiepStrategy(() => TinhTongTien()));
            }
            else
            {
                _thanhToanContext.SetThanhToanStrategy(new ThanhToanPaypalStrategy(() => TinhTongTien(), () => TinhTongTienvnd()));
            }

            // Thực hiện thanh toán với chiến lược đã chọn
            _thanhToanContext.ThucHienThanhToan(HttpContext, Index());
            return View("HoanThanhDonHang");
        }
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
            }
            return gioHang;
        }
        public ActionResult HienThiGioHang()
        {
            List<MatHangMua> gioHang = Index();
            if (gioHang == null || gioHang.Count == 0)
            {
                Session["totalCart"] = 0;
                return RedirectToAction("Index", "SanPhams");
            }
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            ViewBag.TongTiensp = TinhTongTiensp();
            Session["totalCart"] = gioHang.Count;
            return View(gioHang);
        }
        private double TinhTongTiensp()
        {
            double TongTien = 0;
            List<MatHangMua> gioHang = Index();
            if (gioHang != null)
            {
                TongTien = (double)gioHang.Sum(sp => sp.Total());
            }
            return TongTien * 10;
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
            Vourcher exit = db.Vourcher.FirstOrDefault(v => v.MaSP == MaSP);
            SanPham sp = db.SanPham.FirstOrDefault(s => s.MaSP == MaSP);
            if (sp.Soluongton > 0)
            {
                if (sanpham == null)
                {
                    sanpham = new MatHangMua(MaSP);
                    if (exit != null)
                    {
                        int giamGia = (int)(sanpham.Dongia * exit.Uudai / 100);
                        int giaSauGiamGia = (int)(sanpham.Dongia - giamGia);
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
                TongTien = (double)gioHang.Sum(sp => sp.Total());
            }
            return TongTien;
        }
        private double TinhTongTienvnd()
        {
            double TongTien = 0;
            decimal b = 23000;
            List<MatHangMua> gioHang = Index();
            if (gioHang != null)
            {
                TongTien = (double)gioHang.Sum(sp => sp.Total());
            }
            decimal c = (decimal)TongTien / b;
            // Round the result to the nearest integer by casting to int
            int roundedResult = (int)Math.Round((double)c);
            return roundedResult;
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
            
            foreach (var sanpham in giohang)
            {
                var productInDB = db.SanPham.FirstOrDefault(p => p.MaSP == sanpham.MaDT);
                if (productInDB != null)
                {
                    productInDB.Soluongton = productInDB.Soluongton - sanpham.Soluong;
                }
            }
            db.SaveChanges();
            return View(giohang);
        }
        //public ActionResult DongYDatHang(bool isDirectPayment)
        //{
        //    List<MatHangMua> gioHang = Index();

        //    // Tạo instance của ChuoiXuLyThanhToan ở đây, không cần tạo mỗi khi hành động được gọi
        //    // ChuoiXuLyThanhToan có thể được khởi tạo ở constructor hoặc một cách khác phù hợp với ứng dụng của bạn
        //    ChuoiXuLyThanhToan chuoiXuLyThanhToan = new ChuoiXuLyThanhToan();

        //    IThanhToanHandler handler;
        //    if (isDirectPayment)
        //    {
        //        handler = new ThanhToanTrucTiepHandler(TinhTongTien);
        //    }
        //    else
        //    {
        //        handler = new ThanhToanBangPayPalHandler(TinhTongTien, TinhTongTienvnd);
        //    }
        //    chuoiXuLyThanhToan.ThemHandler(handler);
        //    chuoiXuLyThanhToan.XuLy(HttpContext, gioHang);

        //    return View("HoanThanhDonHang");
        //}
        // xử lý đặt hàng
        public ActionResult HoanThanhDonHang()
        {
            return View();
        }
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/GioHang/PaymentWithPayPal?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
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
            return View("HoanThanhDonHang");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var itemList = new ItemList
            {
                items = new List<Item>
                {
                    new Item
                    {
                        name = "Item Name",
                        currency = "USD",
                        price = TinhTongTienvnd().ToString(),
                        quantity = "1",
                        sku = "sku"
                    }
                }
            };

            var payer = new Payer
            {
                payment_method = "paypal"
            };

            var redirUrls = new RedirectUrls
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };

            var details = new Details
            {
                tax = "0",
                shipping = "0",
                subtotal = TinhTongTienvnd().ToString()
            };

            var amount = new Amount
            {
                currency = "USD",
                total = TinhTongTienvnd().ToString(), // Tổng giá trị thanh toán trong USD
                details = details
            };

            var transactionList = new List<Transaction>
            {
                new Transaction
                {
                    description = "Invoice",
                    invoice_number = Guid.NewGuid().ToString(),
                    amount = amount,
                    item_list = itemList
                }
            };

            var payment = new Payment
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            return payment.Create(apiContext);
        }

    }
}
