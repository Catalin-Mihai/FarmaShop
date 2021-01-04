using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FarmaShop.Data.Models;
using FarmaShop.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;


namespace FarmaShop.Web.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderDetail> _orderDetailsRepository;
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly IRepository<ApplicationUserInfo> _userInfoRepository;
        private readonly IRepository<ShoppingCartItem> _shoppingCartItemsRepository;

        public DeletePersonalDataModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<DeletePersonalDataModel> logger, IRepository<Order> orderRepository, IRepository<ApplicationUser> userInfoRepository, IRepository<ShoppingCartItem> shoppingCartItemsRepository, IRepository<ApplicationUserInfo> userInfoRepository1, IRepository<OrderDetail> orderDetailsRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _orderRepository = orderRepository;
            _userRepository = userInfoRepository;
            _shoppingCartItemsRepository = shoppingCartItemsRepository;
            _userInfoRepository = userInfoRepository1;
            _orderDetailsRepository = orderDetailsRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //We can retrieve the user by our custom made Repository to include the navigation proprieties.
            //We will need them later
            var user = (await _userRepository.Get(x => x.Email == User.Identity.Name,
                includeProperties: "Orders,Orders.OrderDetails,ShoppingCartItems,UserInfo")).FirstOrDefault();
            
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }
            }

            //We have to delete the related fields: Order, ShoppingCartItems and UserInfo
            //Delete cascade
            foreach(var order in user.Orders)
            {
                _orderDetailsRepository.DeleteRange(order.OrderDetails);
            }
            _orderRepository.DeleteRange(user.Orders);
            
            _shoppingCartItemsRepository.DeleteRange(user.ShoppingCartItems);
            
            if(user.UserInfo != null)
                _userInfoRepository.Delete(user.UserInfo);

            var result = await _userManager.DeleteAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }
    }
}
