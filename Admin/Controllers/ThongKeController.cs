using Doanphanmem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Doanphanmem.Admin.Controllers
{
    public class ThongKeController : Controller
    {
        // GET: ThongKe
        
        private QL_CHDTEntities db = new QL_CHDTEntities();
        public ActionResult chart()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CharData"].ConnectionString;
            List<string> labels = new List<string>();
            List<decimal> data = new List<decimal>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Các thao tác với cơ sở dữ liệu ở đây
                connection.Open();
                string sql = "SELECT MONTH(NgayDH) AS Thang, SUM(Trigia) AS DoanhThu FROM DONDATHANG GROUP BY MONTH(NgayDH)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            labels.Add(reader["Thang"].ToString());
                            data.Add((decimal)reader["DoanhThu"]);
                        }
                    }
                }
            }
            ViewBag.Labels = labels;
            ViewBag.Data = data;

            return View();
        }
        public ActionResult Index()
        {
            return View();
        }

        public decimal TinhTongTien()
        {
            var dONDATHANGs = db.DONDATHANG.Sum(i=>i.Trigia);
             return dONDATHANGs.Value;
        }
        public int TinhSoLuong()
        {
            var dONDATHANGs = db.DONDATHANG.Count();
             return dONDATHANGs;
        }
        public int TinhSLKH()
        {
            var khachhang = db.DONDATHANG.AsQueryable().Select(dh => dh.MaKH).Distinct().ToList();
             int kh = khachhang.Count;
            return kh;
        }
       
        private Dictionary<string, decimal> FilterDataByDate(Dictionary<string, decimal> data, DateTime fromDate, DateTime toDate)
        {
            // Lọc dữ liệu từ fromDate đến toDate
            var filteredData = data.Where(entry =>
            {
                var parts = entry.Key.Split('-');
                var entryMonth = int.Parse(parts[0]);
                var entryYear = int.Parse(parts[1]);
                var entryDate = new DateTime(entryYear, entryMonth, 1); 

                return entryDate >= fromDate && entryDate <= toDate;
            }).ToDictionary(x => x.Key, x => x.Value);

            return filteredData;
        }

        public ActionResult Chart_sort(DateTime? fromDate, DateTime? toDate)
        {
            var now = DateTime.Now;
            var twelveMonthsData = new Dictionary<string, decimal>();

            for (int i = 0; i < 12; i++)
            {
                var thangNam = $"{now.Month}-{now.Year}";
                var doanhThu = db.DONDATHANG.AsQueryable()
                    .Where(dh => dh.NgayDH.HasValue && dh.NgayDH.Value.Month == now.Month && dh.NgayDH.Value.Year == now.Year)
                    .Sum(dh => dh.Trigia);

                if (doanhThu.HasValue)
                {
                    twelveMonthsData.Add(thangNam, (decimal)doanhThu);
                }
                else
                {
                    twelveMonthsData.Add(thangNam, 0);
                }

                now = now.AddMonths(-1);
            }
            var query = db.DONDATHANG.AsQueryable();

            // Truyền danh sách top 10 khách hàng vào ViewBag hoặc Model

            if (fromDate.HasValue)
            {
                query = query.Where(dh => dh.NgayDH.HasValue && dh.NgayDH.Value >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(dh => dh.NgayDH.HasValue && dh.NgayDH.Value < toDate.Value.AddDays(1));
            }

            // Lấy dữ liệu TopKhachHangModel

            ViewBag.TongTien = TinhTongTien();
            ViewBag.TongDH = TinhSoLuong();
            ViewBag.TongKH = TinhSLKH();
            ViewBag.TwelveMonthsData = twelveMonthsData;

            //tôp 10 khách hàng
            var top10KhachHang = db.DONDATHANG
                .GroupBy(dh => dh.MaKH)
                .Select(g => new
                {
                    MaKH = g.Key,
                    tenkh = g.FirstOrDefault().KhachHang.TenKH,
                    TongTienMua = g.Sum(x => x.Trigia ?? 0),
                    SoLuongDonHang = g.Count()
                })
                .OrderByDescending(kh => kh.TongTienMua)
                .Take(10)
                .ToList();

            var top10KhachHangDetails = new List<TopKhachHangModel>();
            foreach (var khachHang in top10KhachHang.Where(kh => kh.MaKH != null))
            {
                var khachHangDetail = db.KhachHang.Find(khachHang.MaKH);
                if (khachHangDetail != null)
                {
                    var topKhachHangModel = new TopKhachHangModel
                    {
                        MaKH = khachHang.MaKH.Value,
                        tenkh = khachHang.tenkh,
                        TongTienMua = khachHang.TongTienMua,
                        SoLuongDonHang = khachHang.SoLuongDonHang
                    };
                    top10KhachHangDetails.Add(topKhachHangModel);
                }
            }

            ViewBag.Top10KhachHang = top10KhachHangDetails;
            return View();
        }
        public ActionResult Top10KhachHangMuaNhieuNhat()
        {
            var top10KhachHang = db.DONDATHANG
                .GroupBy(dh => dh.MaKH)
                .Select(g => new
                {
                    MaKH = g.Key,
                    TongTienMua = g.Sum(x => x.Trigia ?? 0),  
                    SoLuongDonHang = g.Count()
                })
                .OrderByDescending(kh => kh.TongTienMua)
                .Take(10)
                .ToList();

            var top10KhachHangDetails = new List<TopKhachHangModel>();

            foreach (var khachHang in top10KhachHang.Where(kh => kh.MaKH != null))
            {
                var khachHangDetail = db.KhachHang.Find(khachHang.MaKH);
                if (khachHangDetail != null)
                {
                    var topKhachHangModel = new TopKhachHangModel
                    {
                        MaKH = khachHang.MaKH.Value,
                        TenKhachHang = khachHangDetail.TenKH,
                        TongTienMua = khachHang.TongTienMua,
                        SoLuongDonHang = khachHang.SoLuongDonHang
                    };
                    top10KhachHangDetails.Add(topKhachHangModel);
                }
            }
            return View(top10KhachHangDetails);
        }

        [HttpGet]
        public ActionResult FilterData(DateTime fromDate, DateTime toDate)
        {
            var filteredData = GetFilteredDataFromDatabase(fromDate, toDate);

            return Json(filteredData);
        }
        private Dictionary<string, decimal> GetFilteredDataFromDatabase(DateTime fromDate, DateTime toDate)
        {
            var filteredData = new Dictionary<string, decimal>();
            var result = db.DONDATHANG
                .Where(dh => dh.NgayDH.Value >= fromDate && dh.NgayDH.Value <= toDate)
                .ToList();
            foreach (var item in result)
            {
                var key = $"{item.NgayDH.Value.Month}-{item.NgayDH.Value.Year}";
                if (filteredData.ContainsKey(key))
                {
                    filteredData[key] += (decimal)item.Trigia;
                }
                else
                {
                    filteredData[key] = (decimal)item.Trigia;
                }
            }

            return filteredData;
        }
    }
}