using Microsoft.AspNetCore.Identity;
using RentJunction_API.Data;
using RentJunction_API.DataAccess.Interface;
using RentJunction_API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RentJunction_API.DataAccess
{
    public class UserData : IUserData
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly AppDbContext appDbContext;

        public UserData(UserManager<IdentityUser> userManager, AppDbContext appDbContext)
        {
            this.userManager = userManager;
            this.appDbContext = appDbContext;
        }

        public async Task<IdentityResult> CreateUserAsync(IdentityUser user, string password)
        {
            return await userManager.CreateAsync(user, password);
        }

        public void AddUser(User user)
        {
            appDbContext.Users.Add(user);

            appDbContext.SaveChanges();
        }

        public IQueryable<User> GetUsers()
        {
            return appDbContext.Users;
        }

        public async Task DeleteUser(User user, IdentityUser userDelete)
        {
            await DeleteUserAsync(userDelete);
            appDbContext.Users.Remove(user);
            appDbContext.SaveChanges();
        }

        public async Task DeleteUserAsync(IdentityUser user)
        {
            await userManager.DeleteAsync(user);
        }
        public async Task AddToRole(IdentityUser user, string role)
        {
            await userManager.AddToRoleAsync(user, role);
        }
    }
}
