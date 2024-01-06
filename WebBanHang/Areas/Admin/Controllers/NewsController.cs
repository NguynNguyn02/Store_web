using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using WebBanHang.Models.EF;
using PagedList;
namespace WebBanHang.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Employee")]

    public class NewsController : Controller
    {
        // GET: Admin/News
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(int? page,string searchText)
        {
            var pageSize = 10;
            IEnumerable<News> items = db.News.OrderByDescending(x => x.CreatedDate);
            if (!string.IsNullOrEmpty(searchText))
            {
                items = items.Where(x => x.Alias.Contains(searchText) || x.Title.Contains(searchText));
            }
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex,pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.PageIndex = pageIndex;
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
        public ActionResult Edit(int id)
        {
            var item = db.News.Find(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(News model)
        {
            if (ModelState.IsValid)
            {
                
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHang.Models.Common.Filter.FilterChar(model.Title);
                db.News.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        
        [HttpPost]
        public ActionResult Delete(int id)
        {
              var item = db.News.Find(id);
            if (item != null)
            {
                db.News.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
           
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = db.News.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true,isActive= item.IsActive });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult deleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                foreach (var item in items)
                {
                    var obj = db.News.Find(Convert.ToInt32(item));
                    db.News.Remove(obj);
                    db.SaveChanges();
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}