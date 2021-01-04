using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using FarmaShop.Data.DAL;
using FarmaShop.Data.Models;

namespace FarmaShop.Data.Repositories
{
    public class OrderDetailRepository: BaseRepository<OrderDetail>
    {
        public OrderDetailRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task AddRange(IEnumerable<OrderDetail> orderDetails)
        {
            return _dbSet.AddRangeAsync(orderDetails);
        }
    }
}