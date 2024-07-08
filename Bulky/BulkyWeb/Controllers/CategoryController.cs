using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            /*
            if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The DisplayOrder can't exactly match the name.");
            }
            if (obj.Name.ToLower() == "test")
            {
                ModelState.AddModelError("", "Test is not valid.");
            }
            */
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index", "Category");
            } 
            return View();
        }
        public IActionResult Edit(int? categoryId)
        {
            if(categoryId == null || categoryId == 0)
            {
                return NotFound();
            }
            Category categoryFromDb = _db.Categories.Find(categoryId);
            if(categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Category updated Successfully";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }
        public IActionResult Delete(int? categoryId)
        {
            if (categoryId == null || categoryId == 0)
            {
                return NotFound();
            }
            Category categoryFromDb = _db.Categories.Find(categoryId);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        // কন্ট্রোলারে আমরা অবজেক্ট আকারেও ডাটা রিসিভ করতে পারি আবার attribute
        // এর নাম দিয়েও ডাটা রিসিভ করতে পারি, তবে কোন ফিল্ড ডিজেবল থাকলে তারা ডাটা রিসিভ করা যাবে না।
        public IActionResult DeletePOST(int? id)
        {
            Category deletedObj = _db.Categories.Find(id);
            _db.Categories.Remove(deletedObj);
            _db.SaveChanges();
            TempData["success"] = "Category deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}

