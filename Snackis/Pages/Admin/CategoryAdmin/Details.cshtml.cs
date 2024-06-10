using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Snackis.Data;
using Snackis.Models;

namespace Snackis.Pages.Admin.CategoryAdmin
{
    public class DetailsModel : PageModel
    {
        private readonly Snackis.Data.SnackisContext _context;
        private readonly DAL.CategoryManager _categoryManager;

        public DetailsModel(Snackis.Data.SnackisContext context, DAL.CategoryManager categoryManager)
        {
            _context = context;
            _categoryManager = categoryManager;
        }

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
            else
            {
                Category = category;
            }
            return Page();
        }
    }
}
