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
    public class IndexModel : PageModel
    {
        private readonly Snackis.Data.SnackisContext _context;

        public IndexModel(Snackis.Data.SnackisContext context)
        {
            _context = context;
        }

        public IList<Invite> Invite { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Invite = await _context.Invite.ToListAsync();
        }
    }
}
