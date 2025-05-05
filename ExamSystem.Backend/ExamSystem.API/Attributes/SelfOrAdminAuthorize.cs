using ExamSystem.Core.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class SelfOrAdminAuthorize : Attribute, IAuthorizationFilter
{
    public string RouteKey { get; set; } = "id";

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

        if (roles.Contains(SystemRoles.Admin))
            return;

        var routeIdString = context.RouteData.Values[RouteKey]?.ToString();
        if (!Guid.TryParse(routeIdString, out var routeId))
        {
            context.Result = new ForbidResult();
            return;
        }

        if (userId != routeId.ToString())
        {
            context.Result = new ForbidResult();
        }
    }
}