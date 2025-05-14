using ExamSystem.Application.DTOs.User;
using ExamSystem.Core.Common;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ServiceOperationResult<AuthResponseDto>> LoginAsync(LoginUserDto loginDto);
        Task<ServiceOperationResult<AuthResponseDto>> RegisterAsync(RegisterUserDto registerDto);
        Task<ServiceOperationResult<bool>> ValidateTokenAsync(string token);
    }
}