﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmaShop.Data.Models;
using FarmaShop.Data.Repositories;
using FarmaShop.Web.Models.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FarmaShop.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IRepository<Item> _itemRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<ShoppingCartItem> _shoppingCartItemRepository;

        public CartController(UserManager<ApplicationUser> userManager, IRepository<ShoppingCartItem> shoppingCartItemRepository, IRepository<Item> itemRepository)
        {
            _userManager = userManager;
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _itemRepository = itemRepository;
        }

        // GET
        [Authorize]
        async public Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            // Get all user cart items
            Console.WriteLine(user.Id);
            
            var userCartItems = await _shoppingCartItemRepository.Get(
                ci => ci.User == user,
                includeProperties: "Item,User");

            var cartViewModel = DataMapper.ModelMapper.ToCartViewModel(userCartItems);
            
            return View(cartViewModel);
        }

        [Authorize]
        async public Task Add(int id)
        {
            Console.WriteLine("id: " + id);
            
            var user = await _userManager.GetUserAsync(User);
            
            var item = await _itemRepository.GetById(id);

            if (item == null)
                return;

            var userCartItems = 
                await _shoppingCartItemRepository.Get(it => it.User == user, 
                    includeProperties: "Item");
            
            if (userCartItems != null && userCartItems.Select(ci => ci.Item.Id).Contains(id)) {
                // The item is already in the cart
                return;
            }
            
            var cartItem = new ShoppingCartItem {
                Amount = 1,
                Item = item,
                User = await _userManager.GetUserAsync(User)
            };

            await _shoppingCartItemRepository.Add(cartItem);
            await _shoppingCartItemRepository.SaveChangesAsync();
        }

        [HttpPut]
        [Authorize]
        async public Task<IActionResult> Update(CartItemModel updateCartItem)
        {
            Console.WriteLine("updateCartItem: " + updateCartItem.Id);

            //Security concerns
            //Do we trust client side built update model???
            var updateCartItemDbModel = DataMapper.ModelMapper.FromCartItemModel(updateCartItem);
            

            // Check if it exists in the database and it belongs to user
            if (updateCartItemDbModel == null)
                return BadRequest();
            

            _shoppingCartItemRepository.Update(updateCartItemDbModel);
            await _shoppingCartItemRepository.SaveChangesAsync();
            
            // Return the new price calculated on the server
            // As we don't trust the client side calculations.
            return Ok(updateCartItemDbModel.Amount * updateCartItemDbModel.Item.Price);
        }
        
    }
}