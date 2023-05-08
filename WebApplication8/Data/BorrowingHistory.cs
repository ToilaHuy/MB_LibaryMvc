using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Data
{
    public class BorrowingHistory
    {
        [Key]
        public int BorrowingHistoryId { get; set; }
        public DateTime BorrowDate { get; set; }
        public int BorrowerId { set; get; }
        public Borrower Borrower { set; get; }
        public ICollection<HistoryDetail> HistoryDetails { get; set; }
        // public bool Status { get; set; } = true;

    }
}
