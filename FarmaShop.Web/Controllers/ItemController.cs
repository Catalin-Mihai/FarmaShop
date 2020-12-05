using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FarmaShop.Data.Models;
using FarmaShop.Data.Repositories;
using FarmaShop.Web.ViewModels.Item;
using Microsoft.AspNetCore.Mvc;

namespace FarmaShop.Web.Controllers
{
    public class ItemController : Controller
    {
        private readonly IRepository<Item> _itemRepository;

        public ItemController(IRepository<Item> itemRepository)
        {
            _itemRepository = itemRepository;
        }

        async public Task<IActionResult> Items(int categoryId)
        {
            Console.WriteLine(categoryId.ToString());
            
            var items = await _itemRepository.Get(
                it => it.Categories.Select(cat => cat.Id).Contains(categoryId), 
                includeProperties: "Categories");
            var model = DataMapper.ModelMapper.ToCategoryItemsViewModel(items);
            return View(model);
        }
    }
}