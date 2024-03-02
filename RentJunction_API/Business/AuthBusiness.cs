using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RentJunction_API.Business.Interface;
using RentJunction_API.Data;
using RentJunction_API.DataAccess.Interface;
using RentJunction_API.Models;
using RentJunction_API.Models.Enums;
using RentJunction_API.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RentJunction_API.Business
{
    public class AuthBusiness : IAuthBusiness
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        HttpContext httpContext;
        private readonly IUserData userData;
      

        public AuthBusiness(SignInManager<IdentityUser> signInManager,UserManager<IdentityUser> userManager, IUserData userData ) 
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
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
                    throw new Exception("Invalid Role ID");
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
            else
            {
                throw new Exception("User already exists!");
            }

        }
        public async Task<User> Login(LoginDTO model)
        {
            
            var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

            if (result == null || !result.Succeeded)
            {
                throw new Exception("Invalid Username or Password. Please try again");
            }
            else
            {
                var user = await userData.GetUserByUsername(model.Username);

                return user;
            }   



        }

        public async Task Logout()
        {
          await signInManager.SignOutAsync();
            
        }
    }
}