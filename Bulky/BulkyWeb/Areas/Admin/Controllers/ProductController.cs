using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess.UnitOfWork;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.productRepo.GetAll(null, "Category").ToList();
            return View(objProductList);
        }
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> categoryList = _unitOfWork.categoryRepo.GetAll().Select(
                c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }
            );
            // ViewBag.CategoryList = categoryList;
            // ViewData["CategoryList"] = categoryList;

            ProductViewModel productViewModel = new()
            {
                Product = new Product(),
                CategoryList = categoryList
            };

            return View(productViewModel);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(ProductViewModel productViewModel, IFormFile? file)
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
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productViewModel.Product.ImageUrl = @"\images\product\" + fileName;
                }

                _unitOfWork.productRepo.Add(productViewModel.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product Created Successfully";
                return RedirectToAction("Index", "Product");
            }
            else
            {
                productViewModel.CategoryList = _unitOfWork.categoryRepo.GetAll().Select(
                    c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }
                );
                return View(productViewModel);
            }
        }
        public IActionResult Edit(int? productId)
        {
            if (productId == null || productId == 0)
            {
                return NotFound();
            }
            Product productFromDb = _unitOfWork.productRepo.Get(c => c.Id == productId);
            if (productFromDb == null)
            {
                return NotFound();
            }
            ProductViewModel productViewModel = new()
            {
                Product = productFromDb,
                CategoryList = _unitOfWork.categoryRepo.GetAll().Select(
                    c => new SelectListItem { Text = c.Name, Value = c.Id.ToString(), Selected = c.Id == productFromDb.CategoryId }
                )
            };
            return View(productViewModel);
        }
        [HttpPost]
        public IActionResult Edit(ProductViewModel productViewModel, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productViewModel.Product.ImageUrl))
                    {
                        string oldFilePath = Path.Combine(wwwRootPath, productViewModel.Product.ImageUrl.Remove(0, 1));

                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productViewModel.Product.ImageUrl = @"\images\product\" + fileName;
                }
                _unitOfWork.productRepo.Update(productViewModel.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product updated Successfully";
                return RedirectToAction("Index", "Product");
            }
            else
            {
                productViewModel.CategoryList = _unitOfWork.categoryRepo.GetAll().Select(
                    c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }
                );
                return View(productViewModel);
            }
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.productRepo.GetAll(null, "Category").ToList();
            return Json(new
            {
                data = objProductList
            });
        }
        [HttpDelete]
        public IActionResult Delete(int? productId)
        {
            var productToBeDeleted = _unitOfWork.productRepo.Get(u => u.Id == productId);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.productRepo.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}

