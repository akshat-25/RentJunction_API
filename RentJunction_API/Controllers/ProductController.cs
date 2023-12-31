﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentJunction_API.Business.Interface;
using RentJunction_API.CustomFilters;
using RentJunction_API.Helper;
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
                if (products.Count() > 0)
                    return Ok(products);

                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No Products found!");
            }
            catch(Exception ex)
            {
                throw ex;
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

                if (ModelState.IsValid)
                {
                    var result = productBusiness.AddProduct(product, username);
                    if (result)
                    {
                        return Ok("Product Added Successfully");
                    }
                }

                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Bad Request");
            }
            catch(HttpStatusCodeException ex)
            {
                throw ex;
            }
        }

        [HttpGet("ViewProducts")]
        [OwnerAuthorizeFilter]
        [ServiceFilter(typeof(CustomActionFilter))]

        public IActionResult ViewListedProducts() 
        {
            try
            {
                var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                var products = productBusiness.ViewListedProducts(username);

                if (products.Count() == 0)
                {
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound, "You have not listed any products"); ;
                }

                return Ok(products);
            }
            catch (HttpStatusCodeException ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [AdminAuthorizeFilter]
        [ServiceFilter(typeof(CustomActionFilter))]

        public IActionResult DeleteProduct(int productId)
        {
            try
            {

                var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                if (productBusiness.DeleteProduct(username, productId))
                {
                    return Ok("Product Deleted Successfully");
                }

                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Delete Failed due to technical issues!");
            }
            catch(HttpStatusCodeException ex)
            {
                throw ex;
            }
        }

        [HttpGet("ViewProductDetails")]
        [ServiceFilter(typeof(CustomActionFilter))]

        public IActionResult ViewProductDetails(int id)
        {
            try
            {
                var product = productBusiness.ViewProductDetail(id);

                if (product != null)
                {
                    return Ok(product);
                }

                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No products found with this ID!");
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
                //if (!User.IsInRole("Owner")) { return StatusCode(StatusCodes.Status401Unauthorized); }

                var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                if (productBusiness.UpdateProduct(id, product, username))
                {
                    return Ok("Product Details Updated..");
                }

                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Update Failed!");
            }
            catch(HttpStatusCodeException ex)
            {
                throw ex;
            }
        }

        [HttpPost("rental")]
        [CustomerAuthorizeFilter]
        [ServiceFilter(typeof(CustomActionFilter))]

        public IActionResult RentProduct(int id, [FromBody] RentProductDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                    if (productBusiness.RentProduct(id, model, username))
                    {
                        return StatusCode(StatusCodes.Status201Created);
                    }
                }

                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Bad request!"); ;
            } 
            catch(HttpStatusCodeException ex)
            {
                throw ex;
            }
        }

        [HttpPost("extendRent")]
        [CustomerAuthorizeFilter]
        [ServiceFilter(typeof(CustomActionFilter))]

        public IActionResult ExtendRentPeriod(int id, [FromBody] ExtendRentDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                    if (productBusiness.ExtendRentPeriod(id, model, username))
                    {
                        return StatusCode(StatusCodes.Status201Created);
                    }
                }
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Cannot extend rent period due to some technical error!");
            }

            catch(HttpStatusCodeException ex)
            {
                throw ex;
            }
        }
    }
}