using Doanphanmem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Doanphanmem.Pattern
{
    public class composite
    {
        //composite ----------------------------------------------
        public interface ISearchComposite
        {
            IQueryable<SanPham> ApplySearch(IQueryable<SanPham> query, string searchString);
        }

        //Tìm theo tên sản phẩm
        public class TenSanPhamSearch : ISearchComposite
        {
            public IQueryable<SanPham> ApplySearch(IQueryable<SanPham> query, string searchString)
            {
                return query.Where(s => s.TenSP.Contains(searchString));
            }
        }
        //Tìm theo mã sản phẩm
        public class MaSanPhamSearch : ISearchComposite
        {
            public IQueryable<SanPham> ApplySearch(IQueryable<SanPham> query, string searchString)
            {
                int maSP;
                if (int.TryParse(searchString, out maSP))
                {
                    return query.Where(s => s.MaSP == maSP);
                }
                return query;
            }
        }
        public class CompositeSearch : ISearchComposite
        {
            private List<ISearchComposite> components = new List<ISearchComposite>();

            public void AddComponent(ISearchComposite component)
            {
                components.Add(component);
            }

            public IQueryable<SanPham> ApplySearch(IQueryable<SanPham> query, string searchString)
            {
                foreach (var component in components)
                {
                    query = component.ApplySearch(query, searchString);
                }
                return query;
            }
        }
    }
}