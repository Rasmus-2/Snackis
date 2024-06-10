using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Snackis.Data;
using Snackis.Models;

namespace Snackis.Pages.Admin.CategoryAdmin
{
    public class CreateModel : PageModel
    {
        private readonly Snackis.Data.SnackisContext _context;
        private readonly DAL.CategoryManager _categoryManager;

        public CreateModel(Snackis.Data.SnackisContext context, DAL.CategoryManager categoryManager)
        {
            _context = context;
            _categoryManager = categoryManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _categoryManager.AddCategory(Category);

            return RedirectToPage("./Index");
        }
    }
}
