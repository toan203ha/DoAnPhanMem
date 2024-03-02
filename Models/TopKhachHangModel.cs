using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Doanphanmem.Models
{
    public class TopKhachHangModel
    {
        public int MaKH { get; set; }
        public string tenkh { get; set; }
        public int SoLuongDonHang { get; set; }
        public decimal  TongTienMua { get; set; }
        public string TenKhachHang { get; set; }
    }
}