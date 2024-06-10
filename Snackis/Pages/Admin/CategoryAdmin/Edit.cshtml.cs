using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Snackis.Data;
using Snackis.Models;

namespace Snackis.Pages.Admin.CategoryAdmin
{
    public class EditModel : PageModel
    {
        private readonly Snackis.Data.SnackisContext _context;
        private readonly DAL.CategoryManager _categoryManager;

        public EditModel(Snackis.Data.SnackisContext context, DAL.CategoryManager categoryManager)
        {
            _context = context;
            _categoryManager = categoryManager;
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryManager.GetCategory((int)id);
            if (category == null)
            {
                return NotFound();
            }
            Category = category;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!CategoryExists(Category.Id).Result)
            {
                return NotFound();
            }
            else
            {
                await _categoryManager.UpdateCategory(Category.Id, Category);
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool> CategoryExists(int id)
        {
            Models.Category category = await _categoryManager.GetCategory(id);
            if (category == null)
            {
                return false;
            }
            return true;
        }
    }
}
