using System.Linq;
using Doanphanmem.Models;

namespace Doanphanmem.Pattern
{
    public interface IProductQueryBuilder
    {
        IProductQueryBuilder FilterByName(string name);
        IProductQueryBuilder FilterByPrice(int? minPrice, int? maxPrice);
        IProductQueryBuilder FilterByColor(int? colorId);
        IProductQueryBuilder FilterByCategory(int? categoryId);
        IQueryable<SanPham> Build();
    }

    public class Builder : IProductQueryBuilder
    {
        private IQueryable<SanPham> _query;

        public Builder(IQueryable<SanPham> query)
        {
            _query = query;
        }

        public IProductQueryBuilder FilterByName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                _query = _query.Where(p => p.TenSP.Contains(name));
            }
            return this;
        }

        public IProductQueryBuilder FilterByPrice(int? minPrice, int? maxPrice)
        {
            if (minPrice.HasValue)
            {
                _query = _query.Where(p => p.GiaSp >= minPrice);
            }
            if (maxPrice.HasValue)
            {
                _query = _query.Where(p => p.GiaSp <= maxPrice);
            }

            return this;
        }

        public IProductQueryBuilder FilterByColor(int? colorId)
        {
            if (colorId.HasValue)
            {
                _query = _query.Where(p => p.Mamau == colorId);
            }
            return this;
        }

        public IProductQueryBuilder FilterByCategory(int? categoryId)
        {
            if (categoryId.HasValue)
            {
                _query = _query.Where(p => p.MaLoai == categoryId);
            }
            return this;
        }

        public IQueryable<SanPham> Build()
        {
            return _query;
        }
    }
}
