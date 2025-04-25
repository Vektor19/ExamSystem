using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class ExamOwnerOrAdminAuthorize : Attribute, IAsyncAuthorizationFilter
{
    private readonly IExamService _examService;
    private readonly string _routeKey;

    public ExamOwnerOrAdminAuthorize(IExamService examService, string routeKey = "id")
    {
        _examService = examService;
        _routeKey = routeKey;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        var userRole = user.FindFirstValue(ClaimTypes.Role);
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (userRole == SystemRoles.Admin)
            return;

        var routeIdString = context.RouteData.Values[_routeKey]?.ToString();
        if (!Guid.TryParse(routeIdString, out var examId))
        {
            context.Result = new ForbidResult();
            return;
        }

        var result = await _examService.GetByIdAsync(examId);

        if (!result.Success || result.Data!.CreatedBy.UserId.ToString() != userId)
        {
            context.Result = new ForbidResult();
        }
    }
}
