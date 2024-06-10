using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Snackis.Pages
{
    [Authorize]
    public class GroupPageModel : PageModel
    {
        public Areas.Identity.Data.SnackisUser MyUser { get; set; }

        private UserManager<Areas.Identity.Data.SnackisUser> _userManager { get; set; }
        private readonly Snackis.Data.SnackisContext _context;
        private readonly Services.Helpers _helpers;

        public GroupPageModel(UserManager<Areas.Identity.Data.SnackisUser> userManager, Data.SnackisContext context, Services.Helpers helpers)
        {
            _userManager = userManager;
            _context = context;
            _helpers = helpers;
        }


        public List<Models.Group> MyGroups { get; set; }

        [BindProperty]
        public Models.Group CurrentGroup { get; set; }

        [BindProperty]
        public Models.Group NewGroup { get; set; }

        [BindProperty]
        public Models.GroupMessage NewMessage { get; set; }

        public List<Models.GroupMessage> GroupMessages { get; set; }

        public async Task OnGetAsync(int groupId, int deleteGroupId, string kickUserId)
        {
            if (deleteGroupId != 0)
            {
                Models.Group groupToDelete = await _context.Group.FindAsync(deleteGroupId);
                _context.Group.Remove(groupToDelete);
                await _context.SaveChangesAsync();
                //return RedirectToPage("./Forum", "OnGetAsync", new { categoryId = postToDelete.CategoryId });
            }

            if (kickUserId != null && groupId != 0)
            {
                Models.Group group = await _context.Group.Where(g => g.Id == groupId).FirstOrDefaultAsync();
                group.UserIds.Remove(kickUserId);
                _context.Group.Update(group);
                await _context.SaveChangesAsync();
            }

            if (groupId != 0)
            {
                CurrentGroup = await _context.Group.Where(g => g.Id == groupId).FirstOrDefaultAsync();
                GroupMessages = await _context.GroupMessage.Where(gm => gm.GroupId == groupId).ToListAsync();
            }

            MyUser = await _userManager.GetUserAsync(User);

            List<Models.Group> groups = await _context.Group.ToListAsync();
            MyGroups = groups.Where(g => g.UserIds.Contains(MyUser.Id)).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            MyUser = await _userManager.GetUserAsync(User);
            NewGroup.UserIds = new List<string> { MyUser.Id };

            _context.Group.Add(NewGroup);
            await _context.SaveChangesAsync();
            return RedirectToPage("./GroupPage");
        }

        public async Task<IActionResult> OnPostMessageAsync()
        {
            MyUser = await _userManager.GetUserAsync(User);
            NewMessage.Date = DateTime.Now;
            //NewMessage.GroupId = CurrentGroup.Id;
            NewMessage.UserId = MyUser.Id;
            _context.GroupMessage.Add(NewMessage);
            await _context.SaveChangesAsync();
            return RedirectToPage("./GroupPage", "OnGetAsync", new { groupId = NewMessage.GroupId });
        }

        public async Task<string> FindUsername(string userId)
        {
            return await _helpers.FindUsername(userId);
        }
    }
}
