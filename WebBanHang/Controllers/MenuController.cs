using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
namespace WebBanHang.Controllers
{
    public class MenuController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MenuTop()
        {
            var item = db.Categories.OrderBy(x => x.Position).ToList();
            return PartialView("_MenuTop",item);
        }
        public ActionResult MenuProductCategory()
        {
            var items = db.ProductCategories.ToList();

            return PartialView("_MenuProductCategory", items);

        }
        public ActionResult MenuLeft(int? id)
        {
            if (id != null)
            {
                ViewBag.CateId = id;
            }
            var items = db.ProductCategories.ToList();

            return PartialView("_MenuLeft", items);

        }
        public ActionResult MenuArrivals()
        {
            var items = db.ProductCategories.ToList();

            return PartialView("_MenuArrivals", items);

        }

    }
}