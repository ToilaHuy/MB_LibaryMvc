using System.ComponentModel.DataAnnotations;
// using WebApplication8.Data;

namespace WebApplication8.Models
{
    public class BorrowingHistoryVm
    {
        public int BorrowingHistoryId { get; set; }
        [Display(Name = "Borrow Date")]

        public DateTime BorrowDate { get; set; }
        public int BorrowerId { set; get; }
        public BorrowerVm Borrower { set; get; }
        public double Cost { set; get; }
        public bool Status { set; get; }

        public ICollection<HistoryDetailViewModel> HistoryDetails { get; set; }
    }
    public class BorrowingHistoryVmIndex
    {
        public int BorrowingHistoryId { get; set; }
        [Display(Name = "Borrow Date")]

        public DateTime BorrowDate { get; set; }
        public string Borrower { set; get; }
        public bool Status { set; get; }
        public int Quantity { set; get; }
        public double Cost { set; get; }



    }
    public class BorrowingHistoryVmCreate
    {
        [Required]
        public IEnumerable<ItemViewModel> Items { get; set; }
        public IEnumerable<BorrowerVm> Borrowers { get; set; }
        [Required]
        public int BorrowerId { get; set; }

    }

    public class BorrowingHistoryCreate
    {
        [Required]
        public int BorrowerId { set; get; }
    }
}
