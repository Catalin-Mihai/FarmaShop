using System.Collections.Generic;
using System.Threading.Tasks;
using FarmaShop.Data.DAL;
using FarmaShop.Data.Models;

namespace FarmaShop.Data.Repositories
{
    public class ShoppingCartItemRepository: BaseRepository<ShoppingCartItem>
    {
        public ShoppingCartItemRepository(ApplicationDbContext context) : base(context)
        {
        }
        public void RemoveRange(IEnumerable<ShoppingCartItem> cartItems)
        {
            _dbSet.RemoveRange(cartItems);
        }
    }
}