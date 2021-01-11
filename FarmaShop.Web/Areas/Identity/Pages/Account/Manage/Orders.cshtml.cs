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
        private readonly IRepository<OrderDetail> _orderDetailRepository;
        private readonly IRepository<Item> _itemRepository;

        public OrdersModel(
            UserManager<ApplicationUser> userManager,
            IRepository<Order> orderRepository, IRepository<OrderDetail> orderDetailRepository, IRepository<Item> itemRepository)
        {
            _userManager = userManager;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _itemRepository = itemRepository;
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

        async public Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            Console.WriteLine("DELETE COMANDA ID: " + id);

            await LoadAsync(user);
            //Load data
            
            var orders = Orders as List<Order>;
            var deleteOrder = orders?.Find(o => o.Id == id);

            if (deleteOrder != null) {
                //Remake the stock of products
                foreach (var orderDetail in deleteOrder.OrderDetails) {
                    var item = orderDetail.Item;
                    item.InStock += orderDetail.Amount;
                    _itemRepository.Update(item);
                }
                //Delete all order details associated with the order
                _orderDetailRepository.DeleteRange(deleteOrder.OrderDetails);
                _orderRepository.Delete(deleteOrder);
                await _orderRepository.SaveChangesAsync();
            }
            
            //Reload data
            await LoadAsync(user);
            return Page();
        }
        
    }
}
