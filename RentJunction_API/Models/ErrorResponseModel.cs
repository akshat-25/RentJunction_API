using Newtonsoft.Json;
using System;

namespace RentJunction_API.Models
{
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
