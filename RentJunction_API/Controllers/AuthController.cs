using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentJunction_API.Business.Interface;
using RentJunction_API.CustomFilters;
using RentJunction_API.Helper;
using RentJunction_API.Models.ViewModels;
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
                if (!ModelState.IsValid)
                {
                    throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Registration Failed! Please try again");
                }

                bool result;

                if (User.IsInRole("Admin"))
                {
                    result = await accountBusiness.AddUserAsync(model, true);
                }

                else
                {
                    result = await accountBusiness.AddUserAsync(model, false);
                }

                if (result)
                {
                    return StatusCode(StatusCodes.Status201Created);
                }

                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Registration Falied! Please try again");
            }
            catch(HttpStatusCodeException ex)
            {
                throw ex;
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
                    var result = await accountBusiness.Login(model);

                    return Ok("Login Successful");
                }

                throw new HttpStatusCodeException(HttpStatusCode.Unauthorized, "Login Failed!");
            }
            catch(HttpStatusCodeException ex)
            {
                throw ex;
            }
        }

        [HttpPost("logout")]
        [ServiceFilter(typeof(CustomActionFilter))]
        public async Task<IActionResult> Logout()
        {
            try
            {
                if (await accountBusiness.Logout())
                {
                    return Ok();
                }

                throw new HttpStatusCodeException(HttpStatusCode.NoContent, "User not logged in");
            }
            catch(HttpStatusCodeException ex) 
            {
                throw ex;
            }
        }
    }
}