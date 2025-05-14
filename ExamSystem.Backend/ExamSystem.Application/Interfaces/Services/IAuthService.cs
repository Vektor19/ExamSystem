using ExamSystem.Application.DTOs.User;
using ExamSystem.Core.Common;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<OperationResult<AuthResponseDto>> LoginAsync(LoginUserDto loginDto);
        Task<OperationResult<AuthResponseDto>> RegisterAsync(RegisterUserDto registerDto);
        Task<OperationResult<bool>> ValidateTokenAsync(string token);
    }
}