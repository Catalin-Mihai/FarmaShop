namespace FarmaShop.Data.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        
        public Item Item { get; set; }
        
        public int Amount { get; set; }
        
        public string ShoppingCartId { get; set; }
    }
}
