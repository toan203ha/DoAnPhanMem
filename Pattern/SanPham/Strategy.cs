using Doanphanmem.Models;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Doanphanmem.Pattern
{
    public class Strategy
    {
        public interface IThanhToanStrategy
        {
            void ThanhToan(HttpContextBase httpContext, List<MatHangMua> gioHang);
        }

        // Bước 2: Triển khai các chiến lược cụ thể
        public class ThanhToanTrucTiepStrategy : IThanhToanStrategy
        {
            QL_CHDTEntities db = new QL_CHDTEntities();

            private readonly Func<double> _tinhTongTien;

            public ThanhToanTrucTiepStrategy(Func<double> tinhTongTien)
            {
                _tinhTongTien = tinhTongTien;
            }
            public void ThanhToan(HttpContextBase httpContext, List<MatHangMua> gioHang)
            {
                // Lấy thông tin khách hàng từ Session
                KhachHang kh = httpContext.Session["taikhoan"] as KhachHang;

                // Kiểm tra nếu khách hàng không tồn tại hoặc giỏ hàng trống, thoát khỏi hàm
                if (kh == null || gioHang == null || gioHang.Count == 0)
                {
                    return;
                }

                // Thêm dữ liệu vào đơn hàng
                DONDATHANG donhang = new DONDATHANG
                {
                    MaKH = kh.MaKH,
                    NgayDH = DateTime.Now,
                    Trigia = (decimal)_tinhTongTien(),
                    Dagiao = false,
                    Tennguoinhan = kh.TenKH,
                    Diachinhan = kh.DiaChi,
                    Dienthoainhan = kh.sdt.ToString(),
                    HTThanhtoan = true,
                    HTGiaohang = false,
                    GiaoHang = false
                };

                // Lưu đơn hàng vào cơ sở dữ liệu
                db.DONDATHANG.Add(donhang);

                // Thêm vào chi tiết đơn hàng
                foreach (var sanpham in gioHang)
                {
                    CTDATHANG ct = new CTDATHANG
                    {
                        SODH = donhang.SODH,
                        MaSP = sanpham.MaDT,
                        Soluong = sanpham.Soluong,
                        Dongia = (decimal)_tinhTongTien()
                    };
                    db.CTDATHANG.Add(ct);
                }
                db.SaveChanges();
                // Xóa giỏ hàng
                httpContext.Session["GioHang"] = null;

            }
        }

        public class ThanhToanPaypalStrategy : IThanhToanStrategy
        {
            QL_CHDTEntities db = new QL_CHDTEntities();
            private readonly Func<double> _tinhTongTienvnd;
            private readonly Func<double> _tinhTongTien;

            public ThanhToanPaypalStrategy(Func<double> tinhTongTien, Func<double> tinhTongTienvnd)
            {
                _tinhTongTien = tinhTongTien;
                _tinhTongTienvnd = tinhTongTienvnd;
            }
            public void ThanhToan(HttpContextBase httpContext, List<MatHangMua> gioHang)
            {
                KhachHang kh = httpContext.Session["taikhoan"] as KhachHang;
                if (kh == null || gioHang == null || gioHang.Count == 0)
                {
                    return;
                }
                // Các bước xử lý thanh toán PayPal
                var apiContext = PaypalConfiguration.GetAPIContext();
                string paymentId = null;
                string payerId = httpContext.Request.Params["PayerID"];

                // Xử lý khi không có thông tin PayerID
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = httpContext.Request.Url.Scheme + "://" + httpContext.Request.Url.Authority + "/GioHang/PaymentWithPayPal?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid);
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;

                    // Lấy đường dẫn redirect từ PayPal
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = lnk.href;
                            break;
                        }
                    }

                    // Lưu paymentID vào session
                    httpContext.Session.Add(guid, createdPayment.id);
                    // Redirect đến PayPal
                    httpContext.Response.Redirect(paypalRedirectUrl, true);
                }
                else
                {
                    // Xử lý thanh toán khi có thông tin PayerID
                    var guid = httpContext.Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, httpContext.Session[guid] as string);
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return;
                    }
                    else
                    {

                    }
                }
            }

            private Payment CreatePayment(APIContext apiContext, string redirectUrl)
            {
                var itemList = new ItemList
                {
                    items = new List<Item>
                {
                    new Item
                    {
                        name = "Hóa Đơn Thanh Toán Từ Cửa Hàng",
                        currency = "USD",
                        price = _tinhTongTienvnd().ToString(),
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
                    subtotal = _tinhTongTienvnd().ToString()
                };
                var amount = new Amount
                {
                    currency = "USD",
                    total = _tinhTongTienvnd().ToString(), // Tổng giá trị thanh toán trong USD
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
        }

        // Bước 3: Thiết lập cơ chế lựa chọn chiến lược
        public class ThanhToanContext
        {
            private IThanhToanStrategy _thanhToanStrategy;

            public ThanhToanContext(IThanhToanStrategy thanhToanStrategy)
            {
                _thanhToanStrategy = thanhToanStrategy;
            }
            public void SetThanhToanStrategy(IThanhToanStrategy thanhToanStrategy)
            {
                _thanhToanStrategy = thanhToanStrategy;
            }

            public void ThucHienThanhToan(HttpContextBase httpContext, List<MatHangMua> gioHang)
            {
                _thanhToanStrategy.ThanhToan(httpContext, gioHang);
            }
        }
    }
}