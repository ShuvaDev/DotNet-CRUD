using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public Category category { get; set; }
        private readonly ApplicationDbContext _context;
        public DeleteModel(ApplicationDbContext context)
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
            category = _context.Categories.Find(category.Id);
            _context.Categories.Remove(category);
            _context.SaveChanges();
            TempData["success"] = "Category deleted Successfully";
            return RedirectToPage("Index");
        }
    }
}
