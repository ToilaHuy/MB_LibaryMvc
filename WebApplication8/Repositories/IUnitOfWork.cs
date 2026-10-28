namespace WebApplication8.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IItemRepository Item { get; }
        IBorrowerRepository Borrower { get; }
        IBorrowingHistoryRepository BorrowingHistory { get; }
        Task<int> Save();
    }
}
