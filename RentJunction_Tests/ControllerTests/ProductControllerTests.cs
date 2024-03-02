using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RentJunction_API.Business.Interface;
using RentJunction_API.Controllers;
using RentJunction_API.DataAccess;
using RentJunction_API.Models.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using RentJunction_API.Models.Enums;
using RentJunction_API.Models;
using System.Collections.Generic;
using RentJunction_API.Helper;
using System.Net;

namespace RentJunction_Tests.ControllerTests
{
    [TestClass]
    public class ProductControllerTests
    {
        private Mock<IProductBusiness> mockProductBusiness;
        private ProductController controller;

        [TestInitialize]
        public void TestInitialize()
        {
            mockProductBusiness = new Mock<IProductBusiness>();
            controller = new ProductController(mockProductBusiness.Object);
        }

        private void MockUserClaims()
        {
            var userClaims = new Claim[] { new Claim(ClaimTypes.Name, "testUser") };
            var user = new ClaimsPrincipal(new ClaimsIdentity(userClaims));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [TestMethod]
        public void GetProduct_ProductList_Returns200Ok()
        {
            mockProductBusiness.Setup(x => x.GetProducts(It.IsAny<string>(), It.IsAny<int?>())).Returns(MockData.productList.AsQueryable);

            var result = controller.GetProducts("testCity", 3) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }

        //[TestMethod]

        //public void GetProduct_InvalidCityOrCategory_ShouldThrowException()
        //{
        //    mockProductBusiness.Setup(x => x.GetProducts(It.IsAny<string>(), It.IsAny<int?>())).Throws(new Exception());

        //    Assert.ThrowsException<Exception>(() => controller.GetProducts(null, 34));

        //}

        [TestMethod]
        public void GetProduct_ThrowException_ShouldThrowHttpStatusException()
        {
            mockProductBusiness.Setup(x => x.GetProducts(It.IsAny<string>(), It.IsAny<int?>())).Throws(new Exception());
            Assert.ThrowsException<Exception>(() => controller.GetProducts("InvalidCity", 34));

        }

        [TestMethod]
        public void AddProduct_ProductAdded_Return200Ok()
        {
            mockProductBusiness.Setup(x => x.AddProduct(It.IsAny<AddProductDTO>(), It.IsAny<string>()));

            MockUserClaims();

            var result = controller.AddProduct(new AddProductDTO
            {
                Name = "testProduct",
                CategoryId = RentJunction_API.Models.Enums.ProductCategoryEnum.Property,
                City = "TesterCity",
                Description = "It is a test product",
                Rent = 100,
            }) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }

        [TestMethod]

        public void AddProduct_InvalidUser_ThrowsNullreferenceException()
        {
            mockProductBusiness.Setup(x => x.AddProduct(It.IsAny<AddProductDTO>(), It.IsAny<string>()));

            Assert.ThrowsException<NullReferenceException>(() => controller.AddProduct(new AddProductDTO
            {
                Name = "testProduct",
                CategoryId = ProductCategoryEnum.Property,
                City = "TesterCity",
                Description = "It is a test product",
                Rent = 100,
            }));

        }

        [TestMethod]

        public void AddProduct_InvalidUser_ThrowsHttpStatusException()
        {
            mockProductBusiness.Setup(x => x.AddProduct(It.IsAny<AddProductDTO>(), It.IsAny<string>())).Throws(new HttpStatusCodeException(HttpStatusCode.BadRequest));
            MockUserClaims();

            Assert.ThrowsException<HttpStatusCodeException>(() => controller.AddProduct(new AddProductDTO
            {
                Name = "testProduct",
                CategoryId = ProductCategoryEnum.Property,
                City = "TesterCity",
                Description = "It is a test product",
                Rent = 100,
            }));

        }

        [TestMethod]

        public void AddProduct_InvalidUser_ThrowsException()
        {
            mockProductBusiness.Setup(x => x.AddProduct(It.IsAny<AddProductDTO>(), It.IsAny<string>())).Throws(new Exception());
            MockUserClaims();

            Assert.ThrowsException<Exception>(() => controller.AddProduct(new AddProductDTO
            {
                Name = "testProduct",
                CategoryId = ProductCategoryEnum.Property,
                City = "TesterCity",
                Description = "It is a test product",
                Rent = 100,
            }));

        }
        [TestMethod]
        public void ViewListedProduct_AddedProductList_Retuns200Ok()
        {
            mockProductBusiness.Setup(x => x.ViewListedProducts(It.IsAny<string>())).Returns(MockData.productList.AsQueryable());

            MockUserClaims();

            var result = controller.ViewListedProducts() as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

        }

      
        
        [TestMethod]
        public void ViewListedProduct_ThrowException_ShouldThrowException()
        {
            mockProductBusiness.Setup(x => x.ViewListedProducts(It.IsAny<string>())).Throws(new Exception());
            MockUserClaims();

            Assert.ThrowsException<Exception>(() => controller.ViewListedProducts());

        }

        [TestMethod]
        public void ViewListedProduct_ThrowHttpStatusException_ShouldThrowException()
        {
            mockProductBusiness.Setup(x => x.ViewListedProducts(It.IsAny<string>())).Throws(new HttpStatusCodeException(HttpStatusCode.BadRequest));
            MockUserClaims();

            Assert.ThrowsException<HttpStatusCodeException>(() => controller.ViewListedProducts());

        }

        [TestMethod]
        public void DeleteProduct_ProductDeleted_Return200Ok()
        {
            mockProductBusiness.Setup(x => x.DeleteProduct(It.IsAny<string>(), It.IsAny<int>()));

            MockUserClaims();

            var result = controller.DeleteProduct(1);

            var rs = result as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, rs.StatusCode);
          

        }
        [TestMethod]
        public void DeleteProduct_ProductNotExist_ShouldThrowException()
        {
            mockProductBusiness.Setup(x => x.DeleteProduct(It.IsAny<string>(), It.IsAny<int>())).Throws(new Exception());

            MockUserClaims();

            Assert.ThrowsException<Exception>(() => controller.DeleteProduct(11));

        }

        [TestMethod]
        public void DeleteProduct_ThrowsException_ShouldThrowHttpStatusCodeException()
        {
            mockProductBusiness.Setup(x => x.DeleteProduct(It.IsAny<string>(), It.IsAny<int>())).Throws(new HttpStatusCodeException(HttpStatusCode.BadRequest));

            MockUserClaims();

            Assert.ThrowsException<HttpStatusCodeException>(() => controller.DeleteProduct(11));

        }

        [TestMethod]

        public void DeleteProduct_UserNotLoggedIn_ShouldThrowException()
        {
            mockProductBusiness.Setup(x => x.DeleteProduct(It.IsAny<string>(), It.IsAny<int>()));

            Assert.ThrowsException<NullReferenceException>(() => controller.DeleteProduct(11));

        }
        [TestMethod]
        public void ViewProductDetail_ProductDetail_ReturnOk()
        {
            mockProductBusiness.Setup(x => x.ViewProductDetail(It.IsAny<int>())).Returns(MockData.productList[0]);

            MockUserClaims();

            var result = controller.ViewProductDetails(1) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }

        [TestMethod]

        public void ViewProductDetail_ProductNotExist_ThrowException()
        {
            mockProductBusiness.Setup(x => x.ViewProductDetail(It.IsAny<int>())).Throws(new HttpStatusCodeException(HttpStatusCode.BadRequest));

            MockUserClaims();

            Assert.ThrowsException<HttpStatusCodeException>(() => controller.ViewProductDetails(20));

        }

      
        [TestMethod]

        public void UpdateProduct_ProductUpdated_Return200Ok()
        { 
            mockProductBusiness.Setup(x => x.UpdateProduct(It.IsAny<int>(), It.IsAny<AddProductDTO>(), It.IsAny<string>()));

            MockUserClaims();

            var result = controller.UpdateProduct(1, new AddProductDTO
            {
                Name = "testProduct",
                CategoryId = RentJunction_API.Models.Enums.ProductCategoryEnum.Property,
                City = "TesterCity",
                Description = "It is a test product",
                Rent = 100,
            }) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

        }

        [TestMethod]

        public void UpdateProduct_ExceptionThrown_ThrowsException()
        {
            mockProductBusiness.Setup(x => x.UpdateProduct(It.IsAny<int>(), It.IsAny<AddProductDTO>(), It.IsAny<string>()))
                .Throws(new Exception());
            MockUserClaims();
            Assert.ThrowsException<Exception>(() => controller.UpdateProduct(99, new AddProductDTO
            {
                Name = "testProduct",
                CategoryId = RentJunction_API.Models.Enums.ProductCategoryEnum.Property,
                City = "TesterCity",
                Description = "It is a test product",
                Rent = 100,
            }));

        }

        [TestMethod]

        public void RentProduct_ProductRentSuccess_Return201Createdk()
        {
            mockProductBusiness.Setup(x => x.RentProduct(It.IsAny<int>(), It.IsAny<RentProductDTO>(), It.IsAny<string>()));

            MockUserClaims();

            var result = controller.RentProduct(1,new RentProductDTO
            {
                StartDate = "01-01-2024",
                EndDate = "01-30-2024"
            }) as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status201Created, result.StatusCode);

        }

        [TestMethod]

        public void RentProduct_ExceptionThrown_ShouldThrowException()
        {
            mockProductBusiness.Setup(x => x.RentProduct(It.IsAny<int>(), It.IsAny<RentProductDTO>(), It.IsAny<string>()))
                .Throws(new Exception());

            MockUserClaims();


            Assert.ThrowsException<Exception>(() => controller.RentProduct(1, new RentProductDTO
            {
                StartDate = "01-01-2024",
                EndDate = "01-30-2024"
            }));
        }

        [TestMethod]

        public void RentProduct_HttpStatusExceptionThrown_ShouldThrowException()
        {
            mockProductBusiness.Setup(x => x.RentProduct(It.IsAny<int>(), It.IsAny<RentProductDTO>(), It.IsAny<string>()))
                .Throws(new HttpStatusCodeException(HttpStatusCode.BadRequest));

            MockUserClaims();


            Assert.ThrowsException<HttpStatusCodeException>(() => controller.RentProduct(1, new RentProductDTO
            {
                StartDate = "01-01-2024",
                EndDate = "01-30-2024"
            }));
        }

        [TestMethod]

        public void ExtendRentPeriod_RentPeriodExtended_Return200Ok()
        {
            mockProductBusiness.Setup(x => x.ExtendRentPeriod(It.IsAny<int>(), It.IsAny<ExtendRentDTO>(), It.IsAny<string>()));

            MockUserClaims();

            var result = controller.ExtendRentPeriod(1, new ExtendRentDTO
            {
                NewEndDate = "01-12-2024"
            }) as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status201Created, result.StatusCode);
        }

        [TestMethod]

        public void ExtendRentPeriod_ExceptionThrown_ShouldThrowExceptoin()
        {
            mockProductBusiness.Setup(x => x.ExtendRentPeriod(It.IsAny<int>(), It.IsAny<ExtendRentDTO>(), It.IsAny<string>()))
                .Throws(new Exception());

            MockUserClaims();

            Assert.ThrowsException<Exception>(() => controller.ExtendRentPeriod(1, new ExtendRentDTO
            {
                NewEndDate = "01-12-2024"
            }));
        }

        [TestMethod]

        public void ExtendRentPeriod_ThrowExceptoin_ShouldThrowHttpStatusCodeExceptoin()
        {
            mockProductBusiness.Setup(x => x.ExtendRentPeriod(It.IsAny<int>(), It.IsAny<ExtendRentDTO>(), It.IsAny<string>()))
                .Throws(new HttpStatusCodeException(HttpStatusCode.BadRequest));

            MockUserClaims();

            Assert.ThrowsException<HttpStatusCodeException>(() => controller.ExtendRentPeriod(1, new ExtendRentDTO
            {
                NewEndDate = "01-12-2024"
            }));
        }
        [TestMethod]
        public void ExtendRentPeriod_UserNotInRole_ReturnsUnauthorizedStatus()
        {
        
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            var result = controller.ExtendRentPeriod(1, new ExtendRentDTO()) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status401Unauthorized, result.StatusCode);
        }


       
    }
}
