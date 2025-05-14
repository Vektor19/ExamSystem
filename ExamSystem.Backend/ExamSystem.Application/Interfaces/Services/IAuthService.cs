using ExamSystem.Application.DTOs.User;
using ExamSystem.Core.Common;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<RepositoryOperationResult<AuthResponseDto>> LoginAsync(LoginUserDto loginDto);
        Task<RepositoryOperationResult<AuthResponseDto>> RegisterAsync(RegisterUserDto registerDto);
        Task<RepositoryOperationResult<bool>> ValidateTokenAsync(string token);
    }
}