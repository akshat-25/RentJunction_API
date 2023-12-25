using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RentJunction_API.Data;
using RentJunction_API.DataAccess;
using RentJunction_API.Models;
using System.Linq;

namespace RentJunction_Tests.DataAccessTest
{
    [TestClass]
    public class ProductDataTests
    {
        private Mock<AppDbContext> mockDbContext;
        private ProductsData productDb;
        private DbContextOptions<AppDbContext> options;
        private AppDbContext dbContext;

        [TestInitialize]

        public void TestInitialize()
        {
           mockDbContext = new Mock<AppDbContext>();
           options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "InMemoryDatabase").Options;
           dbContext = new AppDbContext(options);

           

            productDb = new ProductsData(dbContext);
        }
        [TestMethod]
        public void GetProducts()
        {
            dbContext.Products.Add(new Product
            {
                Name = "testProduct",
                CategoryId = RentJunction_API.Models.Enums.ProductCategoryEnum.Property,
                City = "TesterCity",
                Description = "It is a test product",
                Rent = 100,
            });
            dbContext.SaveChanges();

            var products = productDb.GetProducts();

            Assert.IsNotNull(products);  
            Assert.AreEqual(1, products.Count());
            
        }

        [TestMethod]
        public void AddProducts() {

            Product product2 = new Product
            {
                Name = "testProduct2",
                CategoryId = RentJunction_API.Models.Enums.ProductCategoryEnum.Property,
                City = "TesterCity2",
                Description = "It is a test product2",
                Rent = 200,
            };

            productDb.AddProduct(product2);

            Assert.AreEqual(1, productDb.GetProducts().Count());
        }

        [TestMethod]
        public void DeleteProduct() 
        {
            var products = productDb.GetProducts().ToList();
            var product = products[0];
            productDb.DeleteProduct(product);
            productDb.SaveChanges();

            Assert.AreEqual(0, productDb.GetProducts().Count());
        }


    }
    
}
