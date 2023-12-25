using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace RentJunction_API.CustomFilters
{
    public class CustomActionFilter : IActionFilter
    {
        private readonly ILogger<CustomActionFilter> logger;

        public CustomActionFilter(ILogger<CustomActionFilter> logger)
        {
            this.logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation($"Action {context.ActionDescriptor.DisplayName} has started");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation($"Action {context.ActionDescriptor.DisplayName} has completed");
        }
    }
}
