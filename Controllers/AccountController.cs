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
                // Kiểm tra vai trò của người dùng
                if (user.Roleuser == "Admin")
                {
                    // Nếu là nhân viên, thiết lập tên mặc định
                    Session["DisplayName"] = "Admin";
                }
                else
                {
                    // Nếu không phải nhân viên, để người dùng điền tên
                    Session["DisplayName"] = "";
                }
                if (Session["UserRole"] == null)
                    return RedirectToAction("Index", "SanPhams");
                //if (Session["UserRole"] == "Admin")
                else if (user.Roleuser.ToString() == "Admin")
                            return RedirectToAction("./index", "Admin");

                else
                    return RedirectToAction("Login", "Account");
            }
            ViewBag.ErrorInfo = "Sai thông tin đăng nhập";
            return View(model);
        }
        // Đăng ký
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(KhachHang model)
        {
            if (ModelState.IsValid)
            {
                // Thực hiện kiểm tra và lưu thông tin đăng ký vào cơ sở dữ liệu
                var existingUser = db.KhachHangs.FirstOrDefault(u => u.TK == model.TK);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Tài khoản đã tồn tại."); // Thông báo lỗi
                }
                else
                {
                    // Lưu đối tượng KhachHang vào cơ sở dữ liệu
                    db.KhachHangs.Add(model);
                    db.SaveChanges();

                    ViewBag.ThongBao = "Chúc mừng, bạn đã đăng ký thành công!";
                    return View("Login");
                }
            }

            // Đăng ký thất bại, hiển thị thông báo lỗi
            return View(model);
        }


        public ActionResult Chat()
        {
            return View();
        }


        public ActionResult Logout() 
        {
             
            Session.Remove("UserID");
            Session.Remove("UserName");
            Session.Remove("UserRole");
            Session.Remove("taikhoan");
            return RedirectToAction("Index", "SanPhams");
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
 
    }

}