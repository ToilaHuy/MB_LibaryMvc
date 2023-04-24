//using Microsoft.Build.Framework;
using WebApplication8.Data;
using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Models
{
    public class BorrowingCreate
    {

        [Required]
        [MinLength(1)]
        public List<Guid> ItemId { get; set; }
        [Required]

        public Guid BorrowerId { set; get; }
    }
}
