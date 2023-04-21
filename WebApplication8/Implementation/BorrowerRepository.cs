using WebApplication8.Data;
using WebApplication8.Repositories;

namespace WebApplication8.Implementation
{
    public class BorrowerRepository : Repository<Borrower>, IBorrowerRepository
    {
        public BorrowerRepository(MyDbContext context) : base(context) { }
    }
}
