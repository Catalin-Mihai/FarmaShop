using FarmaShop.Data.DAL;
using FarmaShop.Data.Models;

namespace FarmaShop.Data.Repositories
{
    public class ItemRepository: BaseRepository<Item>
    {
        public ItemRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}