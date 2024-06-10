using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Snackis.Data;
using Snackis.Models;

namespace Snackis.Pages.Admin.GroupMessageAdmin
{
    public class DeleteModel : PageModel
    {
        private readonly Snackis.Data.SnackisContext _context;

        public DeleteModel(Snackis.Data.SnackisContext context)
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

            var groupmessage = await _context.GroupMessage.FirstOrDefaultAsync(m => m.Id == id);

            if (groupmessage == null)
            {
                return NotFound();
            }
            else
            {
                GroupMessage = groupmessage;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupmessage = await _context.GroupMessage.FindAsync(id);
            if (groupmessage != null)
            {
                GroupMessage = groupmessage;
                _context.GroupMessage.Remove(GroupMessage);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
