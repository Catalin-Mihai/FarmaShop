using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FarmaShop.Data.Models;
using FarmaShop.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FarmaShop.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRepository<ApplicationUserInfo> _userInfoRepository;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IRepository<ApplicationUserInfo> userInfoRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userInfoRepository = userInfoRepository;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            
            [Display(Name = "Address line 1")]
            public string AddressLine1 { get; set; }
            
            [Display(Name = "Address line 2")]
            public string AddressLine2 { get; set; }
            
            [Display(Name = "City")]
            public string City { get; set; }
            
            [Display(Name = "Country")]
            public string Country { get; set; }
            
            [Display(Name = "Image")]
            public IFormFile ImageFormFile { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            Username = userName;
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            var userInfo = (await _userInfoRepository.Get(u => u.User == user)).FirstOrDefault();

            Input = new InputModel
            {
                PhoneNumber = phoneNumber ?? "None",
                AddressLine1 = userInfo?.AddressLine1 ?? "None",
                AddressLine2 = userInfo?.AddressLine2 ?? "None",
                City = userInfo?.City ?? "None",
                Country = userInfo?.Country ?? "None"
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            
            //User info region
            var applicationUserInfo = new ApplicationUserInfo {
                AddressLine1 = Input.AddressLine1,
                AddressLine2 = Input.AddressLine2,
                Country = Input.Country,
                City = Input.City,
                User = user
            };

            if ((await _userInfoRepository.GetById(user.UserInfoId)) == null)
                await _userInfoRepository.Add(applicationUserInfo);
            else 
                _userInfoRepository.Update(applicationUserInfo);

            await _userInfoRepository.SaveChangesAsync();
            //

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
