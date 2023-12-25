using RentJunction_API.Models.ViewModels;
using System.Threading.Tasks;

namespace RentJunction_API.Business.Interface
{
    public interface IAuthBusiness
    {
        Task<bool> AddUserAsync(RegisterDTO model,bool isAdmin);
        Task<bool> Login(LoginDTO model);
        Task<bool> Logout();
    }
}
