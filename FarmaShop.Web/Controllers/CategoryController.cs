using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FarmaShop.Data.Models;
using FarmaShop.Data.Repositories;
using FarmaShop.Web.Models.Category;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FarmaShop.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoryController(IRepository<Category> categoryRepository, UserManager<ApplicationUser> userManager)
        {
            _categoryRepository = categoryRepository;
            _userManager = userManager;
        }
        
        async public Task<IActionResult> Index()
        {
            //Send categories

            var categories = await _categoryRepository.GetAll() as List<Category>;

            var user = await _userManager.GetUserAsync(User);
            if (user != null) {
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                if (isAdmin) {
                    Console.WriteLine("Este admin - home controller index!");
                    var adminAddNewCategory = new Category {
                        Id = -1,
                        Name = "Adauga o categorie noua!"
                    };
                    categories?.Add(adminAddNewCategory);
                }
            }
            
            var model = DataMapper.ModelMapper.ToCategoryHomeItemsViewModel(categories);
            return View(model);
        }
        
        public IActionResult Items(int id)
        {
            //Is not the new category item for admins
            if (id != -1) {
                Console.WriteLine("A intrat lol: " + id);
                return RedirectToAction("Index", "Item", new {id = id});
            }
            
            //Create new Category
            Console.WriteLine("Continuare: " + id);
            return RedirectToAction("New");
        }

        public IActionResult New()
        {
            var categoryNewModel = new CategoryNewModel();
            return View(categoryNewModel);
        }

        [HttpPost]
        async public Task<IActionResult> Create(CategoryNewModel category)
        {
            Console.WriteLine("AM intrat pe create");
            Console.WriteLine("Name: " + category.Name);
            Console.WriteLine("Descriere: " + category.Description);
            Console.WriteLine("Image bytes: " + category.Image.Length);
            if (ModelState.IsValid) {
                Console.WriteLine("Model valid!");
                var dbModel = DataMapper.ModelMapper.ToCategoryDbModel(category);
                await _categoryRepository.Add(dbModel);
                await _categoryRepository.SaveChangesAsync();
            }
            else {
                Console.WriteLine("Model invalid!");
                return View("New", category);
            }

            return RedirectToAction("Index", "Home");
        }

        async public Task<IActionResult> Edit(int id)
        {
            var categoryDbModel = await _categoryRepository.GetById(id);
            var categoryEditModel = DataMapper.ModelMapper.ToCategoryUpdateModel(categoryDbModel);
            Console.WriteLine("A plecat cu id: " + categoryEditModel.Id);
            return View(categoryEditModel);
        }
        
        [HttpPut]
        async public Task<IActionResult> Update(CategoryUpdateModel categoryUpdateModel)
        {
            Console.WriteLine("AM intrat pe update");
            Console.WriteLine("Id: " + categoryUpdateModel.Id);
            Console.WriteLine("Name: " + categoryUpdateModel.Name);
            Console.WriteLine("Descriere: " + categoryUpdateModel.Description);
            Console.WriteLine("Image bytes: " + categoryUpdateModel.NewImage?.Length);
            if (ModelState.IsValid) {
                Console.WriteLine("Model valid!");
                var dbModel = DataMapper.ModelMapper.ToCategoryDbModel(categoryUpdateModel);
                _categoryRepository.Update(dbModel);
                await _categoryRepository.SaveChangesAsync();
            }
            else {
                Console.WriteLine("Model invalid!");
                return View("Edit", categoryUpdateModel);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpDelete]
        async public Task<IActionResult> Delete(int id)
        {
            Console.WriteLine("A intrat pe delete");
            Console.WriteLine("Id: " + id);
            var categoryDb = await _categoryRepository.GetById(id);

            if (categoryDb != null) {
                _categoryRepository.Delete(categoryDb);
                await _categoryRepository.SaveChangesAsync();
                return Ok();
            }

            return NotFound();
        }
    }
}