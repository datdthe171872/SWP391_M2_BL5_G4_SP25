using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.DTO;
using SWP391_M2_BL5_G4_SP25.Models;
using SWP391_M2_BL5_G4_SP25.Service;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SWP391_M2_BL5_G4_SP25.Pages.Client
{
    [Authorize(Roles = "Client")]
    public class ProfileModel : PageModel
    {
        private readonly MyDBContext _context;
        private readonly UserManager<User> _userManager;
        private readonly UploadImg _uploadImg;

        public ProfileModel(MyDBContext context, UserManager<User> userManager, UploadImg uploadImg)
        {
            _context = context;
            _userManager = userManager;
            _uploadImg = uploadImg;
        }

        public ProfileClient Profile { get; set; } = new ProfileClient();
        [BindProperty]
        public ProfileClientInput Input { get; set; }

        [BindProperty]
        public IFormFile? UploadImage { get; set; }

        public HeaderDTO Header { get; set; } = new HeaderDTO();
        public async Task<IActionResult> OnGetAsync()
        {
            Header.JobCategories = _context.JobCategories.Where(x=>x.isDelete==false).ToList();
            var user = await _userManager.GetUserAsync(User);
            if (user.isDelete)
            {
                return RedirectToPage("/InActiveUser");
            }
            Header.User = user;
            Profile.Fullname = user.FullName;
            Profile.Phone = user.PhoneNumber;
            Profile.Email = user.Email;
            Profile.Location = user.Address;
            var profileClient = _context.ClientProfiles.FirstOrDefault(x => x.UserID == user.Id);
            if (profileClient != null)
            {
                Profile.dob = profileClient.Dob;
                Profile.Desciption = profileClient.Description;
                Profile.Img = profileClient.Logo;
                Profile.Link = profileClient.Link;
            }
            Input = new ProfileClientInput
            {
                Desciption = Profile.Desciption,
                dob = Profile.dob,
                Fullname = user.FullName,
                Link = Profile.Link,
                Location = Profile.Location,
                Phone = Profile.Phone
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var today = DateTime.Now;
            var age = today.Year - Input.dob.Year;
            if (Input.dob.Date > today.AddYears(-age)) age--;
            if (age > 65 || age < 18)
            {
                ModelState.AddModelError(string.Empty, "Date of birth must be greater than 18 and less than 65");
                return await OnGetAsync();
            }
            if (Input.Phone.Length > 11 || Input.Phone.Length <9)
            {
                ModelState.AddModelError(string.Empty, "Phone must be greater than 8 and less than 12");
                return await OnGetAsync();
            }



            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    user.FullName = Input.Fullname;
                    user.Address = Input.Location;
                    user.PhoneNumber = Input.Phone;
                }
                
                if (UploadImage != null)
                {
                    var imagePath = await _uploadImg.UploadAsync(UploadImage,"uploads/img");
                    var clientProfile = _context.ClientProfiles.FirstOrDefault(x => x.UserID == user.Id);
                    if (clientProfile != null)
                    {
                        clientProfile.Dob = Input.dob;
                        clientProfile.Logo = imagePath;
                        clientProfile.Link = Input.Link;
                        clientProfile.Description = Input.Desciption;
                        _context.ClientProfiles.Update(clientProfile);
                    }
                    else
                    {
                        clientProfile = new ClientProfile
                        {
                            UserID = user.Id,
                            Dob = Input.dob,
                            Description = Input.Desciption,
                            Logo = imagePath,
                            Link = Input.Link
                        };
                        _context.ClientProfiles.Add(clientProfile);
                    }
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError(string.Empty, "Error!This action has been rollback.");
            }
            return await OnGetAsync();
        }

    }
}
