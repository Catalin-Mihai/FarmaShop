using Microsoft.AspNetCore.Http;

namespace FarmaShop.Web.Models.Category
{
    public class CategoryUpdateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string OldImageUrl { get; set; }
        
        public IFormFile NewImage { get; set; }
    }
}