using RentJunction_API.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace RentJunction_API.Models
{
    [ExcludeFromCodeCoverage]
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Rent { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public ProductCategoryEnum CategoryId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
