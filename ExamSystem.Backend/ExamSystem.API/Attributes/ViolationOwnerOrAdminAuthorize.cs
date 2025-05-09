using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class ViolationOwnerOrAdminAuthorize : Attribute, IAsyncAuthorizationFilter
{
    private readonly IViolationService _violationService;
    private readonly IExamService _examService;
    private readonly string _routeKey;

    public ViolationOwnerOrAdminAuthorize(IViolationService violationService, IExamService examService, string routeKey = "id")
    {
        _violationService = violationService;
        _examService = examService;
        _routeKey = routeKey;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

        if (!roles.Contains(SystemRoles.Admin) && !roles.Contains(SystemRoles.Examinator))
        {
            context.Result = new ForbidResult();
            return;
        }

        var routeIdString = context.RouteData.Values[_routeKey]?.ToString();
        if (!Guid.TryParse(routeIdString, out var violationId))
        {
            context.Result = new ForbidResult();
            return;
        }

        var violationResult = await _violationService.GetByIdAsync(violationId);
        if (!violationResult.Success)
        {
            context.Result = new ForbidResult();
            return;
        }

        var examUserResult = await _examService.GetExamUserByIdAsync(violationResult.Data!.ExamUserId);

        if (!examUserResult.Success || examUserResult.Data!.UserId.ToString() != userId)
        {
            context.Result = new ForbidResult();
        }
    }
}
