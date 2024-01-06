using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using WebBanHang.Models.EF;

namespace WebBanHang.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Employee")]

    public class ProductImagesController : Controller
    {
        // GET: Admin/ProductImages
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(int id)
        {
            ViewBag.productId = id;
            var items = db.ProductImages.Where(x => x.ProductID == id).ToList();
            return View(items);
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.ProductImages.Find(id);
            db.ProductImages.Remove(item);
            db.SaveChanges();
            return Json(new { success=true});

        }
        [HttpPost]
        public ActionResult AddImage(int productId,string url)
        {
            db.ProductImages.Add(new ProductImage
            {
                ProductID=productId,
                Image = url,
                IsDefault = false
            });
            db.SaveChanges();
            return Json(new { Success = true });

        }
    }
}