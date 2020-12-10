using System.Collections.Generic;

namespace FarmaShop.Data.Models
{
    public class Item
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string ShortDescription { get; set; }
        
        public string LongDescription { get; set; }
        
        public double Price { get; set; }
        
        public byte[] Image { get; set; }
        
        // public bool IsPreferedFood { get; set; }
        
        public int InStock { get; set; }

        public ICollection<Category> Categories { get; set; }
    }
}