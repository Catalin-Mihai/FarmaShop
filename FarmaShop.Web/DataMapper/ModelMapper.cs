using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FarmaShop.Data.Models;
using FarmaShop.Web.Models.Category;
using FarmaShop.Web.Models.Item;
using FarmaShop.Web.ViewModels.Item;

namespace FarmaShop.Web.DataMapper
{
    public static class ModelMapper
    {
        public static ItemModel ToItemModel(Item item)
        {
            return new ItemModel {
                Id = item.Id,
                Name = item.Name,
                ShortDescription = item.ShortDescription,
                LongDescription = item.LongDescription,
                Price = item.Price,
                ImageUrl = item.ImageUrl,
                InStock = item.InStock,
                Categories = new List<CategoryModel>(
                    item.Categories.Select(ToCategoryModel))
            };
        }

        public static CategoryModel ToCategoryModel(Category category)
        {
            return new CategoryModel {
                Id = category.Id,
                Name = category.Name
            };
        }

        public static ItemsViewModel ToCategoryItemsViewModel(IEnumerable<Item> items)
        {
            return new ItemsViewModel {
                Items = new List<ItemModel>(items.Select(ToItemModel))
            };
        }
    }
}