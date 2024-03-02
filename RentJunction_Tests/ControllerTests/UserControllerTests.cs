using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RentJunction_API.Business.Interface;
using RentJunction_API.Controllers;
using RentJunction_API.DataAccess;
using RentJunction_API.Helper;
using RentJunction_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentJunction_Tests.ControllerTests
{
    [TestClass]
    public class UserControllerTests
    {
        private Mock<IUserBusiness> mockUserBusiness;
        private UserController controller;

        [TestInitialize]
        public void TestInitialize()
        {
            mockUserBusiness = new Mock<IUserBusiness>();
            controller = new UserController(mockUserBusiness.Object);
        }

        [TestMethod]
        public void GetCustomers()
        { 
            mockUserBusiness.Setup(x => x.GetCustomers()).Returns(MockData.customersList.AsQueryable());
            var result = controller.GetCustomers() as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }

        [TestMethod]

        public void GetCustomer_ThrowException_ShouldThrowException()
        {
            mockUserBusiness.Setup(x => x.GetCustomers()).Returns(new List<User>().AsQueryable());

            Assert.ThrowsException<HttpStatusCodeException>(() =>  controller.GetCustomers());
        }
        [TestMethod]
        public void GetOwners()
        {
            mockUserBusiness.Setup(x => x.GetOwners()).Returns(MockData.ownerList.AsQueryable());
            var result = controller.GetOwners() as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }

        [TestMethod]
        public void GetOwners_ThrowException_ShouldThrowException()
        {
            mockUserBusiness.Setup(x => x.GetOwners()).Returns(new List<User>().AsQueryable());

            Assert.ThrowsException<HttpStatusCodeException>(() => controller.GetOwners());
        }

        [TestMethod]
        public void DeleteUser_ValidId_ShouldDeleteUser()
        {
            mockUserBusiness.Setup(x => x.DeleteUser(It.IsAny<int>()));

            var task = Task.Run(async () => await controller.DeleteUser(1));
            task.Wait();

            var result = task.Result;

            var okResult = (OkObjectResult)result;

            Assert.IsNotNull(result);
            Assert.AreEqual("Customer Deleted Successfully..", okResult.Value);
        }

        [TestMethod]

        public async Task DeleteUser_ThrowException_ShouldThrowException()
        {
            mockUserBusiness.Setup(x => x.DeleteUser(It.IsAny<int>())).ThrowsAsync(new System.Exception());

            await Assert.ThrowsExceptionAsync<Exception>(() =>  controller.DeleteUser(1));

        }
    }
}
