using System.ComponentModel.DataAnnotations;
using WebApplication8.Data;

namespace WebApplication8.Models
{
    public class BorrowingHistoryVM
    {
        public int BorrowingHistoryId { get; set; }
        public DateTime BorrowDate { get; set; }
        public int BorrowerId { set; get; }
        public Borrower Borrower { set; get; }
        public ICollection<HistoryDetail> HistoryDetails { get; set; }
    }
    public class BorrowingHistoryVMIndex
    {
        public int BorrowingHistoryId { get; set; }
        public DateTime BorrowDate { get; set; }
        public string Borrower { set; get; }

    }
    public class BorrowingHistoryVMCreate
    {
        [Required]
        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<Borrower> Borrowers { get; set; }
        [Required]

        public int BorrowerId { get; set; }

        public List<HistoryDetailViewModel> HistoryDetailViewModel { get; set; }
        public BorrowingHistoryVMCreate() {
            HistoryDetailViewModel = new List<HistoryDetailViewModel>();
        }

    }

    public class BorrowingHistoryCreate
    {

        [Required]
        [MinLength(1)]     
        public int BorrowerId { set; get; }
        public List<HistoryDetailViewModel> HistoryDetailViewModel { get;set; }
    }
}
