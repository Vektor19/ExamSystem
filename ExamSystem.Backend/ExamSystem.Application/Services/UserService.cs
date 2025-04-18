using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ExamSystem.Application.DTOs;
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
        public UserService(IUserRepository userRepository, IMapper mapper, IPasswordHasher passwordHasher, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
        }

        public async Task<OperationResult> CreateUserAsync(RegisterUserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Email) || string.IsNullOrWhiteSpace(userDto.Password))
                return OperationResult.Fail("Email and password are required.");

            var existingUser = await _userRepository.GetByEmailAsync(userDto.Email);
            if (existingUser.Success && existingUser.Data != null)
                return OperationResult.Fail("User with this email already exists.");

            var rolesFromDb = await _roleRepository.GetRolesByNamesAsync([SystemRoles.Student, SystemRoles.Examinator]);

            if (!rolesFromDb.Success || rolesFromDb.Data == null || !rolesFromDb.Data.Any())
                return OperationResult.Fail("Can't create user");

            var user = _mapper.Map<User>(userDto);
            user.PasswordHash = _passwordHasher.HashPassword(userDto.Password);

            user.UserRoles = rolesFromDb.Data.Select(role => new UserRole
            {
                UserRoleId = Guid.NewGuid(),
                RoleId = role.RoleId,
                User = user
            }).ToList();

            var result = await _userRepository.AddAsync(user);
            return result.Success
                ? OperationResult.Ok()
                : OperationResult.Fail("Failed to create user.");
        }


        public async Task<OperationResult> DeleteAsync(Guid id)
        {
            var result = await _userRepository.DeleteAsync(id);
            return result.Success
                ? OperationResult.Ok()
                : OperationResult.Fail("Failed to delete user.");
        }

        public async Task<OperationResult<IEnumerable<UserDto>>> GetAllAsync()
        {
            var result = await _userRepository.GetAllAsync();
            if (!result.Success)
                return OperationResult<IEnumerable<UserDto>>.Fail(result.ErrorMessage!);
            if (!result.Data!.Any())
                return OperationResult<IEnumerable<UserDto>>.Fail("No users found.");

            var userDtos = _mapper.Map<IEnumerable<UserDto>>(result.Data);
            return OperationResult<IEnumerable<UserDto>>.Ok(userDtos);
        }

        public async Task<OperationResult<UserDto>> GetByEmailAsync(string email)
        {
            var result = await _userRepository.GetByEmailAsync(email);
            if (!result.Success)
                return OperationResult<UserDto>.Fail(result.ErrorMessage!);

            var userDto = _mapper.Map<UserDto>(result.Data);
            return OperationResult<UserDto>.Ok(userDto);
        }

        public async Task<OperationResult<UserDto>> GetByIdAsync(Guid id)
        {
            var result = await _userRepository.GetByIdAsync(id);
            if (!result.Success)
                return OperationResult<UserDto>.Fail(result.ErrorMessage!);

            var userDto = _mapper.Map<UserDto>(result.Data);
            return OperationResult<UserDto>.Ok(userDto);
        }

        public async Task<OperationResult> UpdateAsync(Guid userId, UpdateUserDto updateDto)
        {
            var existingUserResult = await _userRepository.GetByIdAsync(userId);
            if (!existingUserResult.Success || existingUserResult.Data == null)
                return OperationResult.Fail("User not found.");

            var user = existingUserResult.Data;

            user.FirstName = updateDto.FirstName;
            user.LastName = updateDto.LastName;
            user.Email = updateDto.Email;

            var updateResult = await _userRepository.UpdateAsync(user);
            return updateResult.Success
                ? OperationResult.Ok()
                : OperationResult.Fail("Failed to update user.");
        }

        public async Task<OperationResult> ValidateCredentialsAsync(string email, string password)
        {
            var result = await _userRepository.GetByEmailAsync(email);
            if (!result.Success || result.Data == null)
                return OperationResult.Fail("User not found.");

            var isValid = _passwordHasher.VerifyPassword(password, result.Data.PasswordHash);
            return isValid
                ? OperationResult.Ok()
                : OperationResult.Fail("Invalid credentials.");
        }
    }
}
