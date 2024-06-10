using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Snackis.Pages
{
    public class IndexModel : PageModel
    {
        public Areas.Identity.Data.SnackisUser MyUser { get; set; }

        private UserManager<Areas.Identity.Data.SnackisUser> _userManager { get; set; }
        private readonly Snackis.Data.SnackisContext _context;
        private readonly DAL.CategoryManager _categoryManager;

        public IndexModel(UserManager<Areas.Identity.Data.SnackisUser> userManager, Data.SnackisContext context, DAL.CategoryManager categoryManager)
        {
            _userManager = userManager;
            _context = context;
            _categoryManager = categoryManager;
        }


        public List<Models.Category> Categories { get; set; }
        public List<Models.Category> TopCategories { get; set; }
        public Models.Category ChoosenCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(int showId)
        {
            MyUser = await _userManager.GetUserAsync(User);

            Categories = await _categoryManager.GetCategories();

            TopCategories = Categories.Where(c => c.ParentId == null).ToList();

            if (showId != 0)
            {
                ChoosenCategory = Categories.Where(c => c.Id == showId).FirstOrDefault();
                if (ChoosenCategory != null)
                {
                    return RedirectToPage("./Forum", "OnGetAsync", new { categoryId = showId });
                }
            }
            return null;
        }
    }
}
