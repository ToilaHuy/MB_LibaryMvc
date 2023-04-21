using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Data
{
    public class HistoryDetail
    {
        [Key]

        public Guid HistoryDetailId { get; set; }
        public Guid ItemId { get; set; }
        public Guid BorrowingHistoryId { get; set; }
        public BorrowingHistory BorrowingHistory { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; }
        public double Cost { get; set; }



    }
}
