using ExamSystem.Application.DTOs;
using ExamSystem.Core.Common;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<OperationResult<UserDto>> GetByIdAsync(Guid id);
        Task<OperationResult<IEnumerable<UserDto>>> GetAllAsync();
        Task<OperationResult<UserDto>> GetByEmailAsync(string email);
        Task<OperationResult> UpdateAsync(Guid userId, UpdateUserDto updateDto);
        Task<OperationResult> DeleteAsync(Guid id);
        Task<OperationResult> ValidateCredentialsAsync(string email, string password);
        Task<OperationResult> CreateUserAsync(RegisterUserDto userDto);
        Task<OperationResult> CreateUserByAdminAsync(CreateUserByAdminDto userDto);
    }
}
