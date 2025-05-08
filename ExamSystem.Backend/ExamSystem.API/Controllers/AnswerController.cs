using Microsoft.AspNetCore.Mvc;
using ExamSystem.Application.DTOs;
using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Common;
using Microsoft.AspNetCore.Authorization;

namespace ExamSystem.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class AnswerController : ControllerBase
    {
        private readonly ILogger<AnswerController> _logger;
        private readonly IAnswerService _answerService;

        public AnswerController(ILogger<AnswerController> logger, IAnswerService examService)
        {
            _logger = logger;
            _answerService = examService;
        }
        [Authorize(Roles = SystemRoles.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _answerService.GetAllAsync();
            return result.Success ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }
        [TypeFilter(typeof(ExamOwnerOrAdminAuthorize))]
        [HttpGet("by-exam/{id}")]
        public async Task<IActionResult> GetAllByExamId(Guid id)
        {
            var result = await _answerService.GetAllByExamIdAsync(id);
            return result.Success ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        [TypeFilter(typeof(SelfOrAdminAuthorize))]
        [HttpGet("by-participant/{id}")]
        public async Task<IActionResult> GetAllByParticipantIdAsync(Guid id)
        {
            var result = await _answerService.GetAllByUserIdAsync(id);
            return result.Success ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        [Authorize(Roles = SystemRoles.Admin)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _answerService.GetByIdAsync(id);
            return result.Success ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }
        [TypeFilter(typeof(CreateAnswerAuthorize))]
        [HttpPost("open-type")]
        public async Task<IActionResult> CreateOpenAnswer([FromBody] CreateOpenAnswerDto answerDto)
        {
            var result = await _answerService.CreateOpenAnswerAsync(answerDto);
            return result.Success ? Ok(result) : BadRequest(result.ErrorMessage);
        }

        [TypeFilter(typeof(CreateAnswerAuthorize))]
        [HttpPost("option-type")]
        public async Task<IActionResult> CreateOptionAnswer([FromBody] CreateOptionAnswerDto answerDto)
        {
            var result = await _answerService.CreateOptionAnswerAsync(answerDto);
            return result.Success ? Ok(result) : BadRequest(result.ErrorMessage);
        }
    }
}