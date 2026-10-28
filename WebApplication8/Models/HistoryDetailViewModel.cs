namespace WebApplication8.Models
{
    public class HistoryDetailViewModel
    {
        public int HistoryDetailId { get; set; }

        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public double Cost { get; set; }
        public bool Status { get; set; }
        public ItemViewModel Item { get; set; }
    }
}
