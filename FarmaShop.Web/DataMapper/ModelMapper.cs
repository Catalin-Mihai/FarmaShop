using System;
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
        public const string DefaultItemImage = "favicon.ico";
        public static ItemModel ToItemModel(Item item)
        {
            var itemModel = new ItemModel {
                Id = item.Id,
                Name = item.Name,
                ShortDescription = item.ShortDescription,
                LongDescription = item.LongDescription,
                Price = item.Price,
                ImageUrl = null,
                InStock = item.InStock,
                Categories = new List<CategoryModel>(
                    item.Categories.Select(ToCategoryModel))
            };
            if (item.Image != null)
                itemModel.ImageUrl = Convert.ToBase64String(item.Image);
            else
                itemModel.ImageUrl = DefaultItemImage;

            return itemModel;
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