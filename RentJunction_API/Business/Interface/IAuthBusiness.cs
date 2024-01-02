using RentJunction_API.Models.ViewModels;
using System.Threading.Tasks;

namespace RentJunction_API.Business.Interface
{
    public interface IAuthBusiness
    {
        Task AddUserAsync(RegisterDTO model,bool isAdmin);
        Task Login(LoginDTO model);
        Task Logout();
    }
}
