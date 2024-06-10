using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Snackis.Data;
using Snackis.Models;

namespace Snackis.Pages.Admin.FlagPostAdmin
{
    public class CreateModel : PageModel
    {
        private readonly Snackis.Data.SnackisContext _context;

        public CreateModel(Snackis.Data.SnackisContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public FlagPost FlagPost { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.FlagPost.Add(FlagPost);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
