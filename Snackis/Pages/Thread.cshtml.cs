using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Snackis.Services;
using System.Security.Claims;

namespace Snackis.Pages
{
    public class ThreadModel : PageModel
    {
        public Areas.Identity.Data.SnackisUser MyUser { get; set; }

        private UserManager<Areas.Identity.Data.SnackisUser> _userManager { get; set; }
        private readonly Snackis.Data.SnackisContext _context;
        private readonly Services.Helpers _helpers;

        public ThreadModel(UserManager<Areas.Identity.Data.SnackisUser> userManager, Data.SnackisContext context, Services.Helpers helpers)
        {
            _userManager = userManager;
            _context = context;
            _helpers = helpers;
        }


        public List<Models.ForumPost> ThreadPosts { get; set; }
        public List<Models.ForumPost> SubPosts { get; set; }
        public Models.ForumPost OriginalPost { get; set; }

        [BindProperty]
        public Models.ForumPost NewPost { get; set; }

        public int MakeNewPost { get; set; }

        public IFormFile UploadedImage { get; set; }

        public async Task<IActionResult> OnGetAsync(int threadId, int answerId, int deleteId, int likeId, string userId, int flagPostId)
        {

            if (userId != null)
            {
                return RedirectToPage("./YourPage", "OnGetAsync", new { userId = userId });
            }

            MyUser = await _userManager.GetUserAsync(User);

            if (answerId != 0)
            {
                MakeNewPost = answerId;
            }

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
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Forum", "OnGetAsync", new { categoryId = postToDelete.CategoryId });
                }
            }

            if (likeId != 0)
            {
                Models.ForumPost likePost = await _context.ForumPost.FindAsync(likeId);
                if (MyUser.LikedPostsIds.Contains(likeId))
                {
                    MyUser.LikedPostsIds.Remove(likeId);
                    if (likePost.UserIdLikes.Contains(MyUser.Id))
                    {
                        likePost.UserIdLikes.Remove(MyUser.Id);
                    }
                }
                else
                {
                    MyUser.LikedPostsIds.Add(likeId);
                    likePost.UserIdLikes.Add(MyUser.Id);
                }

                _context.Update(likePost);
                await _userManager.UpdateAsync(MyUser);
                await _context.SaveChangesAsync();
            }

            if (flagPostId != 0)
            {
                Models.FlagPost flagPost = await _context.FlagPost.Where(f => f.ForumPostId == flagPostId).FirstOrDefaultAsync();
                if (flagPost != null)
                {
                    flagPost.FlaggedByUserIds.Add(MyUser.Id);
                    _context.FlagPost.Update(flagPost);
                }
                else
                {
                    Models.FlagPost newFlagPost = new Models.FlagPost()
                    {
                        ForumPostId = flagPostId,
                        FlaggedByUserIds = new List<string>() { MyUser.Id }
                    };
                    _context.FlagPost.Add(newFlagPost);
                }
                await _context.SaveChangesAsync();
            }


            List<Models.ForumPost> posts = await _context.ForumPost.ToListAsync();
            if (posts != null)
            {
                OriginalPost = posts.Where(p => p.Id == threadId).FirstOrDefault();
                if (OriginalPost != null)
                {
                    List<Models.ForumPost> threadPosts = posts.Where(p => p.ParentId == OriginalPost.Id).ToList();
                    List<Models.ForumPost> subPosts = new List<Models.ForumPost>();
                    foreach (var post in threadPosts)
                    {
                        foreach (var subPost in posts)
                        {
                            if (subPost.ParentId == post.Id)
                            {
                                subPosts.Add(subPost);
                            }
                        }
                    }
                    ThreadPosts = threadPosts.OrderBy(t => t.Date).ToList();
                    SubPosts = subPosts.OrderBy(t => t.Date).ToList();
                }
            }
            return null;
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

            int returnToThreadId = await FindTopLevelPostId((int)NewPost.ParentId);

            return RedirectToPage("./Thread", "OnGetAsync", new { threadId = returnToThreadId });
        }

        public async Task<string> FindUserName(string userId)
        {
            return await _helpers.FindUserName(userId);
        }

        public async Task<string> FindUserProfilePicture(string userId)
        {
            return await _helpers.FindUserProfilePicture(userId);
        }

        public async Task<bool> AlreadyFlaggedByMe(int forumPostId)
        {
            MyUser = await _userManager.GetUserAsync(User);
            return await _helpers.AlreadyFlaggedByMe(forumPostId, MyUser.Id);
        }

        public async Task<int> FindTopLevelPostId(int postId)
        {
            return await _helpers.FindTopLevelPostId(postId);
        }
    }
}
