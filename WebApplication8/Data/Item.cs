using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication8.Data
{
    public class Item
    {
        public enum CategoryName
        {
            Book,
            DVD,
            Magazine
        }
        [Key]
        public Guid ItemId { get; set; }
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
        public ICollection<HistoryDetail>? HistoryDetails { get; set; }


      


    }
}
