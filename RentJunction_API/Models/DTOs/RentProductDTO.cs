using System;
using System.ComponentModel.DataAnnotations;

namespace RentJunction_API.Models.ViewModels
{
    public class RentProductDTO
    {
        [Required]
        public string StartDate { get; set; }
        [Required]
        public string EndDate { get; set; }

    }
}
