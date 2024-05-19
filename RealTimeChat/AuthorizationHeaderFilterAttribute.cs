using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AuthorizationHeaderFilterAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (string.IsNullOrEmpty( context.HttpContext.Request.Headers.Authorization))
        {
            context.Result = new UnauthorizedResult();
            return;
        }
    }
}
