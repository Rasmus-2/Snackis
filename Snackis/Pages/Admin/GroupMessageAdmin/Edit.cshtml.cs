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

namespace Snackis.Pages.Admin.GroupMessageAdmin
{
    public class EditModel : PageModel
    {
        private readonly Snackis.Data.SnackisContext _context;

        public EditModel(Snackis.Data.SnackisContext context)
        {
            _context = context;
        }

        [BindProperty]
        public GroupMessage GroupMessage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupmessage =  await _context.GroupMessage.FirstOrDefaultAsync(m => m.Id == id);
            if (groupmessage == null)
            {
                return NotFound();
            }
            GroupMessage = groupmessage;
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

            _context.Attach(GroupMessage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupMessageExists(GroupMessage.Id))
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

        private bool GroupMessageExists(int id)
        {
            return _context.GroupMessage.Any(e => e.Id == id);
        }
    }
}
