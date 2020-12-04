namespace FarmaShop.Data.Models
{
    // Join Table between Item table and Category table.
    // Each item has many categories and each category belongs to many items
    public class ItemCategory
    {
        public int ItemId { get; set; }
        
        public Item Item { get; set; }
        
        public int CategoryId { get; set; }
        
        public Category Category { get; set; }
    }
}