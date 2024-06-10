using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Snackis.Data;
using Snackis.Models;

namespace Snackis.Pages.Admin.InviteAdmin
{
    public class DetailsModel : PageModel
    {
        private readonly Snackis.Data.SnackisContext _context;

        public DetailsModel(Snackis.Data.SnackisContext context)
        {
            _context = context;
        }

        public Invite Invite { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invite = await _context.Invite.FirstOrDefaultAsync(m => m.Id == id);
            if (invite == null)
            {
                return NotFound();
            }
            else
            {
                Invite = invite;
            }
            return Page();
        }
    }
}
