using ExamSystem.Application.DTOs.User;
using ExamSystem.Core.Common;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<RepositoryOperationResult<UserDto>> GetByIdAsync(Guid id);
        Task<RepositoryOperationResult<IEnumerable<UserDto>>> GetAllAsync();
        Task<RepositoryOperationResult<UserDto>> GetByEmailAsync(string email);
        Task<RepositoryOperationResult> UpdateAsync(Guid userId, UpdateUserDto updateDto);
        Task<RepositoryOperationResult> DeleteAsync(Guid id);
        Task<RepositoryOperationResult> ValidateCredentialsAsync(string email, string password);
        Task<RepositoryOperationResult> CreateUserAsync(RegisterUserDto userDto);
        Task<RepositoryOperationResult> CreateUserByAdminAsync(CreateUserByAdminDto userDto);
        Task<RepositoryOperationResult<IEnumerable<UserDto>>> GetParticipantsByExamIdAsync(Guid examId);
    }
}
