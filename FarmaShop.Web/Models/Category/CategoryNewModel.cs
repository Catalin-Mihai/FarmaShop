using System.ComponentModel.DataAnnotations;
using FarmaShop.Web.Validations;
using Microsoft.AspNetCore.Http;

namespace FarmaShop.Web.Models.Category
{
    public class CategoryNewModel
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }

        [Required]
        [ImageMemorySize(1024, ErrorMessage = "File is too large")]
        public IFormFile Image { get; set; }
        
    }
}