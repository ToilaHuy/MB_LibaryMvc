using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Data
{
    public class BorrowingHistory
    {
        [Key]
        public Guid BorrowingHistoryId { get; set; }
        public DateTime BorrowDate { get; set; }
        public Guid BorrowerId { set; get; }
        public Borrower? Borrower { set; get; }
        public ICollection<HistoryDetail> HistoryDetails { get; set; }

    }
}
