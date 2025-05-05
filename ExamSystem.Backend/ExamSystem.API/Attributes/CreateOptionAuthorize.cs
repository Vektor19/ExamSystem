using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Text.Json;

public class CreateOptionAuthorize : Attribute, IAsyncAuthorizationFilter
{
    private readonly IQuestionService _questionService;

    public CreateOptionAuthorize(IQuestionService questionService)
    {
        _questionService = questionService;
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
            if (jsonDoc.RootElement.TryGetProperty("questionId", out var quesitonProp))
            {
                var questionIdString = quesitonProp.GetString();
                if (!Guid.TryParse(questionIdString, out var questionId))
                {
                    context.Result = new ForbidResult();
                    return;
                }

                var resultQuestion = await _questionService.GetByIdAsync(questionId);
                if (!resultQuestion.Success || resultQuestion.Data == null)
                {
                    context.Result = new ForbidResult();
                    return;
                }
                var question = resultQuestion.Data!;

                if (question.Exam.CreatedBy.UserId.ToString() != userId)
                {
                    context.Result = new ForbidResult();
                }
            }
            else
            {
                context.Result = new BadRequestObjectResult("Field 'questionId' is required.");
            }
        }
        catch (JsonException)
        {
            context.Result = new BadRequestObjectResult("Invalid JSON.");
        }
    }
}
