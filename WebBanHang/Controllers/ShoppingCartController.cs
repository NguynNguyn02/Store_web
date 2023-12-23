using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppongCart
        public ActionResult Index()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                ViewBag.CheckOutCart = cart;
            }
            return View();
        }
        public ActionResult CheckOut()
        {
            
            return View();
        }
        public ActionResult Partial_Item_Cart()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }
        public ActionResult Partial_Item_Pay()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }
        public ActionResult ShowCount()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return Json(new { Count = cart.Items.Count }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new { success = false, msg = "", code = -1, Count = 0 };
            var db = new ApplicationDbContext();
            var checkProduct = db.Products.FirstOrDefault(x => x.Id == id);
            if (checkProduct != null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart == null)
                {
                    cart = new ShoppingCart();
                }
                ShoppingCartItem item = new ShoppingCartItem
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Title,
                    CategoryName = checkProduct.ProductCategory.Title,
                    Alias = checkProduct.Alias,
                    Quantity = quantity
                };
                if (checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault) != null)
                {
                    item.ProductImg = checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault).Image;
                }
                item.Price = checkProduct.Price;
                if (checkProduct.PriceSale > 0)
                {
                    item.Price = (decimal)checkProduct.PriceSale;
                }
                item.TotalPrice = item.Quantity * item.Price;
                cart.AddToCart(item, quantity);
                Session["Cart"] = cart;
                code = new { success = true, msg = "Thêm sản phẩm vào giỏ hàng thành công!", code = 1, Count = cart.Items.Count };
            }
            return Json(code);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];

            if (cart != null)
            {
                var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if (checkProduct != null)
                {
                    cart.Items.Remove(checkProduct);
                    return Json(new { id = id, success = true, Count = cart.Items.Count });
                }
            }

            return Json(new { id = id, success = false, Count = cart.Items.Count });
        }
        [HttpPost]
        public ActionResult Update(int id, int quantity)
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.UpdateQuantity(id, quantity);
                return Json(new { success=true});
            }

            return Json(new { success = false});

        }
        [HttpPost]

        public ActionResult DeleteAll()
        {

            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.ClearCart();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


    }
}