using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Diagnostics.CodeAnalysis;

namespace RentJunction_API.CustomFilters
{
    [ExcludeFromCodeCoverage]
    public class CustomerAuthorizeFilter : Attribute, IAuthorizationFilter
    { 
            public void OnAuthorization(AuthorizationFilterContext context)
            {
                if (!context.HttpContext.User.IsInRole("Customer"))
                {
                    context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
            }
        
    }
}
