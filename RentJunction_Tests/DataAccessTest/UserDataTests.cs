using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RentJunction_API.Data;
using RentJunction_API.DataAccess;
using RentJunction_API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RentJunction_Tests.DataAccessTest
{
    [TestClass]
    public class UserDataTests
    {
        private Mock<UserManager<IdentityUser>> userManagerMock;
        private DbContextOptions<AppDbContext> options;
        private AppDbContext dbContext;
        private UserData userDb;

        [TestInitialize]

        public void TestInitilizd()
        {
            userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "InMemoryDatabase").Options;
            dbContext = new AppDbContext(options);

            userDb = new UserData(userManagerMock.Object,dbContext);
        }
        [TestMethod]

        public void AddUser()
        {
            User user = new User
            {
                Id = 2,
                FirstName = "test1",
                LastName = "abcd",
                City = "testcity1",
                Email = "test1@gmail.com",
                PhoneNumber = "1234567890",
                RoleId = RentJunction_API.Models.Enums.RolesEnum.Owner,
                UserName = "test1234",
            };

            userDb.AddUser(user);
            
            Assert.IsNotNull(user);
            Assert.AreEqual(1, dbContext.Users.Count());
        }

        [TestMethod]
        public void GetUsers()
        {
            dbContext.Users.Add(new User
            {
                Id = 1,
                FirstName = "test1",
                LastName = "abcd",
                City = "testcity1",
                Email = "test1@gmail.com",
                PhoneNumber = "1234567890",
                RoleId = RentJunction_API.Models.Enums.RolesEnum.Customer,
                UserName = "test1234",
            });
            dbContext.SaveChanges();

            var users = userDb.GetUsers().ToList();

            Assert.IsNotNull(users);
            Assert.AreEqual(1, users.Count);
        }

        [TestMethod]
        public void DeleteUser()
        {
            var users = userDb.GetUsers().ToList();
            var user = users[0];

            userDb.DeleteUser(user, MockData.identityUsers[0]).Wait();

            Assert.AreEqual(0,userDb.GetUsers().Count());
        }

        [TestMethod]
        public void DeleteUserAsync()
        {
            userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<IdentityUser>()));

            var result = userDb.DeleteUserAsync(MockData.identityUsers[0]);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateUserAsync()
        {
            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>())).Returns(Task.FromResult(IdentityResult.Success));

            var result = userDb.CreateUserAsync(MockData.identityUsers[0], "password");

            Assert.IsNotNull(result);

        }




    }
}
