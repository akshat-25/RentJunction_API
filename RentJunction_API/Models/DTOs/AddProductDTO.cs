using RentJunction_API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace RentJunction_API.Models.ViewModels
{
    public class AddProductDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public float Rent { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        public ProductCategoryEnum CategoryId { get; set; }
    }
}
