using Doanphanmem.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Doanphanmem.Chain.Handle
{
    public class PaymentProcessor
    {
        private PaymentHandler successor;
        private UrlHelper urlHelper;

        public PaymentProcessor(UrlHelper urlHelper)
        {
            this.urlHelper = urlHelper;
        }

        public void SetSuccessor(PaymentHandler successor)
        {
            this.successor = successor;
        }

        public ActionResult HandleRequest(List<MatHangMua> gioHang, int MaSP)
        {
            if (successor != null)
            {
                return successor.HandleRequest(gioHang, MaSP);
            }

            // Sử dụng UrlHelper để tạo RedirectResult
            string redirectUrl = urlHelper.Action("Index", "SanPhams", new { id = MaSP });
            return new RedirectResult(redirectUrl);
        }
    }
}
