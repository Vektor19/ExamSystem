using Microsoft.AspNetCore.Mvc;
using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Common;
using Microsoft.AspNetCore.Authorization;
using ExamSystem.Application.DTOs.User;
using ExamSystem.API.Extensions;

namespace ExamSystem.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        [Authorize(Roles = SystemRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateByAdmin([FromBody] CreateUserByAdminDto userDto)
        {
            var result = await _userService.CreateUserByAdminAsync(userDto);
            return result.ToActionResult();
        }
        [Authorize(Roles = SystemRoles.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllAsync();
            return result.ToActionResult();
        }
        [SelfOrAdminAuthorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _userService.GetByIdAsync(id);
            return result.ToActionResult();
        }
        [SelfOrAdminAuthorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto updateDto)
        {
            var result = await _userService.UpdateAsync(id, updateDto);
            return result.ToActionResult();
        }
        [SelfOrAdminAuthorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.DeleteAsync(id);
            return result.ToActionResult();
        }

        [TypeFilter(typeof(ExamOwnerOrAdminAuthorize))]
        [HttpGet("by-exam/{id}")]
        public async Task<IActionResult> GetAllByParticipantIdAsync(Guid id)
        {
            var result = await _userService.GetParticipantsByExamIdAsync(id);
            return result.ToActionResult();
        }
    }
}
