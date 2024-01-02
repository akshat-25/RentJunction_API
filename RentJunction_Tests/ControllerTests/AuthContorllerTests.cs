using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RentJunction_API.Business.Interface;
using RentJunction_API.Controllers;
using RentJunction_API.Helper;
using RentJunction_API.Models.Enums;
using RentJunction_API.Models.ViewModels;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
     
namespace RentJunction_Tests.ControllerTests
{   
    [TestClass]
    public class AuthContorllerTests
    {
        private Mock<IAuthBusiness> mockAuthBusiness;
        private AuthController controller;

        [TestInitialize]
        public void TestInitialize()
        {
            mockAuthBusiness = new Mock<IAuthBusiness>();
            controller = new AuthController(mockAuthBusiness.Object);
        }
        private void MockUserClaim()
        {
            var userClaims = new Claim[] { new Claim(ClaimTypes.Role, "Admin") };
            var user = new ClaimsPrincipal(new ClaimsIdentity(userClaims));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = user }

            };
        }


        [TestMethod]
        public void Register_ValidModel_Returns201Created()
        {
            mockAuthBusiness.Setup(x => x.AddUserAsync(It.IsAny<RegisterDTO>(), It.IsAny<bool>())).Returns(Task.FromResult(true));


            var task = Task.Run(async () => await controller.Register(new RegisterDTO()
            {
                FirstName = "test",
                LastName = "User",
                Email = "testUser@gmail.com",
                City = "testCity",
                Password = "testpassword",
                PhoneNumber = "1234567890",
                RoleId = RolesEnum.Admin,
                UserName = "testuser1"
            }));

            task.Wait();

            var result = task.Result;

            var okResult = (StatusCodeResult)result;

            Assert.IsNotNull(result);
          
            Assert.AreEqual(StatusCodes.Status201Created,okResult.StatusCode);
        }

        [TestMethod]
        public async Task Register_InternalServerError_ShouldThrowException()
        {
            mockAuthBusiness.Setup(x => x.AddUserAsync(It.IsAny<RegisterDTO>(), It.IsAny<bool>())).Throws(new Exception());

            MockUserClaim();

            await Assert.ThrowsExceptionAsync<Exception>(() => controller.Register(new RegisterDTO()
            {
                FirstName = "test",
                LastName = "User",
                Email = "testUser@gmail.com",
                City = "testCity",
                Password = "testpassword",
                PhoneNumber = "1234567890",
                RoleId = RolesEnum.Admin,
                UserName = "testuser1"
            }));

        }
        [TestMethod]
        public void Login_LoginSucces_Return200Ok()
        {
            mockAuthBusiness.Setup(x => x.Login(It.IsAny<LoginDTO>()));

            var task = Task.Run(async () => await controller.Login(new LoginDTO()
            {
                Username = "test",
                Password = "password",
            }));

            task.Wait();

            mockAuthBusiness.Verify(p => p.Login(It.IsAny<LoginDTO>()), Times.Once());
            
        }

        [TestMethod]
        public void Logout_LogoutSuccess_Return200Ok()
        {
            mockAuthBusiness.Setup(x => x.Logout()).Returns(Task.FromResult(true));

            var task = Task.Run(async () => await controller.Logout());
            task.Wait();

            var result = task.Result;

            var okResult = (OkResult)result;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK,okResult.StatusCode);
        }

    }
} 