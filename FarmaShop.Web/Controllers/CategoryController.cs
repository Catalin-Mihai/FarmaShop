using System;
using System.Threading.Tasks;
using FarmaShop.Data.Models;
using FarmaShop.Data.Repositories;
using FarmaShop.Web.Models.Category;
using Microsoft.AspNetCore.Mvc;

namespace FarmaShop.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IActionResult Items(int categoryId)
        {
            //Is not the new category item for admins
            if (categoryId != -1) {
                Console.WriteLine("A intrat lol: " + categoryId);
                return RedirectToAction("Items", "Item", new {categoryId = categoryId});
            }
            
            //Create new Category
            Console.WriteLine("Continuare: " + categoryId);
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

        async public Task<IActionResult> Edit(int categoryId)
        {
            var categoryDbModel = await _categoryRepository.GetById(categoryId);
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
    }
}