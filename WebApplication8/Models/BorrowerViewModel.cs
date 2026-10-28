using System.ComponentModel.DataAnnotations;
using WebApplication8.Data;

namespace WebApplication8.Models
{
    public class BorrowerVmCreate
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Name")]
        public string BorrowerName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Avatar")]

        public IFormFile FileUpload { get; set; }


    }
    public class BorrowerVm : BorrowerVmCreate
    {
        [Key]

        public int BorrowerId { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<BorrowingHistory>? BorrowingHistories { get; set; }

    }
    public class BorrowerUpdate 
    {
        public int BorrowerId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Name")]

        public string BorrowerName { get; set; }
        [Required]
        public string Address { get; set; }
        public string ImageUrl { get; set; }


    }
}
