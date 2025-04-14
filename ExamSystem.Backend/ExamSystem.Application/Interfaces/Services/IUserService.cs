using ExamSystem.Application.DTOs;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(Guid id);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> GetByEmailAsync(string email);
        Task<bool> UpdateAsync(UserDto userDto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ValidateCredentialsAsync(string email, string password);
        Task<bool> CreateUserAsync(RegisterUserDto userDto);
    }
}
