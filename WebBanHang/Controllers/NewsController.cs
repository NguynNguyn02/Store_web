using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class NewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: News
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Partial_Home_News()
        {
            var items = db.News.Take(3).ToList();
            return PartialView(items);
        }
    }
}