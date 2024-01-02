using Microsoft.AspNetCore.Identity;
using RentJunction_API.Business.Interface;
using RentJunction_API.DataAccess.Interface;
using RentJunction_API.Models;
using RentJunction_API.Models.Enums;
using System;
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

        public async Task DeleteUser(int id)
        {
                var user = userData.GetUsers().FirstOrDefault(user => user.Id == id);

                if(user == null)
                {
                    throw new Exception("An error has occured..");
                }

                var userDelete = await userManager.FindByIdAsync(user.UserId);               
                await userData.DeleteUser(user,userDelete);
            
        }
    }
}