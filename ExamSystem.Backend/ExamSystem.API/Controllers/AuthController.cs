using Microsoft.AspNetCore.Mvc;
using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Application.DTOs.User;
using ExamSystem.Application.DTOs.Token;
using ExamSystem.API.Extensions;

namespace ExamSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            return result.ToActionResult();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);
            return result.ToActionResult();
        }
        [HttpPost("validate")]
        public async Task<IActionResult> ValidateToken([FromBody] ValidateTokenDto tokenDto)
        {
            var result = await _authService.ValidateTokenAsync(tokenDto.Token);
            return result.ToActionResult();
        }
    }
}
