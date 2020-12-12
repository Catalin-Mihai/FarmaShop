#nullable enable
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace FarmaShop.Web.Validations
{
    public class ImageMemorySize : ValidationAttribute
    {
        private readonly int _kb;
        public ImageMemorySize(int kb)
        {
            _kb = kb;
        }

        public override bool IsValid(object? value)
        {
            var image = value as IFormFile;

            if (image?.Length > _kb * 1024) {
                return false;
            }

            return true;
        }
    }
}