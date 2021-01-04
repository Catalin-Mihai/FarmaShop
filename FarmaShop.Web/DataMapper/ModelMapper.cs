using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FarmaShop.Data.Models;
using FarmaShop.Data.Repositories;
using FarmaShop.Web.Models.Cart;
using FarmaShop.Web.Models.Category;
using FarmaShop.Web.Models.Item;
using FarmaShop.Web.Models.Order;
using FarmaShop.Web.ViewModels.Cart;
using FarmaShop.Web.ViewModels.Category;
using FarmaShop.Web.ViewModels.Item;
using Microsoft.AspNetCore.Http;

namespace FarmaShop.Web.DataMapper
{
    public static class ModelMapper
    {
        #region Util

        public static string ToBase64String(byte[] image)
        {
            if(image != null)
                return "data:image/png;base64, " + Convert.ToBase64String(image);
            
            return null;
        }

        public static byte[] FromBase64String(string imageUrl)
        {
            if (imageUrl != null) {
                imageUrl = imageUrl.Substring("data:image/png;base64, ".Length);
                return Convert.FromBase64String(imageUrl);
            }

            return null;
        }

        public static byte[] FromFileForm(IFormFile formFile)
        {
            if (formFile != null) {
                using (var ms = new MemoryStream())
                {
                    formFile.CopyTo(ms);
                    return ms.ToArray();
                }
            }

            return null;
        }

        #endregion
        
        #region Items
        public static ItemCategoryModel ToItemCategoryModel(Item item)
        {
            var itemModel = new ItemCategoryModel {
                Id = item.Id,
                Name = item.Name,
                ShortDescription = item.ShortDescription,
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

        
        public static ItemCartModel ToItemCartModel(Item item)
        {
            var itemModel = new ItemCartModel {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                ImageUrl = null,
                InStock = item.InStock,
            };

            if (item.Image != null)
                itemModel.ImageUrl = ToBase64String(item.Image);
            
            return itemModel;
        }
        
        
        public static Item ToItemDbModel(ItemCartModel itemCartModel)
        {
            var dbModel = new Item {
                Id = itemCartModel.Id,
                Name = itemCartModel.Name,
                Price = itemCartModel.Price,
                Image = FromBase64String(itemCartModel.ImageUrl),
                InStock = itemCartModel.InStock,
            };
            
            return dbModel;
        }
        
        
        public static ItemsCategoryViewModel ToItemsViewModel(IEnumerable<Item> items)
        {
            return new ItemsCategoryViewModel {
                Items = new List<ItemCategoryModel>(items.Select(ToItemCategoryModel))
            };
        }

        async public static Task<Item> ToItemDbModel(ItemNewModel newModel, IRepository<Category> repository = null)
        {
            var dbModel = new Item {
                Name = newModel.Name,
                Price = newModel.Price,
                Image = FromFileForm(newModel.Image),
                Categories = null,
                InStock = newModel.InStock,
                LongDescription = newModel.LongDescription,
                ShortDescription = newModel.ShortDescription
            };

            if (repository != null && newModel.CategoriesIdsSerialized != null) {

                //Get the categories from DB
                dbModel.Categories = new List<Category>();
                var categoriesIds = JsonSerializer.Deserialize<List<int>>(newModel.CategoriesIdsSerialized);
                
                if (categoriesIds != null)
                    foreach (var categoryId in categoriesIds) {
                        var category = await repository.GetById(categoryId);
                        if (category != null)
                            dbModel.Categories.Add(category); 
                    }
            }

            return dbModel;
        }
        
        public static ItemModel ToItemModel(Item item, Category fromCategory)
        {
            var itemModel = new ItemModel {
                Id = item.Id,
                Name = item.Name,
                LongDescription = item.LongDescription,
                Price = item.Price,
                ImageUrl = null,
                InStock = item.InStock,
                FromCategory = null,
                Categories = null
            };

            if (fromCategory != null) {
                itemModel.FromCategory = ToCategoryMenuItemModel(fromCategory);
            }
            
            if (item.Categories != null) {
                itemModel.Categories = new List<CategoryMenuItemModel>(
                    item.Categories.Select(ToCategoryMenuItemModel));
            }

            if (item.Image != null)
                itemModel.ImageUrl = ToBase64String(item.Image);
            
            return itemModel;
        }
        
        public static ItemUpdateModel ToItemUpdateModel(Item dbModel)
        {
            var updateModel = new ItemUpdateModel {
                Id = dbModel.Id,
                Name = dbModel.Name,
                ShortDescription = dbModel.ShortDescription,
                LongDescription = dbModel.LongDescription,
                Price = dbModel.Price,
                InStock = dbModel.InStock,
                CategoriesIdsSerialized = 
                    JsonSerializer.Serialize(dbModel.Categories),
                OldImageUrl = ToBase64String(dbModel.Image)
            };
                
            return updateModel;
        }

        async public static Task<Item> ToItemDbModel(ItemUpdateModel updateModel, IRepository<Category> repository = null)
        {
            var dbModel = new Item {
                Id = updateModel.Id,
                Name = updateModel.Name,
                Price = updateModel.Price,
                Image = FromFileForm(updateModel.NewImage),
                Categories = null,
                InStock = updateModel.InStock,
                LongDescription = updateModel.LongDescription,
                ShortDescription = updateModel.ShortDescription
            };

            if (dbModel.Image == null) {
                dbModel.Image = FromBase64String(updateModel.OldImageUrl);
            }

            if (repository != null && updateModel.CategoriesIdsSerialized != null) {

                //Get the categories from DB
                dbModel.Categories = new List<Category>();
                var categoriesIds = JsonSerializer.Deserialize<List<int>>(updateModel.CategoriesIdsSerialized);
                
                if (categoriesIds != null)
                    foreach (var categoryId in categoriesIds) {
                        var category = await repository.GetById(categoryId);
                        if (category != null)
                            dbModel.Categories.Add(category); 
                    }
            }

            return dbModel;
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
                    ImageUrl = ToBase64String(category.Image)
                };

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
                    Image = FromFileForm(newModel.Image)
                };

                return dbModel;
            }
            
            #endregion

            #region UpdateModel

            public static CategoryUpdateModel ToCategoryUpdateModel(Category dbModel)
            {
                var updateModel = new CategoryUpdateModel {
                    Id = dbModel.Id,
                    Name = dbModel.Name,
                    Description = dbModel.Description,
                    OldImageUrl = ToBase64String(dbModel.Image)
                };
                
                return updateModel;
            }
            
            public static Category ToCategoryDbModel(CategoryUpdateModel updateModel)
            {
                var dbModel = new Category {
                    Id = updateModel.Id,
                    Name = updateModel.Name,
                    Description = updateModel.Description,
                    Image = FromFileForm(updateModel.NewImage)
                };

                if(dbModel.Image == null){
                    dbModel.Image = FromBase64String(updateModel.OldImageUrl);
                }

                return dbModel;
            }

            #endregion

        #endregion

        #region CartItems

        public static CartItemModel ToCartItemModel(ShoppingCartItem shoppingCartItem)
        {
            var cartItem = new CartItemModel {
                Id = shoppingCartItem.Id,
                Amount = shoppingCartItem.Amount,
                Item = ToItemCartModel(shoppingCartItem.Item),
                User = shoppingCartItem.User
            };

            //Be aware of object items cycle
            //We already specify the cart items. No need to specify them to user too
            cartItem.User.ShoppingCartItems = null;
            
            return cartItem;
        }

        public static ShoppingCartItem FromCartItemModel(CartItemModel cartItemModel)
        {
            var cartItemDbModel = new ShoppingCartItem {
                Id = cartItemModel.Id,
                Amount = cartItemModel.Amount,
                Item = ToItemDbModel(cartItemModel.Item),
                User = cartItemModel.User
            };

            return cartItemDbModel;
        } 
        
        public static CartViewModel ToCartViewModel(IEnumerable<ShoppingCartItem> cartItemModels)
        {
            var cartViewModel = new CartViewModel {
                Items = new List<CartItemModel>(cartItemModels.Select(ToCartItemModel))
            };

            return cartViewModel;
        }


        
        #endregion

        #region Order

            public static Order ToOrderDbModel(OrderNewModel newModel)
            {
                var orderDbModel = new Order {
                    Address = newModel.Address,
                    City = newModel.City,
                    Country = newModel.Country,
                    ZipCode = newModel.ZipCode
                };

                return orderDbModel;
            }

        #endregion
    }
}