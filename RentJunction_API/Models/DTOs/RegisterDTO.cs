using RentJunction_API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace RentJunction_API.Models.ViewModels
{
    public class RegisterDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        //[RegularExpression(@"[2-3]")]
        public RolesEnum RoleId { get; set; }


    }
}
