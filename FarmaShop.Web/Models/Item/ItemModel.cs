using System.Collections.Generic;
using FarmaShop.Web.Models.Category;

namespace FarmaShop.Web.Models.Item
{
    public class ItemModel
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string ShortDescription { get; set; }
        
        public string LongDescription { get; set; }
        
        public double Price { get; set; }
        
        public string ImageUrl { get; set; }

        public int InStock { get; set; }

        public IEnumerable<CategoryMenuItemModel> Categories { get; set; }
    }
}