﻿using System;
using System.Collections.Generic;
 using System.Data.SqlTypes;
 using System.IO;
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

            if (context == null) return;

            if (forceOverride) {
                context.Database.EnsureDeleted();
            }
            context.Database.EnsureCreated(); //Create DB if it's not created

            Task task1 = null;
            Task task2 = null;
            
            if (!context.Categories.Any() || forceOverride) {
                //No categories in the database
                task1 = context.Categories.AddRangeAsync(Categories);
            }
            
            if (!context.Items.Any() || forceOverride) {
                //No items in the database
                task2 = context.Items.AddRangeAsync(Items);
            }

            if (task1 != null) await task1;
            if (task2 != null) await task2;
            await context.SaveChangesAsync();
        }
        
        private static readonly Category[] Categories = {
            new Category {
                Name = "Laptopuri",
                Image = ReadFile("..\\imgs\\category\\laptopuri.png"),
                Description = "Aici gasiti laptopuri"
            },
            new Category {
                Name = "Televizoare",
                Image = ReadFile("..\\imgs\\category\\televizoare.png"),
                Description = "Aici gasiti laptopuri televizoare! Wow!"
            },
            new Category {
                Name = "Electronice",
                Image = ReadFile("..\\imgs\\category\\electronice.png"),
                Description = "Aici gasiti tot ce functioneaza pe curent! Tare, nu?"
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

        public static byte[] ReadFile(string filePath)
        {
            // Load file meta data with FileInfo
            var fileInfo = new FileInfo(filePath);

            // The byte[] to save the data in
            var data = new byte[fileInfo.Length];

            // Load a filestream and put its content into the byte[]
            using (var fs = fileInfo.OpenRead())
            {
                fs.Read(data, 0, data.Length);
            }

            // Delete the temporary file
            // fileInfo.Delete();

            return data;
        }
        
        public static Item[] Items = {
            
            //Laptopuri
            new Item {
                Name = "Laptop Vechi",
                InStock = 0,
                Image = ReadFile("..\\imgs\\laptop\\5.jpg"),
                ShortDescription = "Merge windows 95 pe el de n-ai treaba!",
                LongDescription = "Mai exista asa ceva?",
                Price = 5.0,
                Categories = new List<Category>
                {
                    CategoriesDict["Laptopuri"],
                    CategoriesDict["Electronice"]
                }
            },
            new Item {
                Name = "Laptop Lenovo XYZ123",
                InStock = 20,
                Image = ReadFile("..\\imgs\\laptop\\1.jpg"),
                ShortDescription = "Laptop bun",
                LongDescription = "Laptop foooooaaarttteeeee buuuunnn",
                Price = 2559.59,
                Categories = new List<Category>
                {
                    CategoriesDict["Laptopuri"],
                    CategoriesDict["Electronice"]
                }
            },
            new Item {
                Name = "Laptop HP ABC345",
                Image = ReadFile("..\\imgs\\laptop\\2.png"),
                InStock = 20,
                ShortDescription = "Laptop bunicel... E verde!",
                LongDescription = "Nu e el mare smecherie dar isi face treaba",
                Price = 1599.34,
                Categories = new List<Category>
                {
                    CategoriesDict["Laptopuri"],
                    CategoriesDict["Electronice"]
                }
            },
            new Item {
                Name = "Laptop de jucarie",
                Image = ReadFile("..\\imgs\\laptop\\4.jpg"),
                InStock = 2,
                ShortDescription = "Face sunete cand apesi pe taste",
                LongDescription = "Daca nu ai copii pentru ca sa-l cumperi inseamna ca ai o problema!",
                Price = 50.9,
                Categories = new List<Category>
                {
                    CategoriesDict["Laptopuri"]
                }
            },
            new Item {
                Name = "Televizor Samsung TV123",
                Image = ReadFile("..\\imgs\\televizoare\\2.jpg"),
                InStock = 5,
                ShortDescription = "TV ffff scump",
                LongDescription = "Face cartofi prajiti",
                Price = 1890.60,
                Categories = new List<Category>
                {
                    CategoriesDict["Televizoare"],
                    CategoriesDict["Electronice"]
                }
            },
            new Item {
                Name = "Televizor placat cu aur",
                Image = ReadFile("..\\imgs\\televizoare\\1.webp"),
                InStock = 1,
                ShortDescription = "Unul numai unul",
                LongDescription = "Nu mai e altul ca el",
                Price = 9999.34,
                Categories = new List<Category>
                {
                    CategoriesDict["Televizoare"],
                    CategoriesDict["Electronice"]
                }
            },
            new Item {
                Name = "Televizor fake din lemn",
                Image =  ReadFile("..\\imgs\\televizoare\\4.jpg"),
                InStock = 3,
                ShortDescription = "Nu este electrocasnic!",
                LongDescription = "Nu am idee de ce sunt aici!",
                Price = 2,
                Categories = new List<Category>
                {
                    CategoriesDict["Televizoare"]
                }
            }
            
        };
    }
}