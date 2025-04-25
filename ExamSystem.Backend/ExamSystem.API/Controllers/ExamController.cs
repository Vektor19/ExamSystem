using Microsoft.AspNetCore.Mvc;
using ExamSystem.Application.DTOs;
using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Common;
using Microsoft.AspNetCore.Authorization;
using ExamSystem.Application.Services;

namespace ExamSystem.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ExamController : ControllerBase
    {
        private readonly ILogger<ExamController> _logger;
        private readonly IExamService _examService;

        public ExamController(ILogger<ExamController> logger, IExamService examService)
        {
            _logger = logger;
            _examService = examService;
        }
        [Authorize(Roles = SystemRoles.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _examService.GetAllAsync();
            return result.Success ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }
        [TypeFilter(typeof(SelfOrAdminAuthorize))]
        [HttpGet("by-me/{id}")]
        public async Task<IActionResult> GetAllByCreatedUserIdAsync(Guid id)
        {
            var result = await _examService.GetAllByCreatedUserIdAsync(id);
            return result.Success ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        [TypeFilter(typeof(SelfOrAdminAuthorize))]
        [HttpGet("by-participant/{id}")]
        public async Task<IActionResult> GetAllByParticipantIdAsync(Guid id)
        {
            var result = await _examService.GetAllByParticipantUserIdAsync(id);
            return result.Success ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        [TypeFilter(typeof(ExamReadAccessAuthorize))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _examService.GetByIdAsync(id);
            return result.Success ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }
        [CreateExamAuthorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExamCreateDto examDto)
        {
            var result = await _examService.CreateAsync(examDto);
            return result.Success ? Ok(result) : BadRequest(result.ErrorMessage);
        }

        [TypeFilter(typeof(ExamOwnerOrAdminAuthorize))]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ExamUpdateDto updateDto)
        {
            var result = await _examService.UpdateAsync(id, updateDto);
            return result.Success ? Ok(result) : BadRequest(result.ErrorMessage);
        }
        [TypeFilter(typeof(ExamOwnerOrAdminAuthorize))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _examService.DeleteAsync(id);
            return result.Success ? Ok(result) : BadRequest(result.ErrorMessage);
        }
        [TypeFilter(typeof(ExamOwnerOrAdminAuthorize))]
        [HttpPost("{id}/participants")]
        public async Task<IActionResult> AddParticipant(Guid id, [FromBody] AddParticipantDto dto)
        {
            var result = await _examService.AddParticipantAsync(id, dto.UserId);
            return result.Success ? Ok(result) : BadRequest(result.ErrorMessage);
        }

    }
}