using ExamSystem.Application.DTOs.User;
using ExamSystem.Application.Common.Models;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ServiceOperationResult<AuthResponseDto>> LoginAsync(LoginUserDto loginDto);
        Task<ServiceOperationResult<AuthResponseDto>> RegisterAsync(RegisterUserDto registerDto);
        Task<ServiceOperationResult<bool>> ValidateTokenAsync(string token);
    }
}