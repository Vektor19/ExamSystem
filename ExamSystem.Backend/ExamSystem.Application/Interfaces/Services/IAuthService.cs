using ExamSystem.Application.DTOs;
using ExamSystem.Core.Common;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<OperationResult<AuthResponseDto>> LoginAsync(LoginUserDto loginDto);
        Task<OperationResult<AuthResponseDto>> RegisterAsync(RegisterUserDto registerDto);
    }
}