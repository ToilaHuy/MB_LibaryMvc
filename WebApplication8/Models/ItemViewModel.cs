using System.ComponentModel.DataAnnotations;
using static WebApplication8.Data.Item;

namespace WebApplication8.Models
{
    public class ItemViewModel : ItemVmCreate
    {
        public int ItemId { get; set; }
        public string ImageUrl { get; set; }
        [Display(Name = "Available Quantity")]

        public int AvailableQuantity { get; set; }

    }
    public class ItemViewUpdateModel
    {
        public int ItemId { get; set; }
        public string ImageUrl { get; set; }
        public int AvailableQuantity { get; set; }
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
        public int Quantity { get; set; }

    }

    public class ItemVmCreate

    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Publication Date")]

        public DateTime PublicationDate { get; set; }
        [Required]
        public string Author { get; set; }
        [Range(0, int.MaxValue)]
        public double Price { get; set; }
        [Display(Name = "Number Of Pages")]

        public int? NumberOfPages { get; set; }

        public string? RunTime { get; set; }
        public CategoryName Category { get; set; }
        [Required]
        [Display(Name = "Image")]

        public IFormFile FileUpload { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The field Quantity must be bigger 0")]
        [Required]
        
        public int Quantity { get; set; }

    }
}
