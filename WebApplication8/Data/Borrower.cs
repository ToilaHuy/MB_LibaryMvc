using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Data
{
    public class Borrower
    {
        [Key]   
        public Guid BorrowerId { get; set; }
        [Required]
        [StringLength(50)]
        public string BorrowerName { get; set; }
        public string Address { get; set; }
        public ICollection<BorrowingHistory>? BorrowingHistories { get; set; }
    }
}
