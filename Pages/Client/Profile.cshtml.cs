using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_M2_BL5_G4_SP25.DTO;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages.Client
{
    [Authorize(Roles = "Client")]
    public class ProfileModel : PageModel
    {
        private readonly MyDBContext _context;
        private readonly UserManager<User> _userManager;

        public ProfileModel(MyDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public ProfileClient Profile {  get; set; }

        [BindProperty]
        public ProfileClientInput Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            Profile = new ProfileClient
            {
                Fullname = user.FullName,
                Phone = user.PhoneNumber,
                Email = user.Email
            };
            var profileClient = _context.ClientProfiles.FirstOrDefault(x => x.UserID == user.Id);
            if (profileClient != null)
            {
                Profile.dob = profileClient.Dob;
                Profile.Desciption = profileClient.Description;
                Profile.Img = profileClient.Logo;
                Profile.Link = profileClient.Link;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            return Page();
        }

    }
}
