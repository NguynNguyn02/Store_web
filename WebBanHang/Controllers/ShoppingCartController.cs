using System;
using System.Collections.Generic;
using System.Configuration;
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
                        Quantity = x.Quantity,
                        
                    }));
                    order.TotalAmount = cart.Items.Sum(x => (x.Quantity * x.Price));
                    order.ModifiedDate = DateTime.Now;
                    order.CreatedBy = model.Phone;
                    order.CreatedDate = DateTime.Now;
                    order.TypePayment = model.TypePayment;
                    order.Email = model.Email;
                    Random random = new Random();
                    order.Code = "DH" + random.Next(0, 9) + random.Next(0, 9) + random.Next(0, 9) + random.Next(0, 9) + random.Next(0, 9);
                    db.Orders.Add(order);
                    db.SaveChanges();
                    //send mail cho khach hang
                    var strSanPham = "";
                    var thanhtien = decimal.Zero;
                    var tongtien = decimal.Zero; ;
                    foreach (var sp in cart.Items)
                    {
                        strSanPham += "<tr>";
                        strSanPham += "<td>"+ sp.ProductName+ "</td>";
                        strSanPham += "<td>"+ sp.Quantity + "</td>";
                        strSanPham += "<td>"+ WebBanHang.Common.Common.FormatNumber(sp.TotalPrice,0)+"$"+ "</td>";
                        strSanPham += "</tr>";
                        thanhtien += sp.Price * sp.Quantity;
                    }
                    var phuongthuc = "";
                    if ( model.TypePayment == 1)
                    {
                        phuongthuc = "Thanh toán khi nhận hàng";
                    }
                    else
                    {
                        phuongthuc = "Chuyển khoản";
                    }
                    
                    tongtien = thanhtien;
                    string contentCustom = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/send2.html"));
                    contentCustom = contentCustom.Replace("{{MaDon}}",order.Code);
                    contentCustom = contentCustom.Replace("{{SanPham}}", strSanPham);
                    contentCustom = contentCustom.Replace("{{TenKhachHang}}", order.CustomerName);
                    contentCustom = contentCustom.Replace("{{Phone}}", order.Phone);
                    contentCustom = contentCustom.Replace("{{Email}}", order.Email);
                    contentCustom = contentCustom.Replace("{{NgayDat}}",DateTime.Now.ToString("dd/MM/yyyy") );
                    contentCustom = contentCustom.Replace("{{DiaChiNhanHang}}", order.Address);
                    contentCustom = contentCustom.Replace("{{PhuongThucThanhToan}}", phuongthuc);
                    contentCustom = contentCustom.Replace("{{ThanhTien}}", WebBanHang.Common.Common.FormatNumber(thanhtien, 0));
                    contentCustom = contentCustom.Replace("{{TongTien}}", WebBanHang.Common.Common.FormatNumber(tongtien, 0));
                    WebBanHang.Common.Common.SendMail("TrungNguyenShop", "Đơn hàng + #" + order.Code, contentCustom, model.Email);

                    
                    string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/send1.html"));
                    contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
                    contentAdmin = contentAdmin.Replace("{{MaDon}}", order.Code);
                    contentAdmin = contentAdmin.Replace("{{TenKhachHang}}", order.CustomerName);
                    contentAdmin = contentAdmin.Replace("{{Phone}}", order.Phone);
                    contentAdmin = contentAdmin.Replace("{{Email}}", order.Email);
                    contentAdmin = contentAdmin.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentAdmin = contentAdmin.Replace("{{DiaChiNhanHang}}", order.Address);
                    contentAdmin = contentAdmin.Replace("{{PhuongThucThanhToan}}", phuongthuc);
                    contentAdmin = contentAdmin.Replace("{{ThanhTien}}", WebBanHang.Common.Common.FormatNumber(thanhtien, 0));
                    contentAdmin = contentAdmin.Replace("{{TongTien}}", WebBanHang.Common.Common.FormatNumber(tongtien, 0));
                    WebBanHang.Common.Common.SendMail("TrungNguyenShop", "Đơn hàng mới + #" + order.Code, contentAdmin, ConfigurationManager.AppSettings["EmailAdmin"]);
                    cart.ClearCart();

                    return RedirectToAction("CheckOut_Success", "ShoppingCart");
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