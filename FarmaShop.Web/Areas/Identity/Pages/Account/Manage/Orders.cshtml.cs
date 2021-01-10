using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Linq;
using System.Threading.Tasks;
using FarmaShop.Data.Models;
using FarmaShop.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace FarmaShop.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class OrdersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Order> _orderRepository;

        public OrdersModel(
            UserManager<ApplicationUser> userManager,
            IRepository<Order> orderRepository)
        {
            _userManager = userManager;
            _orderRepository = orderRepository;
        }

        public string Username { get; set; }
        
        public IEnumerable<Order> Orders { set; get; }

        private async Task LoadAsync(ApplicationUser user)
        {
            Orders = await _orderRepository.Get(order => order.User == user, includeProperties:"OrderDetails,OrderDetails.Item");
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
        
    }
}
