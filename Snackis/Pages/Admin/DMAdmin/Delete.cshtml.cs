using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Snackis.Data;
using Snackis.Models;

namespace Snackis.Pages.Admin.DMAdmin
{
    public class DeleteModel : PageModel
    {
        private readonly Snackis.Data.SnackisContext _context;

        public DeleteModel(Snackis.Data.SnackisContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DM DM { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dm = await _context.DM.FirstOrDefaultAsync(m => m.Id == id);

            if (dm == null)
            {
                return NotFound();
            }
            else
            {
                DM = dm;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dm = await _context.DM.FindAsync(id);
            if (dm != null)
            {
                DM = dm;
                _context.DM.Remove(DM);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
