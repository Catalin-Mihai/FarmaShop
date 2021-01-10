using System.ComponentModel.DataAnnotations;

namespace FarmaShop.Web.Models.Order
{
    public class OrderNewModel
    {
        [Required]
        //6 cifre
        [RegularExpression(@"\d{6}")]
        public string ZipCode { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage="Adresa trebuie sa aiba intre 6 si 50 caractere!")] 
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

    }
}