using Microsoft.AspNetCore.Identity;
using RentJunction_API.Models;
using RentJunction_API.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace RentJunction_API.DataAccess
{
    public static class MockData
    {
        public static List<Product> productList = new List<Product>()
        {
            new Product{
                Id = 1,
                Name = "test1",
                City = "testCity1",
                CategoryId = Models.Enums.ProductCategoryEnum.Computer_Accessories,
                Description = "test1 product adding",
                Rent = 100,
                UserId = 1,
            },
            new Product{
                Id = 2,
                Name = "test2",
                City = "testCity2",
                CategoryId = Models.Enums.ProductCategoryEnum.Computer_Accessories,
                Description = "test2 product adding",
                Rent = 200,
                UserId = 1,
            },
            new Product{
                Id = 3,
                Name = "test3",
                City = "testCity3",
                CategoryId = Models.Enums.ProductCategoryEnum.Computer_Accessories,
                Description = "test3 product adding",
                Rent = 100,
                UserId = 1,
            }
        };

        public static List<User> ownerList = new List<User>()
        {
            new User { 
                Id = 1,
                FirstName = "test1",
                LastName = "abcd",
                City = "testcity1",
                Email = "test1@gmail.com",
                PhoneNumber = "1234567890",
                RoleId = Models.Enums.RolesEnum.Owner,
                UserName = "test1234",
            }
        };
        
        public static List<User> customersList = new List<User>()
        {
            new User { 
                Id = 1,
                FirstName = "test1",
                LastName = "abcd",
                City = "testcity1",
                Email = "test1@gmail.com",
                PhoneNumber = "1234567890",
                RoleId = Models.Enums.RolesEnum.Customer,
                UserName = "test1234",
            }
        };

        public static List<Rental> rentalData = new List<Rental>()
        {
            new Rental
            {
                Id = 1,
                StartDate = "12-23-2023",
                EndDate = "12-25-2023",
                Price = 1024,
                ProductId = 1,
                UserId = 1,
            }
        };

        public static List<IdentityUser> identityUsers = new List<IdentityUser>()
        {

            new IdentityUser
            {
                Id = "a3jksfhjalfh",
                UserName = "testUser",
                Email = "test@gmail.com",
                PhoneNumber = "1234567890"
            }
        };
    }
    
}
