using Microsoft.AspNetCore.Mvc;
using ExamSystem.Application.DTOs;
using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Common;

namespace ExamSystem.API.Controllers
{
    [ApiController]
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

        [HttpPost("create-by-admin")]
        public async Task<IActionResult> CreateByAdmin([FromBody] CreateUserByAdminDto userDto)
        {
            var result = await _userService.CreateUserByAdminAsync(userDto);
            return result.Success ? Ok(result) : BadRequest(result.ErrorMessage);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllAsync();
            return result.Success ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _userService.GetByIdAsync(id);
            return result.Success ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto updateDto)
        {
            var result = await _userService.UpdateAsync(id, updateDto);
            return result.Success ? Ok(result) : BadRequest(result.ErrorMessage);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.DeleteAsync(id);
            return result.Success ? Ok(result) : BadRequest(result.ErrorMessage);
        }
    }
}
