using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Data
{
    public class HistoryDetail
    {
        [Key]

        public int HistoryDetailId { get; set; }
        public int ItemId { get; set; }
        public int BorrowingHistoryId { get; set; }
        public BorrowingHistory BorrowingHistory { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; }
        public double Cost { get; set; }
        public bool Status { get; set; } = true;




    }
}
