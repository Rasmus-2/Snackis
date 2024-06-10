using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Snackis.Data;
using Snackis.Models;

namespace Snackis.Pages.Admin.MyInfoPageAdmin
{
    public class DeleteModel : PageModel
    {
        private readonly Snackis.Data.SnackisContext _context;

        public DeleteModel(Snackis.Data.SnackisContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MyInfoPage MyInfoPage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myinfopage = await _context.MyInfoPage.FirstOrDefaultAsync(m => m.Id == id);

            if (myinfopage == null)
            {
                return NotFound();
            }
            else
            {
                MyInfoPage = myinfopage;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myinfopage = await _context.MyInfoPage.FindAsync(id);
            if (myinfopage != null)
            {
                MyInfoPage = myinfopage;
                _context.MyInfoPage.Remove(MyInfoPage);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
