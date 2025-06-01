using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class ProductController : Controller
    {
        private readonly MyWebContext _db;
        public ProductController(MyWebContext db)
        {
            _db = db;
        }
        public IActionResult Index(int id)
        {
            var listProduct = _db.Products.Include(x => x.Category).Where(x => x.CategoryId == id).ToList();
            return View(listProduct);
        }
        public IActionResult getCategory()
        {
            var listCategory = _db.Categories.ToList();
            return PartialView("CategoryPartial", listCategory);
        }
    }
}
