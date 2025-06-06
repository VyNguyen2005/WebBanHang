﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBanHang.Areas.Customer.Models;
using WebBanHang.Models;

namespace WebBanHang.Areas.Customer.Controllers
{
    [Authorize]
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly MyWebContext _db;
        public OrderController(MyWebContext db)
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
            ViewBag.CART = cart;
            return View();
        }
        public IActionResult ProcessOrder(Order order)
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("CART");

            if (ModelState.IsValid)
            {
                order.OrderDate = DateTime.Now;
                order.Total = cart.Total;
                order.State = "Pending";
                _db.Orders.Add(order);
                _db.SaveChanges();
                foreach (var item in cart.Items)
                {
                    var orderdetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        ProductId = item.Product.Id,
                        Quantity = item.Quantity
                    };
                    _db.OrderDetails.Add(orderdetail);
                    _db.SaveChanges();
                }
                HttpContext.Session.Remove("CART");
                return View("Result");
            }
            ViewBag.CART = cart;
            return View("Index", order);
        }
    }
}
