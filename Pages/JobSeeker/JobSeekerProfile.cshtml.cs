using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.DTO;
using SWP391_M2_BL5_G4_SP25.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SWP391_M2_BL5_G4_SP25.Pages.JobSeeker
{
    [Authorize(Roles = "JobSeeker")]
    public class JobSeekerProfileModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly MyDBContext _dbContext;

        public JobSeekerProfileModel(UserManager<User> userManager, MyDBContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [BindProperty]
        public ProfileInputModel Profile { get; set; }

        public string Message { get; set; }
        public string ErrorMessage { get; set; }


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

        public HeaderDTO Header { get; set; }   = new HeaderDTO();

        public async Task<IActionResult> OnGetAsync()
        {
            Header.JobCategories = _dbContext.JobCategories.Where(x => x.isDelete == false).ToList();
            var user = await _userManager.GetUserAsync(User);
            
            if (user.isDelete)
            {
                return RedirectToPage("/InActiveUser");
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


}