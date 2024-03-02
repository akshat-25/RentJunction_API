using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentJunction_API.Business.Interface;
using RentJunction_API.CustomFilters;
using RentJunction_API.Helper;
using RentJunction_API.Models.Enums;
using RentJunction_API.Models.ViewModels;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace RentJunction_API.Controllers
{
    [Route("api/[Controller]")] 
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductBusiness productBusiness;
        public ProductController(IProductBusiness productBusiness)
        {
            this.productBusiness = productBusiness;
        }

        [HttpGet]
        [ServiceFilter(typeof(CustomActionFilter))]

        public IActionResult GetProducts(string city, int? categoryId)
        {
            try
            {
                var products = productBusiness.GetProducts(city, categoryId);
                return Ok(products);

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [HttpPost]
        [OwnerAuthorizeFilter]
        [ServiceFilter(typeof(CustomActionFilter))]

        public IActionResult AddProduct(AddProductDTO product)
        {
            try
            {
                var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                productBusiness.AddProduct(product, username);
                return Ok("Product Added Successfully");
               
            }
            catch (HttpStatusCodeException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("ViewProducts")]
        //[OwnerAuthorizeFilter]
        [ServiceFilter(typeof(CustomActionFilter))]
        [ProducesResponseType(StatusCodes.Status200OK,Type=typeof(String))]
        public IActionResult ViewListedProducts() 
        {
            try
            {
                var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                var products = productBusiness.ViewListedProducts(username);

                return StatusCode(StatusCodes.Status200OK,products);
            }
            catch (HttpStatusCodeException ex)
            {
                throw ex;
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

        [HttpDelete("{id}")]
        //[OwnerAuthorizeFilter]
        [ServiceFilter(typeof(CustomActionFilter))]

        public IActionResult DeleteProduct(int productId)
        {
            try
            {
                var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                productBusiness.DeleteProduct(username, productId);
                
                return Ok("Product Deleted Successfully");

            }
            catch(HttpStatusCodeException ex)
            {
                throw ex;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("ViewProductDetails/{id}")]
        [ServiceFilter(typeof(CustomActionFilter))]

        public IActionResult ViewProductDetails(int id)
        {
            try
            {
                var product = productBusiness.ViewProductDetail(id);
                
                return Ok(product);
            }
            catch (HttpStatusCodeException ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [OwnerAuthorizeFilter]
        [ServiceFilter(typeof(CustomActionFilter))]

        public IActionResult UpdateProduct(int id,[FromBody] AddProductDTO product)
        {
            try
            {
                var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                productBusiness.UpdateProduct(id, product, username);
               
                return Ok("Product Details Updated..");
                
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

        [HttpPost("rental/{id}")]
        [CustomerAuthorizeFilter]
        [ServiceFilter(typeof(CustomActionFilter))]

        public IActionResult RentProduct(int id, [FromBody] RentProductDTO model)
        {
            try
            {
                    var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                    productBusiness.RentProduct(id, model, username);
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

        [HttpPut("extendRent/{id}")]
        [CustomerAuthorizeFilter]
        [ServiceFilter(typeof(CustomActionFilter))]

        public IActionResult ExtendRentPeriod(int id, [FromBody] ExtendRentDTO model)
        {
            try
            {
                    var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                    productBusiness.ExtendRentPeriod(id, model, username);
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
    }
}