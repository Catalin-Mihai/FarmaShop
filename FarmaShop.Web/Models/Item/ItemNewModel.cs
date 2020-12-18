using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FarmaShop.Web.Models.Category;
using FarmaShop.Web.Validations;
using Microsoft.AspNetCore.Http;

namespace FarmaShop.Web.Models.Item
{
    public class ItemNewModel
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string ShortDescription { get; set; }
        
        [Required]
        public string LongDescription { get; set; }
        
        [Required]
        public double Price { get; set; }
        
        [Required]
        [ImageMemorySize(1024, ErrorMessage = "File is too large")]
        public IFormFile Image { get; set; }
        
        [Required]
        public int InStock { get; set; }
        
        public string CategoriesIdsSerialized { get; set; }
    }
}