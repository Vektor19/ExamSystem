using ExamSystem.Application.Common.Enums;
using ExamSystem.Application.Common.Models;
using ExamSystem.Application.DTOs.User;
using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Application.Services;
using Moq;
using Xunit;

namespace ExamSystem.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserService> _userServiceMock = new();
        private readonly Mock<IJwtService> _jwtServiceMock = new();
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _authService = new AuthService(_userServiceMock.Object, _jwtServiceMock.Object);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnUnauthorized_IfCredentialsInvalid()
        {
            var dto = new LoginUserDto { Email = "bad@example.com", Password = "wrong" };
            _userServiceMock.Setup(s => s.ValidateCredentialsAsync(dto.Email, dto.Password))
                .ReturnsAsync(ServiceOperationResult.Fail("Invalid", ServiceOperationErrorType.Unauthorized));
            _userServiceMock.Setup(s => s.GetByEmailAsync(dto.Email))
                .ReturnsAsync(ServiceOperationResult<UserDto>.Fail("Not found", ServiceOperationErrorType.NotFound));

            var result = await _authService.LoginAsync(dto);

            Assert.False(result.Success);
            Assert.Equal(ServiceOperationErrorType.Unauthorized, result.ErrorType);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnToken_IfCredentialsValid()
        {
            var dto = new LoginUserDto { Email = "user@example.com", Password = "pass" };
            var userDto = new UserDto { Email = dto.Email, UserId = Guid.NewGuid() };


            _userServiceMock.Setup(s => s.ValidateCredentialsAsync(dto.Email, dto.Password))
               .ReturnsAsync(ServiceOperationResult.Ok());
            _userServiceMock.Setup(s => s.GetByEmailAsync(dto.Email))
                .ReturnsAsync(ServiceOperationResult<UserDto>.Ok(userDto));
            _jwtServiceMock.Setup(s => s.GenerateToken(userDto))
                .Returns(new TokenResult { Token = "valid_token", Expiration = DateTime.Now.AddHours(1) });

            var result = await _authService.LoginAsync(dto);

            Assert.True(result.Success);
            Assert.Equal("valid_token", result.Data!.AccessToken);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnConflict_IfUserAlreadyExists()
        {
            var dto = new RegisterUserDto { Email = "existing@example.com" };
            var userDto = new UserDto { Email = dto.Email };

            _userServiceMock.Setup(s => s.GetByEmailAsync(dto.Email))
                .ReturnsAsync(ServiceOperationResult<UserDto>.Ok(userDto));

            var result = await _authService.RegisterAsync(dto);

            Assert.False(result.Success);
            Assert.Equal(ServiceOperationErrorType.Conflict, result.ErrorType);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnInternal_IfCreateFails()
        {
            var dto = new RegisterUserDto { Email = "new@example.com" };

            _userServiceMock.Setup(s => s.GetByEmailAsync(dto.Email))
                .ReturnsAsync(ServiceOperationResult<UserDto>.Fail("Not found", ServiceOperationErrorType.NotFound));
            _userServiceMock.Setup(s => s.CreateUserAsync(dto))
                .ReturnsAsync(ServiceOperationResult.Ok());
            _userServiceMock.Setup(s => s.GetByEmailAsync(dto.Email))
                .ReturnsAsync(ServiceOperationResult<UserDto>.Fail("Failed", ServiceOperationErrorType.Internal));

            var result = await _authService.RegisterAsync(dto);

            Assert.False(result.Success);
            Assert.Equal(ServiceOperationErrorType.Internal, result.ErrorType);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnToken_WhenSuccess()
        {
            var dto = new RegisterUserDto { Email = "new@example.com" };
            var userDto = new UserDto { Email = dto.Email, UserId = Guid.NewGuid() };

            _userServiceMock.SetupSequence(s => s.GetByEmailAsync(dto.Email))
                .ReturnsAsync(ServiceOperationResult<UserDto>.Fail("Not found", ServiceOperationErrorType.NotFound))
                .ReturnsAsync(ServiceOperationResult<UserDto>.Ok(userDto));
            _userServiceMock.Setup(s => s.CreateUserAsync(dto))
                .ReturnsAsync(ServiceOperationResult.Ok());
            _jwtServiceMock.Setup(s => s.GenerateToken(userDto))
                .Returns(new TokenResult { Token = "registered_token", Expiration = DateTime.Now.AddHours(1) });

            var result = await _authService.RegisterAsync(dto);

            Assert.True(result.Success);
            Assert.Equal("registered_token", result.Data!.AccessToken);
        }

        [Fact]
        public async Task ValidateTokenAsync_ShouldReturnFalse_IfTokenInvalid()
        {
            _jwtServiceMock.Setup(s => s.ValidateToken("bad_token")).Returns(false);

            var result = await _authService.ValidateTokenAsync("bad_token");

            Assert.False(result.Success);
            Assert.Equal(ServiceOperationErrorType.Unauthorized, result.ErrorType);
        }

        [Fact]
        public async Task ValidateTokenAsync_ShouldReturnTrue_IfTokenValid()
        {
            _jwtServiceMock.Setup(s => s.ValidateToken("good_token")).Returns(true);

            var result = await _authService.ValidateTokenAsync("good_token");

            Assert.True(result.Success);
            Assert.True(result.Data);
        }
    }
}
