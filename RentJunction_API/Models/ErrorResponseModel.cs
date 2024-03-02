using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;

namespace RentJunction_API.Models
{
    [ExcludeFromCodeCoverage]
    public class ErrorResponseModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
