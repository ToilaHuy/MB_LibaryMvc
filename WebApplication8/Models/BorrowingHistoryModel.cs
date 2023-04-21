using static WebApplication8.Data.Item;
using System.ComponentModel.DataAnnotations;
using WebApplication8.Data;

namespace WebApplication8.Models
{
    public class BorrowingHistoryModel
    {
        public Guid BorrowingHistoryId { get; set; }
        public DateTime BorrowDate { get; set; }
        public Guid BorrowerId { set; get; }
        public Borrower? Borrower { set; get; }
        public ICollection<HistoryDetail> HistoryDetails { get; set; }
    }
}
