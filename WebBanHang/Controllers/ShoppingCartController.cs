using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using WebBanHang.Models.EF;

namespace WebBanHang.Controllers
{
    
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ShoppongCart
        public ActionResult Index()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckOutCart = cart;
            }
            return View();
        }
        public ActionResult CheckOut()
        {

            return View();
        }
        public ActionResult CheckOut_Success()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel model)
        {
            var code = new { success = false, Code = -1 };
            if (ModelState.IsValid)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart != null && cart.Items.Any())
                {
                    Order order = new Order();
                    order.CustomerName = model.CustomerName;
                    order.Address = model.Address;
                    order.Phone = model.Phone;
                    cart.Items.ForEach(x => order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = x.ProductId,
                        Price = x.Price,
                        Quantity = x.Quantity
                    }));
                    order.TotalAmount = cart.Items.Sum(x => (x.Quantity * x.Price));
                    order.ModifiedDate = DateTime.Now;
                    order.CreatedBy = model.Phone;
                    order.CreatedDate = DateTime.Now;
                    order.TypePayment = model.TypePayment;
                    
                    Random random = new Random();
                    order.Code = "DH" + random.Next(0, 9) + random.Next(0, 9) + random.Next(0, 9) + random.Next(0, 9) + random.Next(0, 9);
                    db.Orders.Add(order);
                    db.SaveChanges();
                    cart.ClearCart();
                    return RedirectToAction("CheckOut_Success");
                }
            }
            return Json(code);
        }
        public ActionResult Partial_CheckOut()
        {
            return PartialView();
        }
        public ActionResult Partial_Item_Cart()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }
        public ActionResult Partial_Item_Pay()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }
        public ActionResult ShowCount()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
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

            if (cart != null && cart.Items.Any())
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
            if (cart != null && cart.Items.Any())
            {
                cart.UpdateQuantity(id, quantity);
                return Json(new { success = true });
            }

            return Json(new { success = false });

        }
        [HttpPost]

        public ActionResult DeleteAll()
        {

            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                cart.ClearCart();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


    }
}