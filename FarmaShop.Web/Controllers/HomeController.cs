using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using FarmaShop.Data.Models;
using FarmaShop.Data.Repositories;
using FarmaShop.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FarmaShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<ApplicationUser> _applicationUserrepository;
        private readonly UserManager<ApplicationUser> _userManager;

        private const int AdminRoleAdded = 1;
        private const int AlreadyAnAdmin = 2;

        public HomeController(ILogger<HomeController> logger, IRepository<Category> categoryRepository, IRepository<ApplicationUser> applicationUserrepository, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
            _applicationUserrepository = applicationUserrepository;
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
        
        [Authorize]
        public IActionResult Privacy(int state)
        {
            return View(state);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        async public Task<IActionResult> AddAdmin()
        {
            var user = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            var state = AlreadyAnAdmin;
            if (!isAdmin) {
                await _userManager.AddToRoleAsync(user, "Admin");
                state = AdminRoleAdded;
            }

            return RedirectToAction("Privacy", new {state = state});
        }
    }
}
