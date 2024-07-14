using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    public class EditModel : PageModel
    {
        
        private readonly ApplicationDbContext _context;
        [BindProperty]
        public Category category { get; set; }
        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public void OnGet(int? categoryId)
        {
            if(categoryId != null && categoryId != 0)
                category = _context.Categories.Find(categoryId);
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
                TempData["success"] = "Category updated Successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
