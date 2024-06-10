using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Snackis.Services
{
    public class Helpers : IHelpers
    {
        private UserManager<Areas.Identity.Data.SnackisUser> _userManager { get; set; }
        private readonly Snackis.Data.SnackisContext _context;

        public Helpers(UserManager<Areas.Identity.Data.SnackisUser> userManager, Data.SnackisContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<string> FindUserName(string userId)
        {
            Areas.Identity.Data.SnackisUser user = await _userManager.FindByIdAsync(userId);
            return user.FirstName + " " + user.LastName;
        }

        public async Task<string> FindUsername(string userId)
        {
            List<Areas.Identity.Data.SnackisUser> users = await _userManager.Users.Where(u => u.Id == userId).ToListAsync();
            Areas.Identity.Data.SnackisUser user = users.FirstOrDefault();
            return user.UserName;
        }

        public async Task<string> FindUserProfilePicture(string userId)
        {
            List<Areas.Identity.Data.SnackisUser> users = await _userManager.Users.Where(u => u.Id == userId).ToListAsync();
            Areas.Identity.Data.SnackisUser user = users.FirstOrDefault();
            return user.ProfilePicture;
        }

        public async Task<bool> AlreadyFlaggedByMe(int forumPostId, string userId)
        {
            Models.FlagPost flagPost = await _context.FlagPost.Where(f => f.ForumPostId == forumPostId).FirstOrDefaultAsync();
            if (flagPost != null)
            {
                return flagPost.FlaggedByUserIds.Contains(userId);
            }
            return false;
        }

        public async Task<Models.ForumPost> GetCurrentForumPost(int forumPostId)
        {
            return await _context.ForumPost.FindAsync(forumPostId);
        }

        public async Task<string> GetGroupName(int groupId)
        {
            Models.Group group = await _context.Group.Where(g => g.Id == groupId).FirstOrDefaultAsync();
            return group.Name;
        }

        public async Task<string> GetGropOwnerUsername(int groupId)
        {
            Models.Group group = await _context.Group.Where(g => g.Id == groupId).FirstOrDefaultAsync();
            string userId = group.UserIds[0];
            string userName = await FindUsername(userId);
            return userName;
        }

        public async Task<int> FindTopLevelPostId(int postId)
        {
            Models.ForumPost post = await _context.ForumPost.FindAsync(postId);

            if (post.ParentId == null)
            {
                return post.Id;
            }
            else
            {
                return await FindTopLevelPostId((int)post.ParentId);
            }
        }
    }
}
