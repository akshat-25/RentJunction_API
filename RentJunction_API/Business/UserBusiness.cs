using Microsoft.AspNetCore.Identity;
using RentJunction_API.Business.Interface;
using RentJunction_API.Data;
using RentJunction_API.DataAccess.Interface;
using RentJunction_API.Helper;
using RentJunction_API.Models;
using RentJunction_API.Models.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentJunction_API.Business
{
    public class UserBusiness : IUserBusiness
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IUserData userData;
        public UserBusiness(UserManager<IdentityUser> userManager,IUserData userData)
        {

            this.userManager = userManager;
            this.userData = userData;

        }

        public IQueryable<User> GetCustomers()
        {
            var customers = userData.GetUsers().Where(user => user.RoleId == RolesEnum.Customer);
            return customers;
        } 
        
        public IQueryable<User> GetOwners()
        {
            var owners = userData.GetUsers().Where(user => user.RoleId == RolesEnum.Owner);
            return owners;
        }

        public async Task<bool> DeleteUser(int id)
        {
            try
            {
                var user = userData.GetUsers().FirstOrDefault(user => user.Id == id);
                var userDelete = userManager.FindByIdAsync(user.UserId);
           
                if (user.RoleId == RolesEnum.Admin)
                {
                    return false;
                }
                await userData.DeleteUser(user,await userDelete);
                return true;
            }
            catch(HttpStatusCodeException ex)
            {
                throw ex;
            }
                
        }
    }
}