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

namespace Snackis.Pages.Admin.MyInfoPageAdmin
{
    public class EditModel : PageModel
    {
        private readonly Snackis.Data.SnackisContext _context;

        public EditModel(Snackis.Data.SnackisContext context)
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

            var myinfopage =  await _context.MyInfoPage.FirstOrDefaultAsync(m => m.Id == id);
            if (myinfopage == null)
            {
                return NotFound();
            }
            MyInfoPage = myinfopage;
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

            _context.Attach(MyInfoPage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MyInfoPageExists(MyInfoPage.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MyInfoPageExists(int id)
        {
            return _context.MyInfoPage.Any(e => e.Id == id);
        }
    }
}
