using System.ComponentModel.DataAnnotations;
using WebApplication8.Data;

namespace WebApplication8.Models
{
    public class BorrowingHistoryView
    {
        [Required]
        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<Borrower> Borrowers { get; set; }
        [Required(ErrorMessage = "Please select at least one option")]
        [MinLength(1)]
        public List<Guid> ItemId { get; set; }
        public List<Guid> Quantity { get; set; }



    }
}
