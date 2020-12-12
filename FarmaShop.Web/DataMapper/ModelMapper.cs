using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using FarmaShop.Data.Models;
using FarmaShop.Web.Models.Category;
using FarmaShop.Web.Models.Item;
using FarmaShop.Web.ViewModels.Category;
using FarmaShop.Web.ViewModels.Item;

namespace FarmaShop.Web.DataMapper
{
    public static class ModelMapper
    {
        #region Util

        public static string ToBase64String(byte[] image)
        {
            return "data:image/png;base64, " + Convert.ToBase64String(image);
        }

        #endregion
        
        #region Items
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
                Categories = null
            };
            
            if (item.Categories != null) {
                itemModel.Categories = new List<CategoryMenuItemModel>(
                    item.Categories.Select(ToCategoryMenuItemModel));
            }

            if (item.Image != null)
                itemModel.ImageUrl = ToBase64String(item.Image);
            
            return itemModel;
        }

        public static ItemsViewModel ToItemsViewModel(IEnumerable<Item> items)
        {
            return new ItemsViewModel {
                Items = new List<ItemModel>(items.Select(ToItemModel))
            };
        }
        
        #endregion

        #region Categories

            #region MenuItem

            


            public static CategoryMenuItemModel ToCategoryMenuItemModel(Category category)
            {
                var categoryMenuItemModel = new CategoryMenuItemModel {
                    Id = category.Id,
                    Name = category.Name,
                };

                return categoryMenuItemModel;
            }
            
            public static CategoryMenuItemsViewModel ToCategoryMenuItemsViewModel(IEnumerable<Category> categories)
            {
                return new CategoryMenuItemsViewModel {
                    Categories = new List<CategoryMenuItemModel>(categories.Select(ToCategoryMenuItemModel))
                };
            }
            
            #endregion


            #region HomeItem
            
            public static CategoryHomeItemModel ToCategoryHomeItemModel(Category category)
            {
                var categoryHomeItemModel = new CategoryHomeItemModel {
                    Id = category.Id,
                    Name = category.Name,
                    ImageUrl = null
                };

                if (category.Image != null) {
                    categoryHomeItemModel.ImageUrl = ToBase64String(category.Image);
                }
                
                return categoryHomeItemModel;
            }
            
            public static CategoryHomeItemsViewModel ToCategoryHomeItemsViewModel(IEnumerable<Category> categories)
            {
                return new CategoryHomeItemsViewModel {
                    Categories = new List<CategoryHomeItemModel>(categories.Select(ToCategoryHomeItemModel))
                };
            }
            
            #endregion

            #region NewModel

            public static Category ToCategoryDbModel(CategoryNewModel newModel)
            {
                var dbModel = new Category {
                    Name = newModel.Name,
                    Description = newModel.Description,
                };
                
                using (var ms = new MemoryStream())
                {
                    newModel.Image.CopyTo(ms);
                    dbModel.Image = ms.ToArray();
                }

                return dbModel;
            }

            #endregion

        #endregion
    }
}