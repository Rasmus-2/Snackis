using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Snackis.Pages
{
    public class ForumModel : PageModel
    {
        public Areas.Identity.Data.SnackisUser MyUser { get; set; }

        private UserManager<Areas.Identity.Data.SnackisUser> _userManager { get; set; }
        private readonly Snackis.Data.SnackisContext _context;
        private readonly DAL.CategoryManager _categoryManager;

        public ForumModel(UserManager<Areas.Identity.Data.SnackisUser> userManager, Data.SnackisContext context, DAL.CategoryManager categoryManager)
        {
            _userManager = userManager;
            _context = context;
            _categoryManager = categoryManager;
        }

        public Models.Category Category { get; set; }
        public List<Models.ForumPost> ForumPosts { get; set; }

        [BindProperty]
        public Models.ForumPost NewPost { get; set; }

        [BindProperty]
        public IFormFile UploadedImage { get; set; }

        public async Task<IActionResult> OnGetAsync(int categoryId, int showThreadId, int deleteId)
        {
            MyUser = await _userManager.GetUserAsync(User);

            if (showThreadId != 0)
            {
                return RedirectToPage("./Thread", "OnGetAsync", new { threadId = showThreadId });
            }

            Category = await _categoryManager.GetCategory(categoryId);

            List<Models.ForumPost> forumPosts = await _context.ForumPost.Where(f => f.CategoryId == categoryId && f.ParentId == null).ToListAsync();
            ForumPosts = forumPosts.OrderBy(f => f.Date).ToList();


            if (deleteId != 0)
            {
                Models.ForumPost postToDelete = await _context.ForumPost.FindAsync(deleteId);
                if (postToDelete != null)
                {
                    if (System.IO.File.Exists("./wwwroot/images/postImages/" + postToDelete.Image))
                    {
                        System.IO.File.Delete("./wwwroot/images/postImages/" + postToDelete.Image);
                    }

                    _context.ForumPost.Remove(postToDelete);
                    await  _context.SaveChangesAsync();
                    return RedirectToPage("./Forum", "OnGetAsync", new { categoryId = postToDelete.CategoryId });
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var image = UploadedImage;
            string fileName = "";

            if (image != null)
            {
                Random rnd = new();
                fileName = rnd.Next(0, 1000000).ToString() + image.FileName;
                using (var fileStream = new FileStream("./wwwroot/images/postImages/" + fileName, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
            }
            NewPost.Image = fileName;

            NewPost.Date = DateTime.Now;
            NewPost.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            NewPost.UserIdLikes = new List<string>();

            _context.ForumPost.Add(NewPost);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Forum", "OnGetAsync", new { categoryId = NewPost.CategoryId });
        }
    }
}
