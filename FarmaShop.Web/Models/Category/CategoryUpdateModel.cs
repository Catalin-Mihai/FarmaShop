using System.ComponentModel.DataAnnotations;
using FarmaShop.Web.Validations;
using Microsoft.AspNetCore.Http;

namespace FarmaShop.Web.Models.Category
{
    public class CategoryUpdateModel
    {
        public int Id { get; set; }
        
        [StringLength(20, ErrorMessage="Numele este prea lung!")]  
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string OldImageUrl { get; set; }
        
        [ImageMemorySize(4096, ErrorMessage = "Fisierul este prea mare!")]
        public IFormFile NewImage { get; set; }
    }
}