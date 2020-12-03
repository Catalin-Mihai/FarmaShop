using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace FarmaShop.Data.Models
{
	public class ShoppingCart
	{
        public string Id { get; set; }
        
		public IEnumerable<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
