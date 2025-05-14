using AutoMapper;
using ExamSystem.Application.DTOs.User;
using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Interfaces.Repositories;
using ExamSystem.Core.Interfaces.Security;

namespace ExamSystem.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRoleRepository _roleRepository;
        private readonly IExamRepository _examRepository;
        public UserService(IUserRepository userRepository, IMapper mapper, IPasswordHasher passwordHasher, IRoleRepository roleRepository, IExamRepository examRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _examRepository = examRepository;
        }

        public async Task<ServiceOperationResult> CreateUserAsync(RegisterUserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Email) || string.IsNullOrWhiteSpace(userDto.Password))
                return ServiceOperationResult.Fail("Email and password are required.");

            var existingUser = await _userRepository.GetByEmailAsync(userDto.Email);
            if (existingUser.Success && existingUser.Data != null)
                return ServiceOperationResult.Fail("User with this email already exists.");

            var rolesFromDb = await _roleRepository.GetRolesByNamesAsync([SystemRoles.Student, SystemRoles.Examinator]);

            if (!rolesFromDb.Success || rolesFromDb.Data == null || !rolesFromDb.Data.Any())
                return ServiceOperationResult.Fail("Can't create user");

            var user = _mapper.Map<User>(userDto);
            user.UserId = Guid.NewGuid();
            user.PasswordHash = _passwordHasher.HashPassword(userDto.Password);

            user.UserRoles = rolesFromDb.Data.Select(role => new UserRole
            {
                UserRoleId = Guid.NewGuid(),
                RoleId = role.RoleId,
                User = user
            }).ToList();

            var result = await _userRepository.AddAsync(user);
            return result.Success
                ? ServiceOperationResult.Ok()
                : ServiceOperationResult.Fail("Failed to create user.");
        }


        public async Task<ServiceOperationResult> DeleteAsync(Guid id)
        {
            var result = await _userRepository.DeleteAsync(id);
            return result.Success
                ? ServiceOperationResult.Ok()
                : ServiceOperationResult.Fail("Failed to delete user.");
        }

        public async Task<ServiceOperationResult<IEnumerable<UserDto>>> GetAllAsync()
        {
            var result = await _userRepository.GetAllAsync();
            if (!result.Success)
                return ServiceOperationResult<IEnumerable<UserDto>>.Fail(result.ErrorMessage!);
            if (!result.Data!.Any())
                return ServiceOperationResult<IEnumerable<UserDto>>.Fail("No users found.");

            var userDtos = _mapper.Map<IEnumerable<UserDto>>(result.Data);
            return ServiceOperationResult<IEnumerable<UserDto>>.Ok(userDtos);
        }

        public async Task<ServiceOperationResult<UserDto>> GetByEmailAsync(string email)
        {
            var result = await _userRepository.GetByEmailAsync(email);
            if (!result.Success)
                return ServiceOperationResult<UserDto>.Fail(result.ErrorMessage!);

            var userDto = _mapper.Map<UserDto>(result.Data);
            return ServiceOperationResult<UserDto>.Ok(userDto);
        }

        public async Task<ServiceOperationResult<UserDto>> GetByIdAsync(Guid id)
        {
            var result = await _userRepository.GetByIdAsync(id);
            if (!result.Success)
                return ServiceOperationResult<UserDto>.Fail(result.ErrorMessage!);

            var userDto = _mapper.Map<UserDto>(result.Data);
            return ServiceOperationResult<UserDto>.Ok(userDto);
        }

        public async Task<ServiceOperationResult> UpdateAsync(Guid userId, UpdateUserDto updateDto)
        {
            var existingUserResult = await _userRepository.GetByIdAsync(userId);
            if (!existingUserResult.Success || existingUserResult.Data == null)
                return ServiceOperationResult.Fail("User not found.");

            var user = existingUserResult.Data;

            user.FirstName = updateDto.FirstName;
            user.LastName = updateDto.LastName;
            user.Email = updateDto.Email;

            var updateResult = await _userRepository.UpdateAsync(user);
            return updateResult.Success
                ? ServiceOperationResult.Ok()
                : ServiceOperationResult.Fail("Failed to update user.");
        }

        public async Task<ServiceOperationResult> ValidateCredentialsAsync(string email, string password)
        {
            var result = await _userRepository.GetByEmailAsync(email);
            if (!result.Success || result.Data == null)
                return ServiceOperationResult.Fail("User not found.");

            var isValid = _passwordHasher.VerifyPassword(password, result.Data.PasswordHash);
            return isValid
                ? ServiceOperationResult.Ok()
                : ServiceOperationResult.Fail("Invalid credentials.");
        }
        public async Task<ServiceOperationResult> CreateUserByAdminAsync(CreateUserByAdminDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Email) || string.IsNullOrWhiteSpace(userDto.Password))
                return ServiceOperationResult.Fail("Email and password are required.");

            var existingUser = await _userRepository.GetByEmailAsync(userDto.Email);
            if (existingUser.Success && existingUser.Data != null)
                return ServiceOperationResult.Fail("User with this email already exists.");

            if (userDto.Roles == null || !userDto.Roles.Any())
                return ServiceOperationResult.Fail("At least one role must be specified.");

            var rolesFromDb = await _roleRepository.GetRolesByNamesAsync(userDto.Roles);
            if (!rolesFromDb.Success || rolesFromDb.Data == null || !rolesFromDb.Data.Any())
                return ServiceOperationResult.Fail("Invalid roles specified.");

            var user = _mapper.Map<User>(userDto);
            user.UserId = Guid.NewGuid();
            user.PasswordHash = _passwordHasher.HashPassword(userDto.Password);

            user.UserRoles = rolesFromDb.Data.Select(role => new UserRole
            {
                UserRoleId = Guid.NewGuid(),
                RoleId = role.RoleId,
                User = user
            }).ToList();

            var result = await _userRepository.AddAsync(user);
            return result.Success
                ? ServiceOperationResult.Ok()
                : ServiceOperationResult.Fail("Failed to create user.");
        }
        public async Task<ServiceOperationResult<IEnumerable<UserDto>>> GetParticipantsByExamIdAsync(Guid examId)
        {
            var examResult = await _examRepository.GetByIdAsync(examId);
            if (!examResult.Success || examResult.Data == null)
                return ServiceOperationResult<IEnumerable<UserDto>>.Fail("Exam not found.");

            var exam = examResult.Data;
            var participants = exam.ExamUsers.Select(eu => eu.User).ToList();

            var userDtos = _mapper.Map<IEnumerable<UserDto>>(participants);
            return ServiceOperationResult<IEnumerable<UserDto>>.Ok(userDtos);
        }

    }
}
