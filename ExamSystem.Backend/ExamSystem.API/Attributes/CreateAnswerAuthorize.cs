using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Text.Json;

public class CreateAnswerAuthorize : Attribute, IAsyncAuthorizationFilter
{
    private readonly IExamService _examService;

    public CreateAnswerAuthorize(IExamService examService)
    {
        _examService = examService;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
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

        context.HttpContext.Request.EnableBuffering();
        using var reader = new StreamReader(context.HttpContext.Request.Body, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        context.HttpContext.Request.Body.Position = 0;

        try
        {
            using var jsonDoc = JsonDocument.Parse(body);
            if (jsonDoc.RootElement.TryGetProperty("userId", out var userIdProp))
            {
                var userIdFromAnswer = userIdProp.GetString();
                if (userIdFromAnswer != userId)
                {
                    context.Result = new ForbidResult();
                }
                if (jsonDoc.RootElement.TryGetProperty("examId", out var examIdProp))
                {
                    var examId = examIdProp.GetGuid();
                    if ((await _examService.IsParticipantAsync(examId, userIdProp.GetGuid())).Success)
                    {
                        return;
                    }
                    else
                    {
                        context.Result = new ForbidResult();
                    }
                }
                else
                {
                    context.Result = new BadRequestObjectResult("Field 'examId' is required.");
                }
            }
            else
            {
                context.Result = new BadRequestObjectResult("Field 'userId' is required.");
            }
        }
        catch (JsonException)
        {
            context.Result = new BadRequestObjectResult("Invalid JSON.");
        }
    }
}
