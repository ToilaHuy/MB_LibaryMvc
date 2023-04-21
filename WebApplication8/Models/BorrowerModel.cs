using System.ComponentModel.DataAnnotations;
using WebApplication8.Data;

namespace WebApplication8.Models
{
    public class BorrowerModel
    {
        [Required]
        [StringLength(100)]
        public string BorrowerName { get; set; }
        [Required]
        public string Address { get; set; }

    }
}
