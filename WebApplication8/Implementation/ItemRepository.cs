using Microsoft.EntityFrameworkCore;
using WebApplication8.Data;
using WebApplication8.Repositories;

namespace WebApplication8.Implementation
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(MyDbContext context) : base(context) { }

        public override async Task<bool> Remove(int id)
        {
            var exist = await _context.Items
                                .Where(x => x.ItemId == id)
                                .FirstOrDefaultAsync();
            if (exist == null) return false;

            _context.Items.Remove(exist);
            return true;
        }
        public override async Task<bool> Update(Item entity)
        {
            var exist = await _context.Items
                                .Where(x => x.ItemId == entity.ItemId)
                                .FirstOrDefaultAsync();
            if (exist == null) return false;
            exist.Title = entity.Title;
            exist.Price = entity.Price;
            exist.PublicationDate = entity.PublicationDate;
            exist.NumberOfPages = entity.NumberOfPages;
            exist.HistoryDetails = entity.HistoryDetails;
            exist.Category = entity.Category;
            exist.RunTime = entity.RunTime;
            exist.Author = entity.Author;
            // _context.Items.Update(entity);
            return true;
        }
    }
}
