using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Snackis.Pages.Admin
{
    public class IndexModel : PageModel
    {
        public Areas.Identity.Data.SnackisUser MyUser { get; set; }

        private UserManager<Areas.Identity.Data.SnackisUser> _userManager { get; set; }
        private readonly Snackis.Data.SnackisContext _context;
        private readonly Services.Helpers _helpers;

        public IndexModel(UserManager<Areas.Identity.Data.SnackisUser> userManager, Data.SnackisContext context, Services.Helpers helpers)
        {
            _userManager = userManager;
            _context = context;
            _helpers = helpers;
        }


        public List<Models.FlagPost> FlagPosts { get; set; }

        public async Task OnGetAsync(int deleteId)
        {
            if (deleteId != 0)
            {
                Models.FlagPost flagPostToDelete = await _context.FlagPost.FindAsync(deleteId);
                if (flagPostToDelete != null)
                {
                    Models.ForumPost forumPostToDelete = await _context.ForumPost.FindAsync(flagPostToDelete.ForumPostId);
                    if (forumPostToDelete != null)
                    {
                        _context.FlagPost.Remove(flagPostToDelete);
                        _context.ForumPost.Remove(forumPostToDelete);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            List<Models.FlagPost> flagPosts = await _context.FlagPost.ToListAsync();
            FlagPosts = flagPosts.OrderByDescending(f => f.FlaggedByUserIds.Count()).ToList();
        }

        public async Task<Models.ForumPost> GetCurrentForumPost(int forumPostId)
        {
            return await _helpers.GetCurrentForumPost(forumPostId);
        }

        public async Task<string> FindUserName(string userId)
        {
            return await _helpers.FindUserName(userId);
        }
    }
}
