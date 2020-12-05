using FarmaShop.Data.DAL;
using FarmaShop.Data.Models;

namespace FarmaShop.Data.Repositories
{
    public class CategoryRepository: BaseRepository<Category>
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}