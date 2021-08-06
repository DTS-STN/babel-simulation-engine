using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace esdc_simulation_api.Controllers
{
    public class DisableFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context) {
            throw new NotSupportedException("Functionality not currently supported");
        }
    }
}