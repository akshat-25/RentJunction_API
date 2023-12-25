using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentJunction_API.Data;
using RentJunction_API.DataAccess;
using RentJunction_API.Models;
using System.Linq;

namespace RentJunction_Tests.DataAccessTest
{
    [TestClass]
    public class RentalDataTests
    {
        private DbContextOptions<AppDbContext> options;
        private AppDbContext dbContext;
        private RentalData RentalDb;


        [TestInitialize]
        public void TestInitialize()
        {
            options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "InMemoryDatabase").Options;
            dbContext = new AppDbContext(options);
            RentalDb = new RentalData(dbContext);

           
        }

        [TestMethod]

        public void GetRentalData() 
        {
            dbContext.Rentals.Add(new Rental
            {
                Id = 1,
                StartDate = "12-23-2023",
                EndDate = "12-25-2023",
                Price = 1024,
                ProductId = 1,
                UserId = 1,
            });

            dbContext.SaveChanges();
            var rentals = dbContext.Rentals.ToList();

            Assert.IsNotNull(rentals);
            Assert.AreEqual(2, rentals.Count);

        }

        [TestMethod]

        public void AddRental()
        {
           
            var rental = new Rental
            {
                Id = 2,
                StartDate = "12-23-2023",
                EndDate = "12-25-2023",
                Price = 1024,
                ProductId = 11,
                UserId = 2,
            };
            
            dbContext.Rentals.Add(rental);
            dbContext.SaveChanges();
            Assert.AreEqual(1, dbContext.Rentals.Count());

        }


    }
}