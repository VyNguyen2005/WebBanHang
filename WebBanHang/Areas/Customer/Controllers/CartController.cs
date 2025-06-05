using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebBanHang.Areas.Customer.Models;
using WebBanHang.Models;

namespace WebBanHang.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly MyWebContext _db;
        public CartController(MyWebContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("CART");
            if(cart == null)
            {
                cart = new Cart();
            }
            return View(cart);
        }
        public IActionResult AddToCart(int productId)
        {
            var product = _db.Products.FirstOrDefault(x => x.Id == productId);
            if (product != null)
            {
                Cart cart = HttpContext.Session.GetJson<Cart>("CART");
                if (cart == null)
                {
                    cart = new Cart();
                }
                cart.Add(product, 1);
                HttpContext.Session.SetJson("CART", cart);
                return RedirectToAction("Index");
            }
            return Json(new { msg = "error" });
        }
        public IActionResult AddToCartAPI(int productId)
        {
            var product = _db.Products.FirstOrDefault(x => x.Id == productId);
            if(product != null)
            {
                Cart cart = HttpContext.Session.GetJson<Cart>("CART");
                if(cart == null)
                {
                    cart = new Cart();
                }
                cart.Add(product, 1);
                HttpContext.Session.SetJson("CART", cart);
                return Json(new { msg= "Product added to cart", quantity = cart.Quantity });
            }
            return Json(new { msg = "error" });
        }
        public IActionResult GetQuantityOfCart()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("CART");
            if(cart != null)
            {
                return Json(new { quantity = cart.Quantity });
            }
            return Json(new { quantity = 0 });
        }
        public IActionResult Update(int productId, int quantity)
        {
            var product = _db.Products.FirstOrDefault(x => x.Id == productId);
            if(product != null)
            {
                Cart cart = HttpContext.Session.GetJson<Cart>("CART");
                if(cart != null)
                {
                    cart.Update(productId, quantity);
                    HttpContext.Session.SetJson("CART", cart);
                    return RedirectToAction("Index");
                }
            }
            return Json(new { msg = "error" });
        }
        [HttpPost]
        public IActionResult UpdateCartItemAPI(int productId, int quantity)
        {
            var product = _db.Products.FirstOrDefault(x => x.Id == productId);
            if (product != null)
            {
                var cart = HttpContext.Session.GetJson<Cart>("CART") ?? new Cart();
                cart.Update(productId, quantity);
                HttpContext.Session.SetJson("CART", cart);
                return Json(new { msg = "Updated", quantity = cart.Quantity, total = cart.Total.ToString("#,##0") });
            }
            return Json(new { msg = "Error" });
        }
        [HttpPost]
        public IActionResult RemoverFromCartAPI(int productId)
        {
            var cart = HttpContext.Session.GetJson<Cart>("CART");
            if (cart != null)
            {
                cart.Remove(productId);
                HttpContext.Session.SetJson("CART", cart);
                return Json(new { msg = "Removed", quantity = cart.Quantity });
            }
            return Json(new { msg = "Error" });
        }
        public IActionResult Remove(int productId)
        {
            var product = _db.Products.FirstOrDefault(x => x.Id == productId);
            if(product != null)
            {
                Cart cart = HttpContext.Session.GetJson<Cart>("CART");
                if(cart != null)
                {
                    cart.Remove(productId);
                    HttpContext.Session.SetJson("CART", cart);
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}
