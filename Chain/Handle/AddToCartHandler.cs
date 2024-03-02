using Doanphanmem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Doanphanmem.Chain.Handle
{
    public class AddToCartHandler : PaymentHandler
    {
        private readonly QL_CHDTEntities db;

        public AddToCartHandler(QL_CHDTEntities dbContext)
        {
            db = dbContext;
        }

        public override ActionResult HandleRequest(List<MatHangMua> gioHang, int MaSP)
        {
            MatHangMua sanpham = gioHang.FirstOrDefault(s => s.MaDT == MaSP);
            Vourcher exit = db.Vourcher.FirstOrDefault(v => v.MaSP == MaSP);

            if (sanpham == null)
            {
                sanpham = new MatHangMua(MaSP);
                if (exit != null)
                {
                    int giamGia = (int)(sanpham.Dongia * exit.Uudai / 100);
                    int giaSauGiamGia = (int)(sanpham.Dongia - giamGia);
                    sanpham.Dongia = giaSauGiamGia;
                }
                gioHang.Add(sanpham);
            }
            else
            {
                sanpham.Soluong++;
            }

            // Trả về một RedirectResult với URL cụ thể
            return new RedirectResult($"/SanPhams/Index/{MaSP}");
        }
    }
}
