using System;
using FarmaShop.Data.DAL;
using FarmaShop.Data.Models;
using FarmaShop.Data.Repositories;
using FarmaShop.Data.Seeds;
using FarmaShop.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FarmaShop.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddDefaultIdentity<ApplicationUser>(
                    options => options.SignIn.RequireConfirmedAccount = true
                    )
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            
            /*services.AddIdentity<ApplicationUser, IdentityRole>(
                    options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();*/
            
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();
            
            // Dependency Injection 
            // Inject needed repositories for controllers based on their specific type
            // Scoped = Same instance for same request. Different between requests.
            // Transient = New instance for every controller or whatever.
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            
            // allow the frontend to call with more HTTP methods
            //https://khalidabuhakmeh.com/more-http-methods-aspnet-core-html-forms
            //Needed to override a form method
            //@Html.HttpMethodOverride(HttpVerbs.Put) not supported in NET. Core
            app.UseHttpMethodOverride(new HttpMethodOverrideOptions
            {
                FormFieldName = 
                    HtmlHelperExtensions.HttpMethodOverrideFormName
            });
            app.UseStaticFiles();

            app.UseRouting();

            //Integrated DI, no need to register dependencies in service.
            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                
                endpoints.MapRazorPages();
            });

            var seeder = new Seeder(serviceProvider, Configuration);
            seeder.CreateRoles().Wait();
            seeder.PopulateDatabase().Wait();
        }
    }
}
