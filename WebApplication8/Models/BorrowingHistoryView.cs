using System.ComponentModel.DataAnnotations;
using WebApplication8.Data;

namespace WebApplication8.Models
{
    public class BorrowingHistoryView
    {
        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<Borrower> Borrowers { get; set; }
        public IEnumerable<BorrowingHistory>? BorrowingHistories { get; set; }

        [Required]
        public List<Guid> ItemId { set; get; }
        [Required]

        public Guid BorrowerId { set; get; }

    }
}
