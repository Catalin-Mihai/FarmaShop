using System.ComponentModel.DataAnnotations;

namespace FarmaShop.Web.Models.Order
{
    public class OrderNewModel
    {
        [Required]
        public string ZipCode { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

    }
}