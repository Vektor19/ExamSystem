using AutoMapper;
using ExamSystem.Application.Common.Enums;
using ExamSystem.Application.Common.Models;
using ExamSystem.Application.DTOs.User;
using ExamSystem.Application.Services;
using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Interfaces.Repositories;
using ExamSystem.Core.Interfaces.Security;
using Moq;

namespace ExamSystem.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
        private readonly Mock<IRoleRepository> _roleRepoMock = new();
        private readonly Mock<IExamRepository> _examRepoMock = new();
        private readonly UserService _service;

        public UserServiceTests()
        {
            _service = new UserService(
                _userRepoMock.Object,
                _mapperMock.Object,
                _passwordHasherMock.Object,
                _roleRepoMock.Object,
                _examRepoMock.Object);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnConflict_IfUserExists()
        {
            var dto = new RegisterUserDto { Email = "existing@example.com" };
            _userRepoMock.Setup(r => r.GetByEmailAsync(dto.Email))
                .ReturnsAsync(RepositoryOperationResult<User>.Ok(new User()));

            var result = await _service.CreateUserAsync(dto);

            Assert.False(result.Success);
            Assert.Equal(ServiceOperationErrorType.Conflict, result.ErrorType);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnInternal_IfRolesMissing()
        {
            var dto = new RegisterUserDto { Email = "new@example.com" };
            _userRepoMock.Setup(r => r.GetByEmailAsync(dto.Email))
                .ReturnsAsync(RepositoryOperationResult<User>.Fail("not found"));
            _roleRepoMock.Setup(r => r.GetRolesByNamesAsync(It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(RepositoryOperationResult<IEnumerable<Role>>.Fail("No roles"));

            var result = await _service.CreateUserAsync(dto);

            Assert.False(result.Success);
            Assert.Equal(ServiceOperationErrorType.Internal, result.ErrorType);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnOk_WhenUserDeleted()
        {
            var id = Guid.NewGuid();
            _userRepoMock.Setup(r => r.DeleteAsync(id))
                .ReturnsAsync(RepositoryOperationResult.Ok());

            var result = await _service.DeleteAsync(id);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnUserDtos()
        {
            var users = new List<User> { new User { Email = "user@example.com" } };
            var dtos = new List<UserDto> { new UserDto { Email = "user@example.com" } };

            _userRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(RepositoryOperationResult<IEnumerable<User>>.Ok(users));
            _mapperMock.Setup(m => m.Map<IEnumerable<UserDto>>(users)).Returns(dtos);

            var result = await _service.GetAllAsync();

            Assert.True(result.Success);
            Assert.Single(result.Data!);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnNotFound_WhenMissing()
        {
            _userRepoMock.Setup(r => r.GetByEmailAsync("notfound@example.com"))
                .ReturnsAsync(RepositoryOperationResult<User>.Fail("not found"));

            var result = await _service.GetByEmailAsync("notfound@example.com");

            Assert.False(result.Success);
            Assert.Equal(ServiceOperationErrorType.NotFound, result.ErrorType);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnOk_WhenUpdated()
        {
            var id = Guid.NewGuid();
            var user = new User { UserId = id };
            var dto = new UpdateUserDto
            {
                FirstName = "Updated",
                LastName = "User",
                Email = "updated@example.com"
            };

            _userRepoMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(RepositoryOperationResult<User>.Ok(user));
            _userRepoMock.Setup(r => r.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(RepositoryOperationResult.Ok());

            var result = await _service.UpdateAsync(id, dto);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task ValidateCredentialsAsync_ShouldReturnUnauthorized_WhenPasswordInvalid()
        {
            var user = new User { PasswordHash = "hashed" };
            _userRepoMock.Setup(r => r.GetByEmailAsync("test@example.com"))
                .ReturnsAsync(RepositoryOperationResult<User>.Ok(user));
            _passwordHasherMock.Setup(h => h.VerifyPassword("wrong", "hashed"))
                .Returns(false);

            var result = await _service.ValidateCredentialsAsync("test@example.com", "wrong");

            Assert.False(result.Success);
            Assert.Equal(ServiceOperationErrorType.Unauthorized, result.ErrorType);
        }
        [Fact]
        public async Task GetParticipantsByExamIdAsync_ShouldReturnNotFound_IfExamMissing()
        {
            _examRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(RepositoryOperationResult<Exam>.Fail("not found"));

            var result = await _service.GetParticipantsByExamIdAsync(Guid.NewGuid());

            Assert.False(result.Success);
            Assert.Equal(ServiceOperationErrorType.NotFound, result.ErrorType);
        }
    }
}
