using FarmaShop.Data.Models;
using FarmaShop.Web.Models.Item;

namespace FarmaShop.Web.Models.Cart
{
    public class CartItemModel
    {
        public int Id { get; set; }
        
        public ItemCartModel Item { get; set; }
        
        public int Amount { get; set; }
        
        public ApplicationUser User { get; set; }
    }
}