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
        private readonly ItemRepository _itemRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<ShoppingCartItem> _shoppingCartRepository;
        private readonly IRepository<OrderDetail> _orderDetailRepository;
        private readonly IRepository<Order> _orderRepository;

        public ItemController(ItemRepository itemRepository, UserManager<ApplicationUser> userManager, IRepository<Category> categoryRepository, IRepository<ShoppingCartItem> shoppingCartRepository, IRepository<OrderDetail> orderDetailRepository, IRepository<Order> orderRepository)
        {
            _itemRepository = itemRepository;
            _userManager = userManager;
            _categoryRepository = categoryRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _orderDetailRepository = orderDetailRepository;
            _orderRepository = orderRepository;
        }

        async public Task<IActionResult> Index(int id, int fromCategory)
        {
            var fromCategoryEntity = new Category {
                Id = -1
            };
            
            if (fromCategory != 0) {
                fromCategoryEntity = await _categoryRepository.GetById(fromCategory);
            }

            var queryRes = await _itemRepository.Get(x => x.Id == id, includeProperties: "Categories");
            var itemEntity = queryRes.FirstOrDefault();
        
            var itemModel = DataMapper.ModelMapper.ToItemModel(itemEntity, fromCategoryEntity);
            return View(itemModel);
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

        async public Task<IActionResult> New(int id)
        {
            var allCategories = (await _categoryRepository.GetAll()).ToList();

            var newItemModel = DataMapper.ModelMapper.ToItemNewModel(allCategories);

            for (int i = 0; i < newItemModel.Categories.Count; i++) {
                if (newItemModel.Categories[i].Id == id) {
                    newItemModel.Categories[i].Checked = true;
                }
            }

            return View(newItemModel);
        }
        
        [HttpPost]
        async public Task<IActionResult> Create(ItemNewModel itemModel)
        {
            if (ModelState.IsValid) {
                Console.WriteLine("Model valid!");
                var dbModel = await DataMapper.ModelMapper.ToItemDbModel(itemModel, _categoryRepository);
                await _itemRepository.Add(dbModel);
                await _itemRepository.SaveChangesAsync();
            }
            else {
                Console.WriteLine("Model invalid!");
                return View("New", itemModel);
            }

            return RedirectToAction("Index", "Home");
        }
        
        
        async public Task<IActionResult> Edit(int id)
        {
            var allCategories = (await _categoryRepository.GetAll()).ToList();
            
            var itemDbModel = (await _itemRepository.Get(x=>x.Id == id, includeProperties:"Categories")).FirstOrDefault();
            var itemEditModel = DataMapper.ModelMapper.ToItemUpdateModel(itemDbModel, allCategories);
            
            // _itemRepository.DetachEntity(itemDbModel);
            
            Console.WriteLine("A plecat cu id: " + itemEditModel.Id);
            return View(itemEditModel);
        }
        
        [HttpPut]
        async public Task<IActionResult> Update(ItemUpdateModel categoryUpdateModel)
        {
            if (ModelState.IsValid) {
                Console.WriteLine("Model valid!");
                var dbModel = await DataMapper.ModelMapper.ToItemDbModel(categoryUpdateModel, _categoryRepository);
                
                //To delete the many to many relationships between an item and a category
                //the easiest way is to delete the item and recreate it

                // var categories = (await _categoryRepository.Get(
                //     x => dbModel.Categories.Select(y => y.Id).Contains(x.Id), 
                //     includeProperties:"Items")).ToList();

                // var categories = dbModel.Categories;
                //
                // foreach (var category in categories) {
                //     if (category.Items.Select(x => x.Id).Contains(dbModel.Id) == false) {
                //         var itemList = category.Items as HashSet<Item>;
                //         itemList?.Add(dbModel);
                //         category.Items = itemList;
                //         _categoryRepository.Update(category);
                //     }
                // }
                // _itemRepository.DetachEntity();
                _itemRepository.Update(dbModel);
                // _itemRepository.Update(dbModel);
                
                await _itemRepository.SaveChangesAsync();
            }
            else {
                Console.WriteLine("Model invalid!");
                return View("Edit", categoryUpdateModel);
            }

            return RedirectToAction("Index", "Item", new {id = categoryUpdateModel.Id});
        }
        
        [HttpDelete]
        async public Task<IActionResult> Delete(int id)
        {
            Console.WriteLine("A intrat pe delete");
            Console.WriteLine("Id: " + id);
            var itemDb = await _itemRepository.GetById(id);

            if (itemDb != null) {
                //Delete in cascade
                
                //Shopping cart
                var cartItems = (await _shoppingCartRepository.Get(x => x.ItemId == itemDb.Id)).ToList();
                _shoppingCartRepository.DeleteRange(cartItems);
                
                //Orders
                var ordersDetails = (await _orderDetailRepository.Get(x => x.Id == itemDb.Id)).ToList();
                foreach (var orderDetail in ordersDetails) {
                    //Update the price of the order
                    var order = orderDetail.Order;
                    
                    order.OrderTotal -= orderDetail.Amount * orderDetail.Price;
                    
                    _orderRepository.Update(order);
                    
                    //delete order detail
                    _orderDetailRepository.Delete(orderDetail);
                }

                //Now delete the item
                _itemRepository.Delete(itemDb);
                
                await _itemRepository.SaveChangesAsync();
                return Ok();
            }

            return NotFound("Obiectul nu a putut fi gasit in baza de date!");
        }
    }
}