using Microsoft.AspNetCore.Identity;
using RentJunction_API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RentJunction_API.DataAccess.Interface
{
    public interface IUserData
    {
        void AddUser(User user);
        Task<IdentityResult> CreateUserAsync(IdentityUser user, string password);
        Task DeleteUser(User user, IdentityUser userDelete);
        IQueryable<User> GetUsers();
        Task AddToRole(IdentityUser user, string role);
    }
}