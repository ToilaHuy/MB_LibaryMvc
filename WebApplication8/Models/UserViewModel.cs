using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Models
{
    public class UserViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
