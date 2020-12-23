using FarmaShop.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FarmaShop.Data.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /*protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            // Not needed in NET 5.0.
            // Many-to-many supported by default in NET 5.0
            // Primary key is made of itemID and categoryID (composite key)
            builder.Entity<ItemCategory>()
                .HasKey(ic => new {ic.ItemId, ic.CategoryId}); 

            //Set up one to many from both sides
            //Item -> Categories
            builder.Entity<ItemCategory>()
                .HasOne(ic => ic.Item)
                .WithMany(ic => ic.ItemCategories)
                .HasForeignKey(ic => ic.ItemId);
            
            // Category -> Items
            builder.Entity<ItemCategory>()
                .HasOne(ic => ic.Category)
                .WithMany(ic => ic.ItemCategories)
                .HasForeignKey(bc => bc.CategoryId);
        }*/

        public DbSet<Item> Items { get; set; }
        
        public DbSet<Category> Categories { get; set; }
        
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        
        public DbSet<Order> Orders { get; set; }
        
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
