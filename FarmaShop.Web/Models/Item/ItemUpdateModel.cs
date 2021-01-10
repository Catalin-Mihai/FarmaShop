using System;
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
        
        [StringLength(20, ErrorMessage="Numele este prea lung!")] 
        public string Name { get; set; }
        
        [StringLength(64, ErrorMessage="Descrierea este prea lunga!")] 
        public string ShortDescription { get; set; }
        
        public string LongDescription { get; set; }
        
        [Range(0, Int32.MaxValue, ErrorMessage = "Pretul nu poate fi negativ!")]
        public double Price { get; set; }
        
        public string OldImageUrl { get; set; }
        
        [ImageMemorySize(6000, ErrorMessage = "File is too large")]
        public IFormFile NewImage { get; set; }
        
        [Range(0, Int32.MaxValue, ErrorMessage = "Stocul nu poate fi negativ!")]
        public int InStock { get; set; }
        
        public string CategoriesIdsSerialized { get; set; }
    }
}