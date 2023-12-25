using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RentJunction_API.Business;
using RentJunction_API.DataAccess.Interface;
using RentJunction_API.Models.Enums;
using RentJunction_API.Models.ViewModels;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RentJunction_Tests.BusinessTest
{
    [TestClass]
    public class AuthBusinessTests
    {
        private Mock<IUserData> mockUserData;
        private Mock<UserManager<IdentityUser>> userManagerMock;
        private Mock<SignInManager<IdentityUser>> signInManagerMock;
        private AuthBusiness business;

        [TestInitialize]
        public void TestInitialize()
        {
            mockUserData = new Mock<IUserData>();
            userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            signInManagerMock = new Mock<SignInManager<IdentityUser>>
                (userManagerMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(), null, null, null, null);
            business = new AuthBusiness(signInManagerMock.Object, mockUserData.Object);
        }
       
        [TestMethod]
        public void AddUserAsync_UserAdded_ReturnTrue()
        {            
            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
           
            mockUserData.Setup(x => x.CreateUserAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            var task = Task.Run(async () => await business.AddUserAsync(new RegisterDTO
            {
                UserName = "testUser",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                Password = "password123",
                FirstName = "Test",
                LastName = "User",
                City = "City",
                RoleId = RolesEnum.Customer
            }, true));

            task.Wait();
            var result = task.Result;

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Login_LoginSuccesful_ReturnTrue()
        {
            signInManagerMock.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                 .ReturnsAsync(SignInResult.Success);

            var task = Task.Run(async () => await business.Login(new LoginDTO
            {
                Username = "testUser",
                Password = "password123"

            }));

            task.Wait();

            var result = task.Result;

            Assert.IsTrue(result);
        }

        [TestMethod]

        public void Logout_LogoutSuccess_ReturnTrue()
        {
            signInManagerMock.Setup(x => x.SignOutAsync()).Returns(Task.FromResult(true));

            var task = Task.Run(async () => await business.Logout());

            task.Wait();

            var result = task.Result;

            Assert.IsTrue(result);

        }
    }
}
