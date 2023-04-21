namespace WebApplication8.Repositories
{
    public interface IUnitOfWork: IDisposable
    {
        IItemRepository Item { get; }
        IBorrowerRepository Borrower { get; }
        IBorrowingHistoryRepository BorrowingHistory { get; }
        IHistoryDetailRepository HistoryDetail { get; }
        Task<int> Save();
    }
}
