using Microsoft.AspNetCore.Identity;
using RentJunction_API.Business.Interface;
using RentJunction_API.Data;
using RentJunction_API.DataAccess.Interface;
using RentJunction_API.Models;
using RentJunction_API.Models.Enums;
using RentJunction_API.Models.ViewModels;
using System;
using System.Threading.Tasks;

namespace RentJunction_API.Business
{
    public class AuthBusiness : IAuthBusiness
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IUserData userData;

        public AuthBusiness(SignInManager<IdentityUser> signInManager
            , IUserData userData) 
        {
            this.signInManager = signInManager;
            this.userData = userData;
        }

        public async Task AddUserAsync(RegisterDTO model,bool isAdmin)
        {
            var appUser = new IdentityUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
            };

            IdentityResult result;

            result =  await userData.CreateUserAsync(appUser, model.Password);

            if (isAdmin)
            {
                await userData.AddToRole(appUser, "Admin");
            }

            else
            {
                if (model.RoleId == RolesEnum.Owner)
                {
                    await userData.AddToRole(appUser, "Owner");
                }
                else if(model.RoleId == RolesEnum.Customer)
                {
                    await userData.AddToRole(appUser, "Customer");
                }
                else
                {
                    throw new Exception("An error has occured. Please try again");
                }
            }

            if (result.Succeeded)
            {
                var newUser = new User()
                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    City = model.City,
                    PhoneNumber = model.PhoneNumber,
                    RoleId = isAdmin ? RolesEnum.Admin : model.RoleId,
                    UserId = appUser.Id
                };

                userData.AddUser(newUser);

               
            }

            throw new Exception("An error has occured. Please try again");

        }
        public async Task Login(LoginDTO model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

            if (result == null || !result.Succeeded )
            {
                throw new Exception("An error has occured. Please try again");
            }
        }
        public async Task Logout()
        {
          await signInManager.SignOutAsync();
            
        }
    }
}