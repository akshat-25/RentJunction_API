using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RentJunction_API.Models
{  
    public class Rental
    {
        public int Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public float Price { get; set; }
        [ForeignKey("User")]
        public int? UserId { get; set; }
        [ForeignKey("Product")]
        public int? ProductId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual Product Product { get; set; }
    }
}