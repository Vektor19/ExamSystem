using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class ExamReadAccessAuthorize : Attribute, IAsyncAuthorizationFilter
{
    private readonly IExamService _examService;
    private readonly string _routeKey;

    public ExamReadAccessAuthorize(IExamService examService, string routeKey = "id")
    {
        _examService = examService;
        _routeKey = routeKey;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        var userRole = user.FindFirstValue(ClaimTypes.Role);
        var userIdStr = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIdStr == null || !Guid.TryParse(userIdStr, out var userId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (userRole == SystemRoles.Admin)
            return;

        var routeIdStr = context.RouteData.Values[_routeKey]?.ToString();
        if (!Guid.TryParse(routeIdStr, out var examId))
        {
            context.Result = new ForbidResult();
            return;
        }

        var examResult = await _examService.GetByIdAsync(examId);
        if (!examResult.Success)
        {
            context.Result = new NotFoundResult();
            return;
        }

        var exam = examResult.Data!;

        if (exam.CreatedBy.UserId == userId)
            return;

        var isParticipantResult = await _examService.IsParticipantAsync(examId, userId);
        if (!isParticipantResult.Success || !isParticipantResult.Data)
        {
            context.Result = new ForbidResult();
        }
    }
}
