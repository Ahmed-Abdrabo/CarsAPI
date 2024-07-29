using CarAPI_Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CarAPI_Web.Extensions
{
    public class AuthExceptionRedirection : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is AuthException)
                context.Result = new RedirectToActionResult("Login", "Auth", null);
        }
    }
}
