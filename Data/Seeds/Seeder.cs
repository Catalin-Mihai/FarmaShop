using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FarmaShop.Data.DAL;
using FarmaShop.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FarmaShop.Data.Seeds
{
    // Sealed == Non inheritable class
    public sealed class Seeder
    {
        private IServiceProvider ServiceProvider { get; }
        private IConfiguration Configuration { get; }

        public Seeder(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            ServiceProvider = serviceProvider;
            Configuration = configuration;
        }

        async public Task CreateRoles()
        {
            var roleManager = ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var roleName in Configuration.GetSection("Roles")
                .GetChildren().ToArray().Select(c => c.Value).ToArray()) {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist) {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        async public Task PopulateDatabase(bool forceOverride = false)
        {
            //Get the database from the DI
            var context = ServiceProvider.GetService<ApplicationDbContext>();

            context.Database.EnsureCreated(); //Create DB if it's not created
            
            if (!context.Categories.Any() || forceOverride) {
                //No categories in the database
                context.Categories.AddRange(Categories);
            }
            
            if (!context.Items.Any() || forceOverride) {
                //No categories in the database
                context.Items.AddRange(Items);
            }
            
            
        }
        
        private static readonly Category[] Categories = {
            new Category {
                Name = "Vitamine",
                Description = "Vitamine - Descriere..."
            },
            new Category {
                Name = "Suplimente alimentare",
                Description = "Suplimente alimentare - Descriere..."
            },
            new Category {
                Name = "Ingrijire personala",
                Description = "Ingrijire personala - Descriere..."
            }
        };
        
        private static Dictionary<string, Category> CategoriesDict {
            get {
                return GetCategoriesAsDict();
            }
        }
        
        private static Dictionary<string, Category> GetCategoriesAsDict()
        {
            var categories = Categories.ToDictionary(genre => genre.Name);
            return categories;
        }
        
        public static Item[] Items = {
            new Item {
                Name = "Hepatofit Forte D80",
                ImageUrl = null,
                InStock = 20,
                ShortDescription = "Short desc",
                LongDescription = "Long Desc",
                Price = 22.3
            },
            new Item {
                Name = "Item2",
                ImageUrl = null,
                InStock = 20,
                ShortDescription = "Short desc",
                LongDescription = "Long Desc",
                Price = 22.3
            }
        };
    }
}