using WebApplication8.Data;
using WebApplication8.Repositories;

namespace WebApplication8.Implementation
{
    public class ItemRepository : Repository<Item>, IItemRepository 
    {
        public ItemRepository(MyDbContext context) : base(context) { }
    } 
}
