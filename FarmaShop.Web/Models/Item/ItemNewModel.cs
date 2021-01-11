using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FarmaShop.Web.Validations;
using Microsoft.AspNetCore.Http;

namespace FarmaShop.Web.Models.Item
{
    public class ItemNewModel
    {
        [Required]
        [StringLength(20, ErrorMessage="Numele este prea lung!")] 
        public string Name { get; set; }
        
        [Required]
        [StringLength(64, ErrorMessage="Descrierea este prea lunga!")] 
        public string ShortDescription { get; set; }
        
        [Required]
        [StringLength(1024, ErrorMessage="Descrierea este prea lunga!")]
        public string LongDescription { get; set; }
        
        [Required]
        [Range(0, Int32.MaxValue, ErrorMessage = "Pretul nu poate fi negativ!")]
        public double Price { get; set; }
        
        [Required]
        [ImageMemorySize(4096, ErrorMessage = "Fisierul este prea mare!")]
        public IFormFile Image { get; set; }
        
        [Required]
        [Range(0, Int32.MaxValue, ErrorMessage = "Stocul nu poate fi negativ!")]
        public int InStock { get; set; }

        [Required]
        [MinCategoriesCount(1, ErrorMessage = "Trebuie selectata cel putin o categorie!")]
        public IList<ItemNewCategoriesCheckBoxModel> Categories { set; get; }
    }
}