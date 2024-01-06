using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Role
        public ActionResult Index()
        {
            var items = db.Roles.ToList();
            return View(items);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                roleManager.Create(model);
                return RedirectToAction("index");
            }
            return View(model);
        }
        public ActionResult Edit(string id)
        {
            var item = db.Roles.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                roleManager.Update(model);
                return RedirectToAction("index");
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult Delete(string roleId)
        {
            if (!string.IsNullOrEmpty(roleId))
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                var role = roleManager.FindById(roleId);
                if (role != null)
                {
                    roleManager.Delete(role);
                    return Json(new { success = true });
                }
                return Json(new { success = false });

            }
            return Json(new { success = false });

        }
    }
}