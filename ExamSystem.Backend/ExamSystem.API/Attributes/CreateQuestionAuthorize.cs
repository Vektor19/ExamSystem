using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Text.Json;

public class CreateQuestionAuthorize : Attribute, IAsyncAuthorizationFilter
{
    private readonly IExamService _examService;

    public CreateQuestionAuthorize(IExamService examService)
    {
        _examService = examService;
    }
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
            if (jsonDoc.RootElement.TryGetProperty("examId", out var examProp))
            {
                var examIdString = examProp.GetString();
                if (!Guid.TryParse(examIdString, out var examId))
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
            else
            {
                context.Result = new BadRequestObjectResult("Field 'examId' is required.");
            }
        }
        catch (JsonException)
        {
            context.Result = new BadRequestObjectResult("Invalid JSON.");
        }
    }
}
