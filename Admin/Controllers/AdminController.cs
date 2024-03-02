using Doanphanmem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CNPM_NC_DoAnNhanh.Controllers
{
    public class AdminController : Controller
    {
        private QL_CHDTEntities db = new QL_CHDTEntities();
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index_()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginAccount(KhachHang _user)
        {
            var check = db.KhachHang.Where(s => s.TK == _user.TK && s.Pass == _user.Pass).FirstOrDefault();
            if (check == null)
            {
                ViewBag.ErrorInfo = "Sai Info";
                return View("Index");
            }
            else
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                Session["TK"] = _user.TK;
                Session["Pass"] = _user.Pass;
                Session["RoleUser"] = _user.Roleuser;
                if (check.Roleuser.ToString() == "Admin")
                    return RedirectToAction("Index", "Admin");
                else if (check.Roleuser.ToString() == "Customer")
                    return RedirectToAction("Index", "SanPhams");
                else
                    return RedirectToAction("Login", "Account");
            }
        }
    }
}