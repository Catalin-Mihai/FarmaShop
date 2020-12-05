using FarmaShop.Data.DAL;
using FarmaShop.Data.Models;

namespace FarmaShop.Data.Repositories
{
    public class ShoppingCartItemRepository: BaseRepository<ShoppingCartItem>
    {
        public ShoppingCartItemRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}