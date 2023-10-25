using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Doanphanmem.Models
{
    public class MatHangMua
    {
        QL_CHDTEntities db = new QL_CHDTEntities();
        public int MaDT { get; set; }
        public string Ten { get; set; }
        public string AnhBia { get; set; }
        public int Dongia { get; set; }

        public int Mamau { get; set; }
        public int MaLoai { get; set; }
        public decimal GiamGia { get; set; }
        public int Soluong { get; set; }
        public double Total()
        {
            return Soluong * Dongia;
        }

        public MatHangMua(int MaDT)
        {
            this.MaDT= MaDT;
            var getSP = db.SanPham.FirstOrDefault(x => x.MaSP == this.MaDT);
            this.Ten = getSP.TenSP;
            this.AnhBia = getSP.Hinh1;
            this.Dongia = int.Parse(getSP.GiaSp.ToString());
            this.Soluong = 1;
        }
        public void UpdateTotal()
        {
            // Cập nhật giá trị Total() cho sản phẩm
            int total = Soluong * Dongia;
            // Cập nhật lại giá trị Total
            // Có thể thêm các logic xử lý khác ở đây nếu cần
        }
    }
}