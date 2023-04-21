using Microsoft.EntityFrameworkCore;

namespace WebApplication8.Data
{
    public class MyDbContext : DbContext
    {

  
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }
        #region MyDbSet
        public DbSet<Item> Items { get; set; }
        public DbSet<Borrower> Borrowers { get; set; }
        public DbSet<BorrowingHistory> BorrowingHistories { get; set; }

        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<HistoryDetail>(entity =>
            {
                entity.HasOne(b => b.BorrowingHistory)
                    .WithMany(b => b.HistoryDetails)
                    .HasForeignKey(b => b.BorrowingHistoryId);
                entity.HasOne(b => b.Item)
                    .WithMany(b => b.HistoryDetails)
                    .HasForeignKey(b => b.ItemId);
            });
            modelBuilder.Entity<BorrowingHistory>()
                .HasOne(b => b.Borrower)
                .WithMany(b => b.BorrowingHistories)
                .HasForeignKey(b => b.BorrowerId);
        }
        


      
    }
}
