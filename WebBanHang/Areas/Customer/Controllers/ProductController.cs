using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Areas.Customer.Models;
using WebBanHang.Models;

namespace WebBanHang.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductController : Controller
    {
        private readonly MyWebContext _db;
        public ProductController(MyWebContext db)
        {
            _db = db;
        }
        public IActionResult Index(int? id)
        {
            var listCategory = _db.Categories.OrderBy(x => x.DisplayOrder)
                                             .Select(c => new CategoryViewModel { Id = c.Id, Name = c.Name, TotalProduct = _db.Products.Where(p => p.CategoryId == c.Id).Count() }).ToList()
                                             .ToList();
            var listProduct = _db.Products.Where(x => x.CategoryId == id).ToList();
            var categoryName = _db.Categories.FirstOrDefault(x => x.Id == id)?.Name;
            ViewBag.listCategory = listCategory;
            ViewBag.categoryName = categoryName;
            return View(listProduct);
        }
    }
}
