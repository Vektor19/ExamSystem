using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class OptionOwnerOrAdminAuthorize : Attribute, IAsyncAuthorizationFilter
{
    private readonly IQuestionOptionService _questionOptionService;
    private readonly IQuestionService _questionService;
    private readonly IExamService _examService;
    private readonly string _routeKey;

    public OptionOwnerOrAdminAuthorize(IQuestionOptionService questionOptionService, string routeKey = "id", IExamService examService, IQuestionService questionService)
    {
        _questionOptionService = questionOptionService;
        _routeKey = routeKey;
        _examService = examService;
        _questionService = questionService;
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
        if (!Guid.TryParse(routeIdString, out var questionOptionId))
        {
            context.Result = new ForbidResult();
            return;
        }

        var resultQuestionOption = await _questionOptionService.GetByIdAsync(questionOptionId);

        if (!resultQuestionOption.Success)
        {
            context.Result = new ForbidResult();
        }
        var questionOption = resultQuestionOption.Data!;

        var resultQuestion = await _questionService.GetByIdAsync(questionOption.QuestionId);

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
