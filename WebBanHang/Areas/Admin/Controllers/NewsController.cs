using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using WebBanHang.Models.EF;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        // GET: Admin/News
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var items = db.News.OrderByDescending(x => x.CreatedDate).ToList();
            return View(items);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(News model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.CategoryId = 18;
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHang.Models.Common.Filter.FilterChar(model.Title);
                db.News.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }


    }
}