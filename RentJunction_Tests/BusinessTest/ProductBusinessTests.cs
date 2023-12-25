using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RentJunction_API.Business;
using RentJunction_API.DataAccess;
using RentJunction_API.DataAccess.Interface;
using RentJunction_API.Models;
using RentJunction_API.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace RentJunction_Tests.BusinessTest
{
    [TestClass]
    public class ProductBusinessTests
    {
        private Mock<IProductsData> mockProductData;
        private Mock<IUserData> mockUserData;
        private Mock<IRentalData> mockRentalData;
        private ProductBusiness business;
        

        [TestInitialize]

        public void TestInitialize()
        {
            mockProductData = new Mock<IProductsData>();
            mockUserData = new Mock<IUserData>();
            mockRentalData = new Mock<IRentalData>();
            business = new ProductBusiness(mockProductData.Object,mockUserData.Object,mockRentalData.Object);
        }
        
        [TestMethod]
        public void GetProduct_ProductListFetched_ReturnsList()
        {
            mockProductData.Setup(x => x.GetProducts()).Returns(MockData.productList.AsQueryable());

            var result = business.GetProducts("testCity1",3);

            Assert.IsNotNull(result);

            Assert.AreEqual(1, result.Count());

        }

        [TestMethod]

        public void AddProduct_ProductAddedToList_ReturnTrue()
        {

            mockProductData.Setup(x => x.AddProduct(It.IsAny<Product>()));
            mockUserData.Setup(u => u.GetUsers()).Returns(MockData.ownerList.AsQueryable());

            var result = business.AddProduct(new AddProductDTO
            {
                Name = "testProduct",
                CategoryId = RentJunction_API.Models.Enums.ProductCategoryEnum.Property,
                City = "TesterCity",
                Description = "It is a test product",
                Rent = 100,
            }, "test1234");

            Assert.IsTrue(result);
              
        }

        [TestMethod]
        public void DeleteProduct_DeleteProducrFromList_ReturnTrue()
        {
            mockProductData.Setup(p => p.GetProducts()).Returns(new List<Product>
            {
            new Product { Id = 1, UserId = 1 }

            }.AsQueryable);
            mockUserData.Setup(u => u.GetUsers()).Returns(MockData.ownerList.AsQueryable());

            var result = business.DeleteProduct("test1234", 1);

            Assert.IsTrue(result);

        }

        [TestMethod]
        public void UpdateProduct_ProductUpdatedSuccess_ReturnTrue()
        {
            mockProductData.Setup(x => x.GetProducts()).Returns(MockData.productList.AsQueryable());

            mockUserData.Setup(x => x.GetUsers()).Returns(MockData.ownerList.AsQueryable());

            var result = business.UpdateProduct(1, new AddProductDTO
            {
                Name = "testProduct",
                CategoryId = RentJunction_API.Models.Enums.ProductCategoryEnum.Property,
                City = "TesterCity",
                Description = "It is a test product",
                Rent = 100,
            }, "test1234");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ViewProductDetail_FetchAProduct_ReturnProduct()
        {
            mockProductData.Setup(p => p.GetProducts()).Returns(MockData.productList.AsQueryable);

            var result = business.ViewProductDetail(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(MockData.productList[0], result);
        }

        [TestMethod]

        public void RentProduct_RentSuccess_ReturnTrue()
        {
            mockUserData.Setup(x => x.GetUsers()).Returns(MockData.customersList.AsQueryable());

            mockProductData.Setup(p => p.GetProducts()).Returns(MockData.productList.AsQueryable);

            mockRentalData.Setup(r => r.AddRental(It.IsAny<Rental>()));

            var result = business.RentProduct(1, new RentProductDTO
            {
                StartDate = "12-23-2023",
                EndDate = "12-30-2023"
            }, "test1234");

            Assert.IsTrue(result);

        }

        [TestMethod]

        public void ExtendRentPeriod_RentExtendSuccess_ReturnTrue()
        {
            mockUserData.Setup(x => x.GetUsers()).Returns(MockData.customersList.AsQueryable());

            mockProductData.Setup(p => p.GetProducts()).Returns(MockData.productList.AsQueryable);

            mockRentalData.Setup(r => r.GetRentalData()).Returns(MockData.rentalData.AsQueryable);

            var result = business.ExtendRentPeriod(1, new ExtendRentDTO
            {
                NewEndDate = "01-15-2024"
            }, "test1234");

            Assert.IsTrue(result);
        }

    }
}
