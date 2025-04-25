using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class QuestionOwnerOrAdminAuthorize : Attribute, IAsyncAuthorizationFilter
{
    private readonly IQuestionService _questionService;
    private readonly IExamService _examService;
    private readonly string _routeKey;

    public QuestionOwnerOrAdminAuthorize(IQuestionService questionService, string routeKey = "id", IExamService examService = null)
    {
        _questionService = questionService;
        _routeKey = routeKey;
        _examService = examService;
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

        if (userRole != SystemRoles.Admin && userRole != SystemRoles.Examinator)
        {
            context.Result = new ForbidResult();
            return;
        }
        var routeIdString = context.RouteData.Values[_routeKey]?.ToString();
        if (!Guid.TryParse(routeIdString, out var questionId))
        {
            context.Result = new ForbidResult();
            return;
        }

        var resultQuestion = await _questionService.GetByIdAsync(questionId);

        if (!resultQuestion.Success)
        {
            context.Result = new ForbidResult();
        }
        var question = resultQuestion.Data!;

        var resultExam = await _examService.GetByIdAsync(question.Exam.ExamId);

        if (!resultExam.Success || resultExam.Data!.CreatedBy.UserId.ToString() != userId)
        {
            context.Result = new ForbidResult();
        }
    }
}
