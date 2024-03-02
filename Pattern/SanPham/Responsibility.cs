using Doanphanmem.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Serilog;

namespace Doanphanmem.Pattern
{
    public class Responsibility
    {
        private static readonly string LogFilePath = "C:\\Logs\\logfile.txt";
        public interface IValidationHandler
        {
            IValidationHandler NextHandler { get; set; }
            bool Validate(SanPham sanPham, out string errorMessage);
        }
        // kiểm tra các trường dữ liệu bắt buộc
        public class RequiredFieldsValidation : IValidationHandler
        {
            public IValidationHandler NextHandler { get; set; }

            public bool Validate(SanPham sanPham, out string errorMessage)
            {
                errorMessage = null;
                // Kiểm tra các trường bắt buộc
                if (string.IsNullOrEmpty(sanPham.TenSP) || sanPham.GiaSp <= 0 || string.IsNullOrEmpty(sanPham.Mota) || sanPham.GiaSp == null ||
                    sanPham.Soluongton <= 0 || sanPham.Soluongton == null || string.IsNullOrEmpty(sanPham.Thongso))
                {
                    errorMessage = "Vui lòng điền đầy đủ thông tin.";
                    return false;
                }
                return NextHandler?.Validate(sanPham, out errorMessage) ?? true;
            }
        }
        // kiểm tra độ dài kí tự
        public class FormatValidation : IValidationHandler
        {
            public IValidationHandler NextHandler { get; set; }
            public bool Validate(SanPham sanPham, out string errorMessage)
            {
                errorMessage = null;
                // Kiểm tra độ dài của TenSP
                if (sanPham.TenSP.Length >= 5 && sanPham.TenSP.Length <= 30)
                {
                    // Kiểm tra độ dài của Mota và Thongso
                    if (sanPham.Mota.Length <= 300 && sanPham.Thongso.Length <= 300)
                    {
                        return true;
                    }
                    else
                    {
                        errorMessage = "Mô tả không được vượt quá 300 ký tự và thông số không được vượt quá 300 ký tự.";
                        return false;
                    }
                }
                else
                {
                    errorMessage = "Tên sản phẩm phải có độ dài từ 5 đến 30 ký tự.";
                    return false;
                }
            }
        }
        // Kiểm tra sản phẩm trùng
        public class ExistValidation : IValidationHandler
        {
            private readonly QL_CHDTEntities _dbContext;

            public ExistValidation(QL_CHDTEntities dbContext)
            {
                _dbContext = dbContext;
            }

            public IValidationHandler NextHandler { get; set; }

            public bool Validate(SanPham sanPham, out string errorMessage)
            {
                errorMessage = null;

                var tenSanPhamNormalized = sanPham.TenSP;
                var existingProduct = _dbContext.SanPham.FirstOrDefault(s => s.TenSP == sanPham.TenSP);
                if (existingProduct != null)
                {
                    errorMessage = "Sản phẩm đã tồn tại trong hệ thống.";
                    return false;
                }
                return NextHandler?.Validate(sanPham, out errorMessage) ?? true;
            }
        }

    }
}