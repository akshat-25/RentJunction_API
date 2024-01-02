using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RentJunction_API.Business.Interface;
using RentJunction_API.Controllers;
using RentJunction_API.DataAccess;
using RentJunction_API.Models.ViewModels;
using System.Linq;
using System.Security.Claims;

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
        public void ViewListedProduct_AddedProductList_Retuns200Ok()
        {
            mockProductBusiness.Setup(x => x.ViewListedProducts(It.IsAny<string>())).Returns(MockData.productList.AsQueryable());

            MockUserClaims();

            var result = controller.ViewListedProducts() as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

        }

        [TestMethod]
        public void DeleteProduct_ProductDeleted_Return200Ok()
        {
            mockProductBusiness.Setup(x => x.DeleteProduct(It.IsAny<string>(), It.IsAny<int>()));

            MockUserClaims();

            var result = controller.DeleteProduct(1) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

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
    }
}
