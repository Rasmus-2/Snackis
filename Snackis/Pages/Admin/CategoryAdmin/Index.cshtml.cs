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
    public class IndexModel : PageModel
    {
        private readonly Snackis.Data.SnackisContext _context;
        private readonly DAL.CategoryManager _categoryManager;

        public IndexModel(Snackis.Data.SnackisContext context, DAL.CategoryManager categoryManager)
        {
            _context = context;
            _categoryManager = categoryManager;
        }

        public IList<Category> Category { get;set; } = default!;

        public async Task OnGetAsync()
        {   
            Category = await _categoryManager.GetCategories();
        }
    }
}
