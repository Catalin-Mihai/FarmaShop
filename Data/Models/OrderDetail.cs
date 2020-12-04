namespace FarmaShop.Data.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        
        public int OrderId { get; set; }
        
        public int FoodId { get; set; }
        
        public int Amount { get; set; }
        
        public double Price { get; set; }
        
        public virtual Item Item { get; set; }
        
        public virtual Order Order { get; set; }
    }
}