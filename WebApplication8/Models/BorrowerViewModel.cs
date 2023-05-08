using System.ComponentModel.DataAnnotations;
using WebApplication8.Data;

namespace WebApplication8.Models
{
    public class BorrowerVMCreate
    {
        [Required]
        [StringLength(100)]
        public string BorrowerName { get; set; }
        [Required]
        public string Address { get; set; }
    }
    public class BorrowerVM: BorrowerVMCreate
    {
        [Key]
        public int BorrowerId { get; set; }
        public ICollection<BorrowingHistory>? BorrowingHistories { get; set; }
        
    }
}
