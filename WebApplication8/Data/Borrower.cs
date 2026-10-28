using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Data
{
    public class Borrower
    {
        [Key]
        public int BorrowerId { get; set; }
        [Required]
        [StringLength(50)]
        public string BorrowerName { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<BorrowingHistory>? BorrowingHistories { get; set; }
    }
}
