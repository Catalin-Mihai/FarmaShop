using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FarmaShop.Data.Models;
using FarmaShop.Data.Repositories;
using FarmaShop.Web.Models.Item;
using FarmaShop.Web.ViewModels.Item;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FarmaShop.Web.Controllers
{
    public class ItemController : Controller
    {
        private readonly IRepository<Item> _itemRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Category> _categoryRepository;

        public ItemController(IRepository<Item> itemRepository, UserManager<ApplicationUser> userManager, IRepository<Category> categoryRepository)
        {
            _itemRepository = itemRepository;
            _userManager = userManager;
            _categoryRepository = categoryRepository;
        }
        
        
        public IActionResult Index(int id)
        {
            return View(id);
        }
        
        
        async public Task<IActionResult> ByCategory(int id)
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
                        Categories = new List<Category>(),
                        InStock = -1
                    };
                    adminAddNewItem.Categories.Add(
                        await _categoryRepository.GetById(id) //To know to which category the new item belongs 
                    );
                    items?.Add(adminAddNewItem);
                }
            }
            
            var model = DataMapper.ModelMapper.ToItemsViewModel(items);
            return View(model);
        }

        /*async public Task<IActionResult> Index(int id)
        {
            Console.WriteLine(categoryId.ToString());
            
            var items = await _itemRepository.Get(
                it => it.Categories.Select(cat => cat.Id).Contains(categoryId), 
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
                        Categories = new List<Category>(),
                        InStock = -1
                    };
                    adminAddNewItem.Categories.Add(
                        await _categoryRepository.GetById(categoryId) //To know to which category the new item belongs 
                    );
                    items?.Add(adminAddNewItem);
                }
            }
            
            var model = DataMapper.ModelMapper.ToItemsViewModel(items);
            return View(model);
        }*/

        public IActionResult New(int id)
        {
            //Populate the new item category with the category id received
            var categoriesIds = new List<int> {id};
            
            var newItemModel = new ItemNewModel {
                //Serialize the categories list because we can't use complex data types in views
                CategoriesIdsSerialized = JsonConvert.SerializeObject(categoriesIds)
            };

            return View(newItemModel);
        }
        
        [HttpPost]
        async public Task<IActionResult> Create(ItemNewModel categoryModel)
        {
            if (ModelState.IsValid) {
                Console.WriteLine("Model valid!");
                var dbModel = await DataMapper.ModelMapper.ToItemDbModel(categoryModel, _categoryRepository);
                await _itemRepository.Add(dbModel);
                await _itemRepository.SaveChangesAsync();
            }
            else {
                Console.WriteLine("Model invalid!");
                return View("New", categoryModel);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}