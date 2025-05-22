using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class CategoryController : Controller
    {
        private readonly MyWebContext _db;
        private readonly IWebHostEnvironment _hosting;
        public CategoryController(IWebHostEnvironment hosting, MyWebContext db)
        {
            _db = db;
            _hosting = hosting;
        }
        public IActionResult Index()
        {
            var listCategory = _db.Categories.ToList();
            return View(listCategory);
        }
        public IActionResult Delete(int id)
        {
            var category = _db.Categories.Where(x => x.Id == id).FirstOrDefault();
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(category);
            _db.SaveChanges();
            TempData["success"] = "Category deleted success";
            return RedirectToAction("Index");
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Category category)
        {
            if (ModelState.IsValid) 
            {
                _db.Categories.Add(category);
                _db.SaveChanges();
                TempData["success"] = "Category inserted success";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Update(int id)
        {
            var category = _db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult Update(Category category)
        {
            if (ModelState.IsValid) 
            {
                _db.Categories.Update(category);
                _db.SaveChanges();
                TempData["success"] = "Category updated success";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
