using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentJunction_API.Business.Interface;
using RentJunction_API.CustomFilters;
using RentJunction_API.Helper;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RentJunction_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness userBusiness;
        public UserController(IUserBusiness userBusiness)
        {
            this.userBusiness = userBusiness;
        }

        [HttpGet("viewCustomers")]
        [AdminAuthorizeFilter]
        public IActionResult GetCustomers()
        {
            try
            {
                if (userBusiness.GetCustomers().ToList().Count > 0)
                {
                    return Ok(userBusiness.GetCustomers());
                }

                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No registered customers !");
            }
            catch(HttpStatusCodeException ex)
            {
                throw ex;
            }
        }

        [HttpGet("viewOwners")]
        [AdminAuthorizeFilter]
        public IActionResult GetOwners()
        {
            try
            {
                if (userBusiness.GetOwners().ToList().Count() > 0)
                {
                    return Ok(userBusiness.GetOwners());
                }

                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No registered owners !");
            }
            catch(HttpStatusCodeException ex)
            {
                throw ex;
            }
        }

        [HttpDelete("{id}")]
        [AdminAuthorizeFilter]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await userBusiness.DeleteUser(id);
                return Ok("Customer Deleted Successfully..");
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "User not found !");
            }
            catch (HttpStatusCodeException exception)
            {
                throw exception;
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }
    }
}
