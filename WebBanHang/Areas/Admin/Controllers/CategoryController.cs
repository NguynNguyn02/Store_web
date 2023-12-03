using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using WebBanHang.Models.EF;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Admin/Category
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var item = db.Categories;
            return View(item);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Category model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHang.Models.Common.Filter.FilterChar(model.Title);
                db.Categories.Add(model);
                db.SaveChanges();
            return RedirectToAction("Index");
            }
            return View(model); 
        }
        public ActionResult Edit(int id)
        {
            var item = db.Categories.Find(id);
            return View();
        }
    }
}