using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    // [BindProperties] If we want to bind all properties
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Category category { get; set; }
        private readonly ApplicationDbContext _context;
        public CreateModel(ApplicationDbContext context) 
        {
            _context = context;
        }
        public IActionResult OnPost()
        {
            if(ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                TempData["success"] = "Category Created Successfully";
                return RedirectToPage("Index");

            }
            return RedirectToPage("Create");
        }
        public void OnGet()
        {
            
        }
    }
}
