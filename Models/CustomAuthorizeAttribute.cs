using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace KhumaloCraft
{
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _message;

        public CustomAuthorizeAttribute(string message)
        {
            _message = message;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                var returnUrl = context.HttpContext.Request.Path;
                var loginUrl = $"/User/Login?message={Uri.EscapeDataString(_message)}&returnUrl={Uri.EscapeDataString(returnUrl)}";
                context.Result = new RedirectResult(loginUrl);
            }
        }
    }
}
