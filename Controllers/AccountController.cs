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


        // Tạo action khác để đảm bảo người dùng đã đăng nhập (điều hướng từ action Login sau khi đăng nhập thành công)
        //public ActionResult Dashboard()
        //{
        //    if (Session["UserID"] == null)
        //    {
        //        // Điều hướng về trang đăng nhập nếu người dùng chưa đăng nhập
        //        return RedirectToAction("Login");
        //    }

        //    // Lấy thông tin người dùng từ Session và hiển thị trong section
        //    var userModel = new UserModel
        //    {
        //        MaKH = (int)Session["UserID"],
        //        TenKH = Session["UserName"].ToString(),
        //        Roleuser = Session["UserRole"].ToString()
        //    };

        //    return View(userModel);
        //}
    }

}