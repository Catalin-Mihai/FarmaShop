using FarmaShop.Data.DAL;
using FarmaShop.Data.Models;

namespace FarmaShop.Data.Repositories
{
    public class OrderRepository: BaseRepository<Order>
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}