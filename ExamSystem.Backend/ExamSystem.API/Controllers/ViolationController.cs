using Microsoft.AspNetCore.Mvc;
using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Common;
using Microsoft.AspNetCore.Authorization;
using ExamSystem.Application.DTOs.Violation;
using ExamSystem.API.Extensions;

namespace ExamSystem.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ViolationController : ControllerBase
    {
        private readonly ILogger<ViolationController> _logger;
        private readonly IViolationService _violationService;
        private readonly IExamService _examService;

        public ViolationController(ILogger<ViolationController> logger, IViolationService violationService, IExamService examService)
        {
            _logger = logger;
            _violationService = violationService;
            _examService = examService;
        }
        [Authorize(Roles = SystemRoles.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _violationService.GetAllAsync();
            return result.ToActionResult();
        }
        [TypeFilter(typeof(ExamUserAuthorize))]
        [HttpGet("by-examuser/{id}")]
        public async Task<IActionResult> GetAllByExamUserIdAsync(Guid id)
        {
            var result = await _violationService.GetAllByExamUserIdAsync(id);
            return result.ToActionResult();
        }

        [TypeFilter(typeof(ViolationOwnerOrAdminAuthorize))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _violationService.GetByIdAsync(id);
            return result.ToActionResult();
        }
        [TypeFilter(typeof(ExamUserAuthorize))]
        [HttpPost("exam-user/{id}")]
        public async Task<IActionResult> Create(Guid id, [FromBody] CreateViolationDto violationDto)
        {
            var result = await _violationService.CreateAsync(violationDto);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            if (violationDto.IsCritical)
            {
                var blockingResult = await _examService.BlockExamUserByIdAsync(id);
                if (!blockingResult.Success)
                {
                    return BadRequest(blockingResult.ErrorMessage);
                }
            }
            return Ok(result.Data);

        }
    }
}