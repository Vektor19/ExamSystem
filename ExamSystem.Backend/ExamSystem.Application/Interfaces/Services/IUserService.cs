using ExamSystem.Application.DTOs.User;
using ExamSystem.Core.Common;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<ServiceOperationResult<UserDto>> GetByIdAsync(Guid id);
        Task<ServiceOperationResult<IEnumerable<UserDto>>> GetAllAsync();
        Task<ServiceOperationResult<UserDto>> GetByEmailAsync(string email);
        Task<ServiceOperationResult> UpdateAsync(Guid userId, UpdateUserDto updateDto);
        Task<ServiceOperationResult> DeleteAsync(Guid id);
        Task<ServiceOperationResult> ValidateCredentialsAsync(string email, string password);
        Task<ServiceOperationResult> CreateUserAsync(RegisterUserDto userDto);
        Task<ServiceOperationResult> CreateUserByAdminAsync(CreateUserByAdminDto userDto);
        Task<ServiceOperationResult<IEnumerable<UserDto>>> GetParticipantsByExamIdAsync(Guid examId);
    }
}
