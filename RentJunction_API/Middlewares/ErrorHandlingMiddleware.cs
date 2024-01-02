using Microsoft.AspNetCore.Http;
using RentJunction_API.Helper;
using RentJunction_API.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace RentJunction_API.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (HttpStatusCodeException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception exceptionObj)
            {
                await HandleExceptionAsync(context, exceptionObj);
            }

        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string result = new ErrorResponseModel()
            {
                Message = exception.Message,
                StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError)
            }.ToString();
            context.Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
            

            return context.Response.WriteAsync(result);
        }

        private Task HandleExceptionAsync(HttpContext context, HttpStatusCodeException exception)
        {
            string result = null;
            context.Response.ContentType = "applicaiton/json";
            if (exception is HttpStatusCodeException)
            {
                result = new ErrorResponseModel()
                {
                    Message = exception.Message,
                    StatusCode = (int)exception.StatusCode
                }.ToString();
                context.Response.StatusCode = (int)exception.StatusCode;
            }
            else
            {
                result = new ErrorResponseModel()
                {
                    Message = "Runtime Error",
                    StatusCode = (int)HttpStatusCode.BadRequest
                }.ToString();
            }
            return context.Response.WriteAsync(result);
        }
    }
}
