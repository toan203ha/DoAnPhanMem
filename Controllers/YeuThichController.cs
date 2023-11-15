using Antlr.Runtime.Tree;
using Doanphanmem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Doanphanmem.Controllers
{
    public class YeuThichController : Controller
    {
      
            private QL_CHDTEntities db;

            public YeuThichController()
            {
                db = new QL_CHDTEntities();
            }

        public ActionResult YeuThichPartial()
        {
            
            return PartialView();
        }

        public ActionResult AddToFavorite(int productId)
            {
            // Lấy thông tin khách hàng đang đăng nhập từ Session hoặc cách xác thực khác.
            int customerId = (int)Session["UserID"];
            var existingFavorite = db.YeuThiches.FirstOrDefault(y => y.MaSP == productId && y.MaKH == customerId);

                if (existingFavorite != null)
                {
                    // Nếu sản phẩm đã tồn tại trong danh sách yêu thích, bạn có thể xử lý lệnh xóa sản phẩm ra khỏi danh sách yêu thích ở đây.
                    db.YeuThiches.Remove(existingFavorite);
                    //return RedirectToAction("Index","SanPhams");
                }

                else
                {
                    // Nếu sản phẩm chưa tồn tại trong danh sách yêu thích, thêm sản phẩm mới vào danh sách 
                    var newFavorite = new YeuThich
                    {
                        MaSP = productId,
                        MaKH = customerId
                    };
                    db.YeuThiches.Add(newFavorite);
                }
 
                db.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu.

                return RedirectToAction("FavoriteList"); 
            }
            
            public ActionResult FavoriteList()
            {
            // Lấy danh sách sản phẩm yêu thích của khách hàng từ cơ sở dữ liệu
            int customerId = (int)Session["UserID"];           
                var favoriteProducts = db.YeuThiches
                    .Where(y => y.MaKH == customerId)
                    .Select(y => y.SanPham)
                    .ToList();
                if (favoriteProducts == null || favoriteProducts.Count==0)
                {
                    return RedirectToAction("EmptyFavo","YeuThich");
                }
            return View(favoriteProducts);
            }

   
        public ActionResult EmptyFavo()
            {
             return View();
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YeuThich yeuThich = db.YeuThiches.Find(id);
            if (yeuThich == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index", "SanPhams");
        }
    }


    

}