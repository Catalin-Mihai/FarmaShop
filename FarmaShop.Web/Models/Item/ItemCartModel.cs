using System.Collections.Generic;
using FarmaShop.Web.Models.Category;

namespace FarmaShop.Web.Models.Item
{
    public class ItemCartModel
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public double Price { get; set; }
        
        public string ImageUrl { get; set; }

        public int InStock { get; set; }
        
    }
}