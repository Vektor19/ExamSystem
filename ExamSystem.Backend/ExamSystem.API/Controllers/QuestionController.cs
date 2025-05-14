using Microsoft.AspNetCore.Mvc;
using ExamSystem.Application.DTOs;
using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Common;
using Microsoft.AspNetCore.Authorization;
using ExamSystem.Application.DTOs.Question;

namespace ExamSystem.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly ILogger<QuestionController> _logger;
        private readonly IQuestionService _questionService;

        public QuestionController(ILogger<QuestionController> logger, IQuestionService questionService)
        {
            _logger = logger;
            _questionService = questionService;
        }
        [Authorize(Roles = SystemRoles.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _questionService.GetAllAsync();
            return result.Success ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }
        [TypeFilter(typeof(ExamReadAccessAuthorize))]
        [HttpGet("by-exam/{id}")]
        public async Task<IActionResult> GetAllByExam(Guid id)
        {
            var result = await _questionService.GetAllByExamIdAsync(id);
            return result.Success ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        [TypeFilter(typeof(SelfOrAdminAuthorize))]
        [HttpGet("not-completed/by-user/{id}/exam/{examId}")]
        public async Task<IActionResult> GetAllUnansweredByUser(Guid id, Guid examId)
        {
            var result = await _questionService.GetAllUnansweredByUserAsync(id, examId);
            return result.Success ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        [TypeFilter(typeof(QuestionOwnerOrAdminAuthorize))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _questionService.GetByIdAsync(id);
            return result.Success ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }
        [TypeFilter(typeof(CreateQuestionAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] QuestionCreateDto questionDto)
        {
            var result = await _questionService.CreateAsync(questionDto);
            return result.Success ? Ok(result) : BadRequest(result.ErrorMessage);
        }
        [TypeFilter(typeof(QuestionOwnerOrAdminAuthorize))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _questionService.DeleteAsync(id);
            return result.Success ? Ok(result) : BadRequest(result.ErrorMessage);
        }
        [TypeFilter(typeof(QuestionOwnerOrAdminAuthorize))]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] QuestionUpdateDto questionDto)
        {
            var result = await _questionService.UpdateAsync(id, questionDto);
            return result.Success ? Ok(result) : BadRequest(result.ErrorMessage);
        }
        [TypeFilter(typeof(QuestionOwnerOrAdminAuthorize))]
        [HttpPost("{id}/grade-openanswer")]
        public async Task<IActionResult> GradeTextQuestionAnswer(Guid id, [FromBody] GradeOpenAnswerDto gradeOpenAnswerDto)
        {
            var result = await _questionService.GradeTextQuestionAnswerAsync(id, gradeOpenAnswerDto);
            return result.Success ? Ok(result) : BadRequest(result.ErrorMessage);
        }
    }
}