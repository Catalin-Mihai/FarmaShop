using System.ComponentModel.DataAnnotations;
using FarmaShop.Web.Validations;
using Microsoft.AspNetCore.Http;

namespace FarmaShop.Web.Models.Category
{
    public class CategoryNewModel
    {
        [Required]
        [StringLength(20, ErrorMessage="Numele este prea lung!")]  
        public string Name { get; set; }
        
        [Required]
        [StringLength(64, ErrorMessage="Descrierea este prea lunga!")]
        public string Description { get; set; }

        [Required]
        [ImageMemorySize(4096, ErrorMessage = "Fisierul este prea mare!")]
        public IFormFile Image { get; set; }
        
    }
}