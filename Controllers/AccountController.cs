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
<<<<<<< HEAD
<<<<<<< HEAD
=======

                // Kiểm tra vai trò của người dùng
                if (user.Roleuser == "NhanVien")
                {
                    // Nếu là nhân viên, thiết lập tên mặc định
                    Session["DisplayName"] = "NhanVien";
                }
                else
                {
                    // Nếu không phải nhân viên, để người dùng điền tên
                    Session["DisplayName"] = "";
                }
>>>>>>> Toan_7_11
                return RedirectToAction("Index", "SanPhams"); 
=======
                if (user.Roleuser.ToString() == "Admin")
                    return RedirectToAction("./index", "Admin");
                else if (user.Roleuser.ToString() == "Customer")
                    return RedirectToAction("Index", "SanPhams");
                else
                    return RedirectToAction("Login", "Account");
>>>>>>> Tuan
            }

            // Đăng nhập thất bại, hiển thị thông báo lỗi
            ViewBag.ErrorInfo = "Sai thông tin đăng nhập";
            return View(model);
        }

<<<<<<< HEAD
<<<<<<< HEAD
=======
        public ActionResult Chat()
        {
            return View();
        }




>>>>>>> Toan_7_11
        public ActionResult Logout() 
        {
             
            Session.Remove("UserID");
            Session.Remove("UserName");
            Session.Remove("UserRole");
            Session.Remove("taikhoan");
            return RedirectToAction("Index", "SanPhams");
        }
<<<<<<< HEAD
       
=======
        
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
>>>>>>> Tuan
=======

>>>>>>> Toan_7_11
    }

}