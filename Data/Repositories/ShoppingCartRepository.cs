using FarmaShop.Data.DAL;
using FarmaShop.Data.Models;

namespace FarmaShop.Data.Repositories
{
    public class ShoppingCartRepository: BaseRepository<ShoppingCart>
    {
        public ShoppingCartRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}