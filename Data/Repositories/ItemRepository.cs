using System.Collections.Generic;
using FarmaShop.Data.DAL;
using FarmaShop.Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FarmaShop.Data.Repositories
{
    public class ItemRepository: BaseRepository<Item>
    {
        public ItemRepository(ApplicationDbContext context) : base(context)
        {
        }
        
        public override void Update(Item item)
        {
            var categories = item.Categories as List<Category>;

            //Delete all relationships and remake them
            var queryString = "DELETE FROM CategoryItem WHERE ItemsId = " + item.Id;
            _context.Database.ExecuteSqlRaw(queryString);
                
            //Update with the new ones
            if (categories != null)
                foreach (var category in categories) {
                    var queryString2 = "INSERT INTO CategoryItem VALUES(" + category.Id + ", " + item.Id + ")";
                    _context.Database.ExecuteSqlRaw(queryString2);
                }
            
            base.Update(item);
        }
    }
}