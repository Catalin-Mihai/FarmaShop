using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmaShop.Data.Models;
using FarmaShop.Data.Repositories;
using FarmaShop.Web.Models.Item;
using FarmaShop.Web.Models.Order;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FarmaShop.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IRepository<Item> _itemRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<ShoppingCartItem> _shoppingCartItemRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderDetail> _orderDetailRepository;

        public OrderController(IRepository<Item> itemRepository, UserManager<ApplicationUser> userManager, IRepository<ShoppingCartItem> shoppingCartItemRepository, IRepository<Order> orderRepository, IRepository<OrderDetail> orderDetailRepository)
        {
            _itemRepository = itemRepository;
            _userManager = userManager;
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        // GET
        public IActionResult New()
        {
            return View();
        }
        
        [HttpPost]
        async public Task<IActionResult> Create(OrderNewModel newModel)
        {
            if (ModelState.IsValid) {
                Console.WriteLine("Model valid!");
                var order = DataMapper.ModelMapper.ToOrderDbModel(newModel);

                var user = await _userManager.GetUserAsync(User);
                
                //add user to the order
                order.User = user;

                //The user cart items can be retrieved from db.
                var userCartItems = 
                    await _shoppingCartItemRepository.Get(it => it.User == user, 
                        includeProperties: "Item");

                var shoppingCartItems = userCartItems.ToList();
            
                if (shoppingCartItems.Count < 0) {
                    // The item is already in the cart
                    return View("OrderPlaced", "Ceva nu a mers cum trebuie! Nu au fost gasite produse in cos la plasarea comenzii!");
                }
            
                //update the total price
                order.OrderTotal = shoppingCartItems.Sum(it => it.Item.Price * it.Amount);
                
                //Add a new order entity in db
                await _orderRepository.Add(order);

                //Create an array of order details.
                var orderDetails = new List<OrderDetail>(shoppingCartItems.Count);
                foreach (var item in shoppingCartItems)
                {
                    orderDetails.Add(
                        new OrderDetail
                        {
                            Order = order,
                            Item = item.Item,
                            Amount = Math.Min(item.Amount, item.Item.InStock), //in case something went wrong. Don't underflow
                            Price = item.Item.Price,
                        });
                    //Update the new stock amount
                    item.Item.InStock = Math.Max(item.Item.InStock - item.Amount, 0);
                    _itemRepository.Update(item.Item);
                }
                
                //Remove user cart items;
                _shoppingCartItemRepository.DeleteRange(shoppingCartItems);
                
                await _orderDetailRepository.AddRange(orderDetails);
                await _orderDetailRepository.SaveChangesAsync();
            
                return View("OrderPlaced", "Comanda plasata cu succes!");
                
            }

            //Bad model
            Console.WriteLine("Model invalid!");
            return View("New", newModel);

        }
    }
}