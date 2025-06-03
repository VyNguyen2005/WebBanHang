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
            int selectedId = id ?? listCategory.FirstOrDefault()?.Id ?? 1;
            var listProduct = _db.Products.Where(x => x.CategoryId == selectedId).ToList();
            var categoryName = _db.Categories.FirstOrDefault(x => x.Id == selectedId)?.Name;
            ViewBag.listCategory = listCategory;
            ViewBag.CATEGORY_NAME = categoryName;
            return View(listProduct);
        }
        public IActionResult LoadPartial(int catid = 1)
        {
            var dsSanPham = _db.Products.Where(p => p.CategoryId == catid).ToList();
            var catName = _db.Categories.Find(catid).Name;
            ViewBag.CATEGORY_NAME = catName;
            return PartialView("ProductPartial", dsSanPham);
        }
    }
}
