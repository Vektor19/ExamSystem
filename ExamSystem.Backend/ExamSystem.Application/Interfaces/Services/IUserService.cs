using ExamSystem.Application.DTOs.User;
using ExamSystem.Core.Common;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<OperationResult<UserDto>> GetByIdAsync(Guid id);
        Task<OperationResult<IEnumerable<UserDto>>> GetAllAsync();
        Task<OperationResult<UserDto>> GetByEmailAsync(string email);
        Task<RepositoryOperationResult> UpdateAsync(Guid userId, UpdateUserDto updateDto);
        Task<RepositoryOperationResult> DeleteAsync(Guid id);
        Task<RepositoryOperationResult> ValidateCredentialsAsync(string email, string password);
        Task<RepositoryOperationResult> CreateUserAsync(RegisterUserDto userDto);
        Task<RepositoryOperationResult> CreateUserByAdminAsync(CreateUserByAdminDto userDto);
        Task<OperationResult<IEnumerable<UserDto>>> GetParticipantsByExamIdAsync(Guid examId);
    }
}
