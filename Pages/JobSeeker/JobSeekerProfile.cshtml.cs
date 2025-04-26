using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SWP391_M2_BL5_G4_SP25.Pages.JobSeeker
{
    [Authorize(Roles = "JobSeeker")]
    public class JobSeekerProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public JobSeekerProfileModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public ProfileInputModel Profile { get; set; }

        public string Message { get; set; }
        public string ErrorMessage { get; set; }

        // Input model for binding form data
        public class ProfileInputModel
        {
            [Required(ErrorMessage = "Full Name is required.")]
            [StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters.")]
            public string FullName { get; set; }

            [Required(ErrorMessage = "Email is required.")]
            [EmailAddress(ErrorMessage = "Invalid Email Address.")]
            public string Email { get; set; }

            [Phone(ErrorMessage = "Invalid Phone Number.")]
            [StringLength(15, ErrorMessage = "Phone Number cannot exceed 15 characters.")]
            public string PhoneNumber { get; set; }

            [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
            public string Address { get; set; }
        }

        // Load user profile data on GET
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ErrorMessage = "Unable to load user profile.";
                return Page();
            }

            Profile = new ProfileInputModel
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };

            return Page();
        }

        // Handle profile update on POST
        public async Task<IActionResult> OnPostUpdateProfileAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ErrorMessage = "Unable to load user profile.";
                return Page();
            }

            // Update user properties
            user.FullName = Profile.FullName;
            user.PhoneNumber = Profile.PhoneNumber;
            user.Address = Profile.Address;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                Message = "Profile updated successfully.";
            }
            else
            {
                ErrorMessage = "Error updating profile: " + string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return Page();
        }
    }

    
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
    }
}