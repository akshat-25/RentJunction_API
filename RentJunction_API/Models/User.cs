using Microsoft.AspNetCore.Identity;
using RentJunction_API.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace RentJunction_API.Models
{
    [ExcludeFromCodeCoverage]
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }       
        public string LastName { get; set; }      
        public string UserName { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }     
        public string Email { get; set; }
        public RolesEnum RoleId { get; set; }

        [ForeignKey("IdentityUser")]
        public string? UserId { get; set; }

        [JsonIgnore]
        public IdentityUser IdentityUser { get; set; }
    }
}
