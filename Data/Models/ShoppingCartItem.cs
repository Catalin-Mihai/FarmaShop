namespace FarmaShop.Data.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }

        public int Amount { get; set; }
        
        public int ItemId { get; set; }
        
        public Item Item { get; set; }
        
        public string UserId { set; get; }
        
        public ApplicationUser User { get; set; }
    }
}
