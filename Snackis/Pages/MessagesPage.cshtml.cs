using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Snackis.Pages
{
    [Authorize]
    public class MessagesPageModel : PageModel
    {
        public Areas.Identity.Data.SnackisUser MyUser { get; set; }

        private UserManager<Areas.Identity.Data.SnackisUser> _userManager { get; set; }
        private readonly Snackis.Data.SnackisContext _context;
        private readonly Services.Helpers _helpers;

        public MessagesPageModel(UserManager<Areas.Identity.Data.SnackisUser> userManager, Data.SnackisContext context, Services.Helpers helpers)
        {
            _userManager = userManager;
            _context = context;
            _helpers = helpers;
        }

        public List<Models.DM> MyDMs { get; set; }
        public List<string> MyCorrespondents { get; set; }
        public List<string> MyCorrespondentsUsernames { get; set; }

        [BindProperty]
        public string CorrespondentId { get; set; }
        
        public List<Models.DM> CurrentDMs { get; set; }

        [BindProperty]
        public Models.DM NewDM { get; set; }

        public async Task OnGetAsync(string userId)
        {
            MyUser = await _userManager.GetUserAsync(User);
            List<Models.DM> dms = await _context.DM.ToListAsync();
            MyDMs = dms.Where(d => d.UserIds.Contains(MyUser.Id)).ToList();
            MyCorrespondents = new List<string>();
            MyCorrespondentsUsernames = new List<string>();

            foreach (var dm in MyDMs)
            {
                foreach (var id in dm.UserIds)
                {
                    if (!MyCorrespondents.Contains(id) && id != MyUser.Id)
                    {
                        MyCorrespondents.Add(id);
                    }
                }
            }

            foreach (var id in MyCorrespondents)
            {
                MyCorrespondentsUsernames.Add(await FindUsername(id));
            }

            if (userId != null)
            {
                CorrespondentId = userId;
                CurrentDMs = new List<Models.DM>();
                foreach (var dm in MyDMs)
                {
                    foreach (var id in dm.UserIds)
                    {
                        if (id == CorrespondentId)
                        {
                            CurrentDMs.Add(dm);
                            break;
                        }
                    }
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            MyUser = await _userManager.GetUserAsync(User);
            NewDM.Date = DateTime.Now;
            NewDM.UserIds = new List<string>()
            {
                MyUser.Id,
                CorrespondentId
            };

            _context.DM.Add(NewDM);
            await _context.SaveChangesAsync();
            return RedirectToPage("./MessagesPage", "OnGetAsync", new { userId = CorrespondentId });
        }

        public async Task<string> FindUsername(string userId)
        {
            return await _helpers.FindUsername(userId);
        }
    }
}
