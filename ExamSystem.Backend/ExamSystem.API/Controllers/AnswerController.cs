using Microsoft.AspNetCore.Mvc;
using ExamSystem.Application.DTOs;
using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Common;
using Microsoft.AspNetCore.Authorization;
using ExamSystem.API.Extensions;

namespace ExamSystem.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class AnswerController : ControllerBase
    {
        private readonly ILogger<AnswerController> _logger;
        private readonly IAnswerService _answerService;
        private readonly IExamService _examService;
        private readonly IQuestionService _questionService;

        public AnswerController(ILogger<AnswerController> logger, IAnswerService answerService, IExamService examService, IQuestionService questionService)
        {
            _logger = logger;
            _answerService = answerService;
            _examService = examService;
            _questionService = questionService;
        }
        [Authorize(Roles = SystemRoles.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _answerService.GetAllAsync();
            return result.ToActionResult();
        }
        [TypeFilter(typeof(ExamOwnerOrAdminAuthorize))]
        [HttpGet("by-exam/{id}")]
        public async Task<IActionResult> GetAllByExamId(Guid id)
        {
            var result = await _answerService.GetAllByExamIdAsync(id);
            return result.ToActionResult();
        }

        [TypeFilter(typeof(SelfOrAdminAuthorize))]
        [HttpGet("by-participant/{id}")]
        public async Task<IActionResult> GetAllByParticipantIdAsync(Guid id)
        {
            var result = await _answerService.GetAllByUserIdAsync(id);
            return result.ToActionResult();
        }

        [Authorize(Roles = SystemRoles.Admin)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _answerService.GetByIdAsync(id);
            return result.ToActionResult();
        }
        [TypeFilter(typeof(CreateAnswerAuthorize))]
        [HttpPost("open-type")]
        public async Task<IActionResult> CreateOpenAnswer([FromBody] CreateOpenAnswerDto answerDto)
        {
            var examInProgressResult = await _examService.IsExamInProgressAsync(answerDto.ExamId);
            if (!examInProgressResult.Success || !examInProgressResult.Data)
            {
                return BadRequest("Exam is not in progress.");
            }
            var isUserBlockedResult = await _examService.IsUserBlockedInExamAsync(answerDto.ExamId, answerDto.UserId);
            if (isUserBlockedResult.Success && isUserBlockedResult.Data)
            {
                return Forbid("You are blocked from answering this exam.");
            }
            var result = await _answerService.CreateOpenAnswerAsync(answerDto);
            if (result.Success)
            {
                var unansweredResult = await _questionService.GetAllUnansweredByUserAsync(answerDto.UserId, answerDto.ExamId);
                if (unansweredResult.Success && !unansweredResult.Data!.ToList().Any())
                {
                    await _examService.FinishExamAsync(answerDto.ExamId, answerDto.UserId);
                };
                return Ok(result);
            }
            return BadRequest(result.ErrorMessage);
        }

        [TypeFilter(typeof(CreateAnswerAuthorize))]
        [HttpPost("option-type")]
        public async Task<IActionResult> CreateOptionAnswer([FromBody] CreateOptionAnswerDto answerDto)
        {
            var examInProgressResult = await _examService.IsExamInProgressAsync(answerDto.ExamId);
            if (!examInProgressResult.Success || !examInProgressResult.Data)
            {
                return BadRequest("Exam is not in progress.");
            }
            var isUserBlockedResult = await _examService.IsUserBlockedInExamAsync(answerDto.ExamId, answerDto.UserId);
            if (isUserBlockedResult.Success && isUserBlockedResult.Data)
            {
                return Forbid("You are blocked from answering this exam.");
            }
            var result = await _answerService.CreateOptionAnswerAsync(answerDto);
            if (result.Success)
            {
                var unansweredResult = await _questionService.GetAllUnansweredByUserAsync(answerDto.UserId, answerDto.ExamId);
                if (unansweredResult.Success && !unansweredResult.Data!.ToList().Any())
                {
                    await _examService.FinishExamAsync(answerDto.ExamId, answerDto.UserId);
                };
                return Ok(result);
            }
            return BadRequest(result.ErrorMessage);
        }
        [TypeFilter(typeof(ExamUserAuthorize))]
        [HttpGet("by-examuser/{id}")]
        public async Task<IActionResult> GetAllByExamUserId(Guid id)
        {
            var result = await _answerService.GetAllByExamUserIdAsync(id);
            return result.ToActionResult();
        }
    }
}