using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RentJunction_API.Business;
using RentJunction_API.DataAccess;
using RentJunction_API.DataAccess.Interface;
using RentJunction_API.Models;
using System;
using System.Collections.Generic;
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

            var result = business.GetCustomers( );

            Assert.IsNotNull(result);
        }
        
        [TestMethod]
        public void GetCustomers_ExceptionThrown()
        {
            mockUserData.Setup(x => x.GetUsers()).Returns(new List<User>().AsQueryable);
            
            Assert.ThrowsException<Exception>(() => business.GetCustomers());
        }

       
        [TestMethod]
        public void GetOwners_FetchedOwnerList_ReturnsList()
        {
            mockUserData.Setup(x => x.GetUsers()).Returns(MockData.ownerList.AsQueryable);

            var result = business.GetOwners();

            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void GetOwners_ExceptionThrown()
        {
            mockUserData.Setup(x => x.GetUsers()).Returns(new List<User>().AsQueryable);

            Assert.ThrowsException<Exception>(() => business.GetOwners());
        }
        [TestMethod]

        public void DeleteUser_ValidId_ShouldDeleteUser()
        {
            mockUserData.Setup(x => x.DeleteUser(It.IsAny<User>(), It.IsAny<IdentityUser>()));
            mockUserData.Setup(x => x.GetUsers()).Returns(MockData.ownerList.AsQueryable);

            userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(MockData.identityUsers[0]));

            var task = Task.Run(async () => await business.DeleteUser(1));

            task.Wait();

            mockUserData.Verify(u => u.DeleteUser(It.IsAny<User>(), It.IsAny<IdentityUser>()), Times.Once);
        }

       
        [TestMethod]

        public async Task DeleteUser_IdNotExist_ShouldThrowException()
        {
            mockUserData.Setup(x => x.DeleteUser(It.IsAny<User>(), It.IsAny<IdentityUser>()));
            mockUserData.Setup(x => x.GetUsers()).Returns(MockData.ownerList.AsQueryable);

            await Assert.ThrowsExceptionAsync<Exception>(() => business.DeleteUser(111));

        }
    }
}
