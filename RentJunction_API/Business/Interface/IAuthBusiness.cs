using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RentJunction_API.Models;
using RentJunction_API.Models.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RentJunction_API.Business.Interface
{
    public interface IAuthBusiness
    {
        Task AddUserAsync(RegisterDTO model,bool isAdmin);
        Task<User> Login(LoginDTO model);
        Task Logout();
    }
}
