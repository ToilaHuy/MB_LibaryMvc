using System.ComponentModel.DataAnnotations;
using WebApplication8.Data;

namespace WebApplication8.Models
{
    public class HistoryDetailViewModel
    {   
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public double Cost { get; set; }
    }
}
