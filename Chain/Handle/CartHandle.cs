using Doanphanmem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Doanphanmem.Chain.Handle
{
    public abstract class CartHandle
    {
        protected CartHandle successor;

        public void SetSuccessor(CartHandle successor)
        {
            this.successor = successor;
        }

        public abstract ActionResult HandleRequest(List<MatHangMua> gioHang, int MaSP);
    }
}