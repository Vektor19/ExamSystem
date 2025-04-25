using ExamSystem.Core.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Text.Json;

public class CreateExamAuthorize : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        var userRole = user.FindFirstValue(ClaimTypes.Role);
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userRole))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (userRole != SystemRoles.Admin && userRole != SystemRoles.Examinator)
        {
            context.Result = new ForbidResult();
            return;
        }

        context.HttpContext.Request.EnableBuffering();
        using var reader = new StreamReader(context.HttpContext.Request.Body, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        context.HttpContext.Request.Body.Position = 0;

        try
        {
            using var jsonDoc = JsonDocument.Parse(body);
            if (jsonDoc.RootElement.TryGetProperty("createdByUserId", out var createdByProp))
            {
                var createdByUserId = createdByProp.GetString();
                if (createdByUserId != userId)
                {
                    context.Result = new ForbidResult();
                }
            }
            else
            {
                context.Result = new BadRequestObjectResult("Field 'createdByUserId' is required.");
            }
        }
        catch (JsonException)
        {
            context.Result = new BadRequestObjectResult("Invalid JSON.");
        }
    }
}
