using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Doanphanmem.Models
{
    public class ProductViewModel
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public decimal GiaSp { get; set; }
        public decimal? GiaGiam { get; set; }
        public string Hinh1 { get; set; }
        public string Mota { get; set; }
    }
}