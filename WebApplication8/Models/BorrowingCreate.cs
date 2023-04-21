using Microsoft.Build.Framework;

namespace WebApplication8.Models
{
    public class BorrowingCreate
    {

        [Required]
        public List<Guid>? ItemId { set; get; }
        [Required]
        public Guid BorrowerId { set; get; }
    }
}
