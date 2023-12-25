using System.ComponentModel.DataAnnotations;

namespace RentJunction_API.Models.ViewModels
{
    public class ExtendRentDTO
    {
        [Required]
        public string NewEndDate { get; set; }
    }
}
