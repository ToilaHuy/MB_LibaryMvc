using System.ComponentModel.DataAnnotations;
using static WebApplication8.Data.Item;

namespace WebApplication8.Models
{
    public class ItemModel
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        [Required]
        public DateTime PublicationDate { get; set; }
        [Required]
        public string Author { get; set; }
        [Range(0, int.MaxValue)]
        public double Price { get; set; }
        public int? NumberOfPages { get; set; }
        public string? RunTime { get; set; }
        public CategoryName Category { get; set; }
    }
}
