using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FarmaShop.Data.Seeds
{
    public sealed class Seeder
    {
        private IServiceProvider ServiceProvider { get; }
        public IConfiguration Configuration { get; }

        public Seeder(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            ServiceProvider = serviceProvider;
            Configuration = configuration;
        }

        async public Task CreateRoles()
        {
            var roleManager = ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var roleName in Configuration.GetSection("Roles")
                .GetChildren().ToArray().Select(c => c.Value).ToArray())
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        async public static Task PopulateCategories()
        {
            
        }
    }
}