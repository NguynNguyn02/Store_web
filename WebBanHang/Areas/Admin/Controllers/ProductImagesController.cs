using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
namespace WebBanHang.Areas.Admin.Controllers
{
    public class ProductImagesController : Controller
    {
        // GET: Admin/ProductImages
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(int id)
        {
            var items = db.ProductImages.Where(x => x.ProductID == id).ToList();
            return View(items);
        }
    }
}