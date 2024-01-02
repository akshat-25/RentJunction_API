using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RentJunction_API.Business;
using RentJunction_API.DataAccess;
using RentJunction_API.DataAccess.Interface;
using RentJunction_API.Models;
using RentJunction_API.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using RentJunction_API.Models.Enums;

using System;

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

        public void GetProduct_InvalidCityOrCategory_ShouldThrowException()
        {
            mockProductData.Setup(x => x.GetProducts()).Returns(MockData.productList.AsQueryable());

            Assert.ThrowsException<Exception>(() => business.GetProducts("random",99));
        }

        [TestMethod]

        public void AddProduct_ProductAddedToList_ProductAdded()
        {
            mockProductData.Setup(x => x.AddProduct(It.IsAny<Product>()));
            mockUserData.Setup(u => u.GetUsers()).Returns(MockData.ownerList.AsQueryable());
            business.AddProduct(new AddProductDTO
            {
                Name = "testProduct",
                CategoryId = ProductCategoryEnum.Property,
                City = "TesterCity",
                Description = "It is a test product",
                Rent = 100,
            }, "test1234");

            mockProductData.Verify(p => p.AddProduct(It.IsAny<Product>()),Times.Once);
        }

        [TestMethod]
        public void DeleteProduct_DeleteProducrFromList_ShouldDeleteProduct()
        {
            mockProductData.Setup(p => p.GetProducts()).Returns(new List<Product>
            {
            new Product { Id = 1, UserId = 1 }

            }.AsQueryable);

            mockUserData.Setup(u => u.GetUsers()).Returns(MockData.ownerList.AsQueryable());

            business.DeleteProduct("test1234", 1);

            mockProductData.Verify(p => p.DeleteProduct(It.IsAny<Product>()),Times.Once);  

        }

        [TestMethod]

        public void DeleteProduct_ProductDoesNotExist_ShouldThrowException()
        {
            var products = new List<Product>();
            mockProductData.Setup(p => p.GetProducts()).Returns(products.AsQueryable());

            mockUserData.Setup(u => u.GetUsers()).Returns(MockData.ownerList.AsQueryable());

            Assert.ThrowsException<Exception>(() => business.DeleteProduct("test1234", 7));

        }

        [TestMethod]
        public void UpdateProduct_ProductUpdatedSuccess_ShouldUpdateProduct()
        {
            mockProductData.Setup(x => x.GetProducts()).Returns(MockData.productList.AsQueryable());

            mockUserData.Setup(x => x.GetUsers()).Returns(MockData.ownerList.AsQueryable());

            business.UpdateProduct(1, new AddProductDTO
            {
                Name = "testProduct",
                CategoryId = ProductCategoryEnum.Property,
                City = "TesterCity",
                Description = "It is a test product",
                Rent = 100,
            }, "test1234");

            var updatedProduct = MockData.productList.FirstOrDefault(p => p.Id == 1);
            Assert.AreEqual("testProduct", updatedProduct.Name);
            Assert.AreEqual("TesterCity", updatedProduct.City);
            Assert.AreEqual("It is a test product", updatedProduct.Description);
            Assert.AreEqual(100,updatedProduct.Rent);
            
        }

        [TestMethod]

        public void UpdateProduct_ProductIdUnavailable_ShouldThrowException()
        {
            mockProductData.Setup(x => x.GetProducts()).Returns(MockData.productList.AsQueryable());

            mockUserData.Setup(x => x.GetUsers()).Returns(MockData.ownerList.AsQueryable());

            Assert.ThrowsException<Exception>(() => business.UpdateProduct(111, new AddProductDTO
            {
                Name = "testProduct",
                CategoryId = ProductCategoryEnum.Property,
                City = "TesterCity",
                Description = "It is a test product",
                Rent = 100,
            }, "test1234"));
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

        public void RentProduct_RentSuccess_ShouldRentAProduct()
        {
            mockUserData.Setup(x => x.GetUsers()).Returns(MockData.customersList.AsQueryable());

            mockProductData.Setup(p => p.GetProducts()).Returns(MockData.productList.AsQueryable);

            mockRentalData.Setup(r => r.AddRental(It.IsAny<Rental>()));

            business.RentProduct(1, new RentProductDTO
            {
                StartDate = DateTime.Now.AddDays(1).ToString(),
                EndDate = DateTime.Now.AddDays(5).ToString(),
            }, "test1234");

           mockRentalData.Verify(r => r.AddRental(It.IsAny<Rental>()),Times.Once());

        }

        [TestMethod]
        public void RentProduct_InvalidDate_ShouldThrowException()
        {
            mockUserData.Setup(x => x.GetUsers()).Returns(MockData.customersList.AsQueryable());

            mockProductData.Setup(p => p.GetProducts()).Returns(MockData.productList.AsQueryable);

            mockRentalData.Setup(r => r.AddRental(It.IsAny<Rental>()));

            Assert.ThrowsException<Exception>(() => business.RentProduct(1, new RentProductDTO
            {
                StartDate = "Invalid Start Date",
                EndDate = "Invalid End Date",
            },"test1234"));
        }
        [TestMethod]

        public void ExtendRentPeriod_RentExtendSuccess_ReturnTrue()
        {
            mockUserData.Setup(x => x.GetUsers()).Returns(MockData.customersList.AsQueryable());

            mockProductData.Setup(p => p.GetProducts()).Returns(MockData.productList.AsQueryable);

            mockRentalData.Setup(r => r.GetRentalData()).Returns(MockData.rentalData.AsQueryable);

            business.ExtendRentPeriod(1, new ExtendRentDTO
            {
                NewEndDate = DateTime.Now.Date.AddDays(10).ToString(),
            }, "test1234");

            var updatedRentalProduct = MockData.rentalData.FirstOrDefault(p => p.ProductId == 1);
            Assert.AreEqual(DateTime.Now.Date.AddDays(10).ToString(), updatedRentalProduct.EndDate);
        }

        [TestMethod]
        public void ExtendRentPeriod_InvalidNewEndDate_ShouldThrowException()
        {
            mockUserData.Setup(x => x.GetUsers()).Returns(MockData.customersList.AsQueryable());

            mockProductData.Setup(p => p.GetProducts()).Returns(MockData.productList.AsQueryable);

            mockRentalData.Setup(r => r.GetRentalData()).Returns(MockData.rentalData.AsQueryable);

            Assert.ThrowsException<Exception>(() => business.ExtendRentPeriod(1, new ExtendRentDTO
            {
                NewEndDate = "Invalid Date"
            }, "test1234")); ;
        }

        [TestMethod]
        public void ViewListedProduct_FetchListOfProducts_ReturnsList()
        {
            mockProductData.Setup(p => p.GetProducts()).Returns(MockData.productList.AsQueryable());
            mockUserData.Setup(u => u.GetUsers()).Returns(MockData.ownerList.AsQueryable());

            var result = business.ViewListedProducts(MockData.ownerList[0].UserName);
            Assert.IsNotNull(result);

        }

        [TestMethod]
        public void ViewListedProduct_UsernameUnavailable_ShouldThrowException()
        {
          
            mockProductData.Setup(p => p.GetProducts()).Returns(MockData.productList.AsQueryable());
            mockUserData.Setup(u => u.GetUsers()).Returns(MockData.ownerList.AsQueryable());

            Assert.ThrowsException<Exception>(() => business.ViewListedProducts("Username"));
        }
    
    }
}
