using Doanphanmem.Models;
using System;
using System.Collections.Generic;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Doanphanmem.Controllers
{
    public class AccountController : Controller
    {
        private QL_CHDTEntities db = new QL_CHDTEntities(); // Thay thế YourDbContext bằng context của bạn

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserModel model)
        {
            var user = db.KhachHangs.FirstOrDefault(u => u.TK == model.TK && u.Pass == model.Pass);

            if (user != null)
            {
                ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                Session["taikhoan"] = user;
                // Đăng nhập thành công, lưu thông tin người dùng vào Session
                Session["UserID"] = user.MaKH;
                Session["UserName"] = user.TenKH;
                Session["UserRole"] = user.Roleuser;
                return RedirectToAction("Index", "SanPhams"); 
            }

            // Đăng nhập thất bại, hiển thị thông báo lỗi
            ViewBag.ErrorInfo = "Sai thông tin đăng nhập";
            return View(model);
        }

        public ActionResult Logout() 
        {
             
            Session.Remove("UserID");
            Session.Remove("UserName");
            Session.Remove("UserRole");
            Session.Remove("taikhoan");
            return RedirectToAction("Index", "SanPhams");
        }
       
    }

}