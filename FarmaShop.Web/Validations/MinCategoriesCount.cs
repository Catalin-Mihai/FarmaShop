#nullable enable
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FarmaShop.Web.Models.Item;
using Microsoft.AspNetCore.Http;

namespace FarmaShop.Web.Validations
{
    public class MinCategoriesCount : ValidationAttribute
    {
        private readonly int _min;
        public MinCategoriesCount(int min)
        {
            _min = min;
        }

        public override bool IsValid(object? value)
        {
            var categoriesList = value as List<ItemNewCategoriesCheckBoxModel>;
            if (categoriesList == null) return false;
            
            if (categoriesList.Select(x => x.Checked).Count() < _min) {
                return false;
            }

            return true;
        }
    }
}