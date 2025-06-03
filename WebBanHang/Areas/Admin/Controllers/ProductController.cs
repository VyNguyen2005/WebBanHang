using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebBanHang.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly MyWebContext _db;
        private readonly IWebHostEnvironment _hosting;
        public ProductController(IWebHostEnvironment hosting , MyWebContext db)
        {
            _db = db;
            _hosting = hosting;
        }
        public const int ITEMS_PER_PAGE = 5;
        public int currentPage { get; set; }
        public int countPages { get; set; }
        public IActionResult Index(int page = 1)
        {
            int totalItems = _db.Products.Count();
            int countPages = (int)Math.Ceiling((double)totalItems / ITEMS_PER_PAGE);
            if (page < 1) page = 1;
            if (page > countPages) page = countPages;

            var productList = _db.Products
                .Include(x => x.Category)
                .Skip((page - 1) * ITEMS_PER_PAGE)
                .Take(ITEMS_PER_PAGE)
                .ToList();

            var viewModel = new WebBanHang.Models.ViewModels.PagingModel()
            {
                Products = productList,
                CurrentPage = page,
                CountPages = countPages,
                GenerateUrl = p => Url.Action("Index", new { page = p })
            };

            return View(viewModel);
        }
        public IActionResult Add()
        {
            ViewBag.CategoryList = _db.Categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
            return View();
        }
        [HttpPost]
        public IActionResult Add(Product product, IFormFile ImageUrl)
        {
            if (ModelState.IsValid)
            {
                if (ImageUrl != null)
                {
                    product.ImageUrl = SaveImage(ImageUrl);
                }
                _db.Products.Add(product);
                _db.SaveChanges();
                TempData["success"] = "Product inserted success";
                return RedirectToAction("Index");
            }
            ViewBag.CategoryList = _db.Categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
            return View();

        }

        private string SaveImage(IFormFile image)
        {
            var filename = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var path = Path.Combine(_hosting.WebRootPath, @"images/products");
            var saveFile = Path.Combine(path, filename);
            using (var filestream = new FileStream(saveFile, FileMode.Create))
            {
                image.CopyTo(filestream);
            }
            return @"images/products/" + filename;
        }
        public IActionResult Delete(int id)
        {
            var product = _db.Products
            .Include(x => x.Category)
            .FirstOrDefault(x => x.Id == id);
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            if (!String.IsNullOrEmpty(product.ImageUrl))
            {
                var oldFilePath = Path.Combine(_hosting.WebRootPath, product.ImageUrl);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }
            _db.Products.Remove(product);
            _db.SaveChanges();
            TempData["success"] = "Product deleted success";
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            var product = _db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.CategoryList = _db.Categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
            return View(product);
        }
        [HttpPost]
        public IActionResult Update(Product product, IFormFile ImageUrl)
        {
            if (ModelState.IsValid) 
            {
                var existingProduct = _db.Products.Find(product.Id);
                if (ImageUrl != null)
                {
                    product.ImageUrl = SaveImage(ImageUrl);
                    if (!string.IsNullOrEmpty(existingProduct.ImageUrl))
                    {
                        var oldFilePath = Path.Combine(_hosting.WebRootPath, existingProduct.ImageUrl);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }
                }
                else
                {
                    product.ImageUrl = existingProduct.ImageUrl;
                }
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.ImageUrl = product.ImageUrl;
                _db.SaveChanges();
                TempData["success"] = "Product updated success";
                return RedirectToAction("Index");
            }
            ViewBag.CategoryList = _db.Categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            return View();
        }
    }
}