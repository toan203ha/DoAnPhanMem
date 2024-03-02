using Doanphanmem.Models;
using System.Linq;

namespace Doanphanmem.Pattern
{
    public class Prototype
    {
        public interface IPrototype
        {
            IPrototype Clone();
        }
        public class TaoBanSao : IPrototype
        {
            private SanPham originalProduct;

            public TaoBanSao(SanPham product)
            {
                this.originalProduct = product;
            }
            public IPrototype Clone()
            {
                return new TaoBanSao(this.originalProduct);
            }
        }
    }
}
