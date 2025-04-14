using ExamSystem.Application.DTOs;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginUserDto loginDto);
        Task<AuthResponseDto> RegisterAsync(RegisterUserDto registerDto);
        Task<bool> ValidateUserAsync(LoginUserDto loginDto);
    }
}