using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentJunction_API.Business.Interface;
using RentJunction_API.CustomFilters;
using RentJunction_API.Helper;
using RentJunction_API.Models.ViewModels;
using System;
using System.Net;
using System.Threading.Tasks;

namespace RentJunction_API.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthBusiness accountBusiness;
        public AuthController(IAuthBusiness accountBusiness)
        {  
            this.accountBusiness = accountBusiness;
        }

        [HttpPost]
        [ServiceFilter(typeof(CustomActionFilter))]

        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            try
            {         
                if (User.IsInRole("Admin"))
                {
                    await accountBusiness.AddUserAsync(model, true);
                }

                else
                {
                   await accountBusiness.AddUserAsync(model, false);
                }

                return StatusCode(StatusCodes.Status201Created);
            }
            catch(HttpStatusCodeException exception)
            {
                throw exception;
            }
            catch(Exception exception)
            {
                throw exception;
            }
            
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(CustomActionFilter))]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await accountBusiness.Login(model);

                    return Ok("Login Successful");
                }

                throw new HttpStatusCodeException(HttpStatusCode.Unauthorized, "Login Failed!");
            }
            catch(HttpStatusCodeException exception)
            {
                throw exception;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [HttpPost("logout")]
        [ServiceFilter(typeof(CustomActionFilter))]
        public async Task<IActionResult> Logout()
        {
            try
            {
              await accountBusiness.Logout();
               
              return Ok();
                
              throw new HttpStatusCodeException(HttpStatusCode.NoContent, "User not logged in");

            }
            catch(HttpStatusCodeException ex) 
            {
                throw ex;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}