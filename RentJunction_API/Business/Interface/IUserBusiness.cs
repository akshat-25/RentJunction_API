using RentJunction_API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RentJunction_API.Business.Interface
{
    public interface IUserBusiness
    {
        IQueryable<User> GetCustomers();
        IQueryable<User> GetOwners();
        Task<bool> DeleteUser(int id);
    }
}
