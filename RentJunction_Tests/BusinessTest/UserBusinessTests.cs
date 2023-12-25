using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RentJunction_API.Business;
using RentJunction_API.DataAccess;
using RentJunction_API.DataAccess.Interface;
using RentJunction_API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RentJunction_Tests.BusinessTest
{
    [TestClass]
    public class UserBusinessTests
    {
        private Mock<IUserData> mockUserData;
        private Mock<IdentityUser> mockIdentityUserData;
        private Mock<UserManager<IdentityUser>> userManagerMock;
        private UserBusiness business;
        

        [TestInitialize]
        public void TestInitialize()
        {
            mockUserData = new Mock<IUserData>();
            mockIdentityUserData = new Mock<IdentityUser>();
            userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            business = new UserBusiness(userManagerMock.Object, mockUserData.Object);
        }

        [TestMethod]
        public void GetCustomers_FetchedCustomerList_ReturnsList()
        {
            mockUserData.Setup(x => x.GetUsers()).Returns(MockData.customersList.AsQueryable);

            var result = business.GetCustomers();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetOwners_FetchedOwnerList_ReturnsList()
        {
            mockUserData.Setup(x => x.GetUsers()).Returns(MockData.ownerList.AsQueryable);

            var result = business.GetOwners();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteUser_DeleteUserFromRecords_ReturnsTrue()
        {

            mockUserData.Setup(x => x.DeleteUser(It.IsAny<User>(), It.IsAny<IdentityUser>())).Returns(Task.FromResult(true));
            mockUserData.Setup(x => x.GetUsers()).Returns(MockData.ownerList.AsQueryable);

            userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(MockData.identityUsers[0]));

            var task = Task.Run(async () => await business.DeleteUser(1));

            task.Wait();

            var result = task.Result;

            Assert.IsTrue(result);

        }
    }
}
