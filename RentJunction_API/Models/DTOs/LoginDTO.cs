using System.ComponentModel.DataAnnotations;

namespace RentJunction_API.Models.ViewModels
{
    public class LoginDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
