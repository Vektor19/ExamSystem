using Microsoft.AspNetCore.Mvc;
using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Common;
using Microsoft.AspNetCore.Authorization;
using ExamSystem.Application.DTOs.QuestionOption;
using ExamSystem.API.Extensions;

namespace ExamSystem.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class QuestionOptionController : ControllerBase
    {
        private readonly ILogger<QuestionOptionController> _logger;
        private readonly IQuestionOptionService _questionOptionService;

        public QuestionOptionController(ILogger<QuestionOptionController> logger, IQuestionOptionService questionService)
        {
            _logger = logger;
            _questionOptionService = questionService;
        }
        [Authorize(Roles = SystemRoles.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _questionOptionService.GetAllAsync();
            return result.ToActionResult();
        }
        [TypeFilter(typeof(QuestionOwnerOrAdminAuthorize))]
        [HttpGet("by-question/{id}")]
        public async Task<IActionResult> GetAllByQuestion(Guid id)
        {
            var result = await _questionOptionService.GetAllByQuestionIdAsync(id);
            return result.ToActionResult();
        }

        [TypeFilter(typeof(OptionOwnerOrAdminAuthorize))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _questionOptionService.GetByIdAsync(id);
            return result.ToActionResult();
        }
        [TypeFilter(typeof(CreateOptionAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] QuestionOptionCreateDto questionDto)
        {
            var result = await _questionOptionService.CreateAsync(questionDto);
            return result.ToActionResult();
        }
        [TypeFilter(typeof(OptionOwnerOrAdminAuthorize))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _questionOptionService.DeleteAsync(id);
            return result.ToActionResult();
        }
        [TypeFilter(typeof(OptionOwnerOrAdminAuthorize))]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] QuestionOptionUpdateDto questionDto)
        {
            var result = await _questionOptionService.UpdateAsync(id, questionDto);
            return result.ToActionResult();
        }
    }
}