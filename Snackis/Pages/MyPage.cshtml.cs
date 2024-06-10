using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Snackis.Pages
{
    [Authorize]
    public class MyPageModel : PageModel
    {
        public Areas.Identity.Data.SnackisUser MyUser { get; set; }

        private UserManager<Areas.Identity.Data.SnackisUser> _userManager { get; set; }
        private readonly Snackis.Data.SnackisContext _context;
        private readonly Services.Helpers _helpers;

        public MyPageModel(UserManager<Areas.Identity.Data.SnackisUser> userManager, Data.SnackisContext context, Services.Helpers helpers)
        {
            _userManager = userManager;
            _context = context;
            _helpers = helpers;
        }

        [BindProperty]
        public Models.MyInfoPage InfoPage { get; set; }

        public IFormFile UploadedImage { get; set; }

        public bool ChangeHeader { get; set; }
        public bool AddProfilePicture { get; set; }
        public bool AddMyPageImage { get; set; }
        public bool ChangeText { get; set; }

        public bool ShowMyPosts { get; set; }
        public List<Models.ForumPost> MyPosts { get; set; }

        public bool ShowMyLiked { get; set; }
        public List<Models.ForumPost> MyLiked { get; set; }

        public List<Models.Invite> MyInvites { get; set; }

        public async Task OnGetAsync(
            int changeHeaderId, int deleteProfilePictureId, int addProfilePictureId, int deleteMyPageImageId,
            int addMyPageImageId, int changeTextId, int showMyPostsId, int showMyLikedPostsId,
            int joinGroupId, int declineGroupId)
        {
            MyUser = await _userManager.GetUserAsync(User);
            if (MyUser != null)
            {
                if (showMyPostsId != 0)
                {
                    MyPosts = await _context.ForumPost.Where(f => f.UserId == MyUser.Id).ToListAsync();
                    ShowMyPosts = true;
                }

                if (showMyLikedPostsId != 0)
                {
                    MyLiked = await _context.ForumPost.Where(f => f.UserIdLikes.Contains(MyUser.Id)).ToListAsync();
                    ShowMyLiked = true;
                }

                if (joinGroupId != 0)
                {
                    Models.Group group = await _context.Group.Where(g => g.Id == joinGroupId).FirstOrDefaultAsync();
                    group.UserIds.Add(MyUser.Id);
                    _context.Group.Update(group);

                    Models.Invite invite = await _context.Invite.Where(i => i.GroupId == group.Id && i.ReceiverId == MyUser.Id).FirstOrDefaultAsync();
                    _context.Invite.Remove(invite);

                    await _context.SaveChangesAsync();
                }

                if (declineGroupId != 0)
                {
                    Models.Invite invite = await _context.Invite.Where(i => i.GroupId == declineGroupId && i.ReceiverId == MyUser.Id).FirstOrDefaultAsync();
                    _context.Invite.Remove(invite);
                    await _context.SaveChangesAsync();
                }

                MyInvites = await _context.Invite.Where(i => i.ReceiverId == MyUser.Id).ToListAsync();

                List<Models.MyInfoPage> myPages = await _context.MyInfoPage.Where(m => m.UserId == MyUser.Id).ToListAsync();
                InfoPage = myPages.FirstOrDefault();
                if (InfoPage == null)
                {
                    InfoPage = new Models.MyInfoPage()
                    {
                        LastChanged = DateTime.Now,
                        UserId = MyUser.Id,
                        Header = "Min sida"
                    };

                    _context.MyInfoPage.Add(InfoPage);
                    await _context.SaveChangesAsync();
                }

                if (changeHeaderId != 0)
                {
                    ChangeHeader = true;
                }

                if (addProfilePictureId != 0)
                {
                    AddProfilePicture = true;
                }

                if (deleteProfilePictureId != 0)
                {
                    if (MyUser.ProfilePicture != null)
                    {
                        if (System.IO.File.Exists("./wwwroot/images/profilePictureImages/" + MyUser.ProfilePicture))
                        {
                            System.IO.File.Delete("./wwwroot/images/profilePictureImages/" + MyUser.ProfilePicture);
                        }

                        MyUser.ProfilePicture = null;
                        await _userManager.UpdateAsync(MyUser);
                        await PageChanged(InfoPage.Id);
                    }
                }

                if (addMyPageImageId != 0)
                {
                    AddMyPageImage = true;
                }

                if (deleteMyPageImageId != 0)
                {
                    if (InfoPage.Image != null)
                    {
                        if (System.IO.File.Exists("./wwwroot/images/myPageImages/" + InfoPage.Image))
                        {
                            System.IO.File.Delete("./wwwroot/images/myPageImages/" + InfoPage.Image);
                        }

                        InfoPage.Image = null;
                        _context.MyInfoPage.Update(InfoPage);
                        await _context.SaveChangesAsync();
                        await PageChanged(InfoPage.Id);
                    }
                }

                if (changeTextId != 0)
                {
                    ChangeText = true;
                }
            }
        }

        public async Task<IActionResult> OnPostHeaderAsync()
        {
            Models.MyInfoPage infoPageToBeUpdated = await _context.MyInfoPage.FirstOrDefaultAsync(m => m.Id == InfoPage.Id);
            infoPageToBeUpdated.Header = InfoPage.Header;

            _context.MyInfoPage.Update(infoPageToBeUpdated);
            await _context.SaveChangesAsync();
            await PageChanged(InfoPage.Id);
            return RedirectToPage("./MyPage");
        }

        public async Task<IActionResult> OnPostProfilePictureAsync()
        {
            MyUser = await _userManager.GetUserAsync(User);
            var image = UploadedImage;

            if (image != null)
            {
                Random rnd = new();
                string fileName = rnd.Next(0, 1000000).ToString() + image.FileName;
                using (var fileStream = new FileStream("./wwwroot/images/profilePictureImages/" + fileName, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                if (fileName != null)
                {
                    MyUser.ProfilePicture = fileName;
                    await _userManager.UpdateAsync(MyUser);
                }
            }
            await PageChanged(InfoPage.Id);
            return RedirectToPage("./MyPage");
        }

        public async Task<IActionResult> OnPostMyPageImageAsync()
        {
            Models.MyInfoPage infoPageToBeUpdated = await _context.MyInfoPage.FirstOrDefaultAsync(m => m.Id == InfoPage.Id);
            var image = UploadedImage;

            if (image != null)
            {
                Random rnd = new();
                string fileName = rnd.Next(0, 1000000).ToString() + image.FileName;
                using (var fileStream = new FileStream("./wwwroot/images/myPageImages/" + fileName, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                if (fileName != null)
                {
                    infoPageToBeUpdated.Image = fileName;
                    _context.MyInfoPage.Update(infoPageToBeUpdated);
                    await _context.SaveChangesAsync();
                    await PageChanged(InfoPage.Id);
                }
            }

            return RedirectToPage("./MyPage");
        }

        public async Task<IActionResult> OnPostTextAsync()
        {
            Models.MyInfoPage infoPageToBeUpdated = await _context.MyInfoPage.FirstOrDefaultAsync(m => m.Id == InfoPage.Id);
            infoPageToBeUpdated.Text = InfoPage.Text;

            _context.MyInfoPage.Update(infoPageToBeUpdated);
            await _context.SaveChangesAsync();
            await PageChanged(InfoPage.Id);
            return RedirectToPage("./MyPage");
        }

        public async Task PageChanged(int pageId)
        {
            Models.MyInfoPage infoPageToBeUpdated = await _context.MyInfoPage.FirstOrDefaultAsync(m => m.Id == pageId);
            if (infoPageToBeUpdated != null)
            {
                infoPageToBeUpdated.LastChanged = DateTime.Now;
                _context.MyInfoPage.Update(infoPageToBeUpdated);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> GetGroupName(int groupId)
        {
            return await _helpers.GetGroupName(groupId);
        }

        public async Task<string> GetGropOwnerUsername(int groupId)
        {
            return await _helpers.GetGropOwnerUsername(groupId);
        }

        public async Task<string> FindUserName(string userId)
        {
            return await _helpers.FindUserName(userId);
        }

        public async Task<string> FindUserProfilePicture(string userId)
        {
            return await _helpers.FindUserProfilePicture(userId);
        }
    }
}