using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FarmaShop.Data.Models;
using FarmaShop.Data.Repositories;
using FarmaShop.Web.ViewModels.Item;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FarmaShop.Web.Controllers
{
    public class ItemController : Controller
    {
        private readonly IRepository<Item> _itemRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ItemController(IRepository<Item> itemRepository, UserManager<ApplicationUser> userManager)
        {
            _itemRepository = itemRepository;
            _userManager = userManager;
        }

        // [HttpGet("{categoryId}")]
        async public Task<IActionResult> Index(int id)
        {
            Console.WriteLine(id.ToString());
            
            var items = await _itemRepository.Get(
                it => it.Categories.Select(cat => cat.Id).Contains(id), 
                includeProperties: "Categories") as List<Item>;

            var user = await _userManager.GetUserAsync(User);
            if (user != null) {
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                if (isAdmin) {
                    Console.WriteLine("Este admin!");
                    var adminAddNewItem = new Item {
                        Id = -1,
                        Name = "Adauga un produs nou!",
                        ShortDescription = "Aceasta optiune este disponibila doar administratorilor.",
                        Price = 0,
                        InStock = -1
                    };
                    items?.Add(adminAddNewItem);
                }
            }
            
            var model = DataMapper.ModelMapper.ToItemsViewModel(items);
            return View(model);
        }
    }
}