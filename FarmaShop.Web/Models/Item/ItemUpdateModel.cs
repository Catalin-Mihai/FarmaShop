using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FarmaShop.Web.Models.Category;
using FarmaShop.Web.Validations;
using Microsoft.AspNetCore.Http;

namespace FarmaShop.Web.Models.Item
{
    public class ItemUpdateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public string ShortDescription { get; set; }
        
        public string LongDescription { get; set; }
        
        public double Price { get; set; }
        
        public string OldImageUrl { get; set; }
        
        [ImageMemorySize(1024, ErrorMessage = "File is too large")]
        public IFormFile NewImage { get; set; }
        
        public int InStock { get; set; }
        
        public string CategoriesIdsSerialized { get; set; }
    }
}