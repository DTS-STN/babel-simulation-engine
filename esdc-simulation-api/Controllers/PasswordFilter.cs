using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace esdc_simulation_api.Controllers
{
    public class PasswordFilterOptions {
        public string Password { get; set; }
    }

    public class PasswordFilterAttribute : ActionFilterAttribute
    {
        private readonly string _password;

        public PasswordFilterAttribute(IOptions<PasswordFilterOptions> options) {
            _password = options.Value.Password;
        }

        public override void OnActionExecuting(ActionExecutingContext context) {
            var headerPass = context.HttpContext.Request.Headers["password"];
            if (headerPass != _password) {
                throw new UnauthorizedAccessException("Correct value missing from 'password' header");
            }
        }
    }
}