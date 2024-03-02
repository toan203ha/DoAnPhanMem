using Doanphanmem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Doanphanmem.Chain.Handle
{
    public abstract class PaymentHandler
    {
        protected PaymentHandler successor; // Xử lý viên kế tiếp trong chuỗi

        // Thiết lập xử lý viên kế tiếp
        public void SetSuccessor(PaymentHandler successor)
        {
            this.successor = successor;
        }

        // Phương thức xử lý yêu cầu
        public virtual ActionResult HandleRequest(List<MatHangMua> gioHang, int MaSP)
        {
            // Nếu xử lý viên hiện tại có thể xử lý yêu cầu, thực hiện xử lý
            // Nếu không, chuyển yêu cầu cho xử lý viên kế tiếp (nếu có)
            if (successor != null)
            {
                return successor.HandleRequest(gioHang, MaSP);
            }
            // Nếu không có xử lý viên nào có thể xử lý yêu cầu, trả về một giá trị mặc định (có thể là null hoặc một hành động khác)
            return null;
        }

        public static explicit operator Controller(PaymentHandler v)
        {
            throw new NotImplementedException();
        }
    }

}