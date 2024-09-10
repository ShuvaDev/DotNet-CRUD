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
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Company company)
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.companyRepo.Add(company);
                _unitOfWork.Save();
                TempData["success"] = "Company Created Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? companyId)
        {
            Company companyObj = _unitOfWork.companyRepo.Get(c => c.Id == companyId);
            return View(companyObj);
        }
        [HttpPost]
        public IActionResult Edit(Company company)
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.companyRepo.Update(company);
                _unitOfWork.Save();
                TempData["success"] = "Company updated Successfully";
                return RedirectToAction("Index");
            }
            return View(company);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.companyRepo.GetAll().ToList();
            return Json(new
            {
                data = objCompanyList
            });
        }
        [HttpDelete]
        public IActionResult Delete(int? companyId)
        {
            var companyToBeDeleted = _unitOfWork.companyRepo.Get(c => c.Id == companyId);
            if(companyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.companyRepo.Remove(companyToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }
    }
}