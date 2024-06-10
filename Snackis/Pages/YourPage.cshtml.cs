using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Snackis.Pages
{
    [Authorize]
    public class YourPageModel : PageModel
    {
        public Areas.Identity.Data.SnackisUser MyUser { get; set; }

        private UserManager<Areas.Identity.Data.SnackisUser> _userManager { get; set; }
        private readonly Snackis.Data.SnackisContext _context;

        public YourPageModel(UserManager<Areas.Identity.Data.SnackisUser> userManager, Data.SnackisContext context)
        {
            _userManager = userManager;
            _context = context;
        }



        public Areas.Identity.Data.SnackisUser PageOwner { get; set; }
        public Models.MyInfoPage YourInfoPage { get; set; }
        public List<Models.Group> MyGroups { get; set; }
        public int ShowInvite { get; set; }
        public List<int> PageOwnerInvitesGroupIds { get; set; }

        [BindProperty]
        public Models.Invite NewInvite { get; set; }

        public async Task<IActionResult> OnGet(string userId, int groupId, string message)
        {
            if (message != null)
            {
                return RedirectToPage("./MessagesPage", "OnGetAsync", new { userId = message });
            }

            MyUser = await _userManager.GetUserAsync(User);
            List<Models.Group> myUnfilteredGroups = await _context.Group.Where(g => g.UserIds[0] == MyUser.Id).ToListAsync();
            PageOwner = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            YourInfoPage = await _context.MyInfoPage.Where(m => m.UserId == userId).FirstOrDefaultAsync();
            List<Models.Invite> invites = await _context.Invite.Where(i => i.ReceiverId == userId).ToListAsync();

            PageOwnerInvitesGroupIds = new List<int>();
            foreach (var invite in invites)
            {
                PageOwnerInvitesGroupIds.Add(invite.GroupId);
            }

            MyGroups = new List<Models.Group>();
            foreach (var group in myUnfilteredGroups)
            {
                if (!PageOwnerInvitesGroupIds.Contains(group.Id))
                {
                    MyGroups.Add(group);
                }
            }

            if (groupId != 0)
            {
                ShowInvite = groupId;
            }

            return null;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string test = NewInvite.Message;
            _context.Invite.Add(NewInvite);
            await _context.SaveChangesAsync();

            return RedirectToPage("./YourPage", "OnGetAsync", new { userId = NewInvite.ReceiverId });
        }
    }
}
