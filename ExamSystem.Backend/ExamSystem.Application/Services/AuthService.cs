using ExamSystem.Application.DTOs;
using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Common;
using ExamSystem.Core.Interfaces;

namespace ExamSystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        public AuthService(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }
        public async Task<OperationResult<AuthResponseDto>> LoginAsync(LoginUserDto loginDto)
        {
            bool isValid = await _userService.ValidateCredentialsAsync(loginDto.Email, loginDto.Password);
            if (!isValid)
                return OperationResult<AuthResponseDto>.Fail("Invalid email or password");
            var user = await _userService.GetByEmailAsync(loginDto.Email);
            var tokenResult = _jwtService.GenerateToken(user);
            if (string.IsNullOrEmpty(tokenResult.Token))
                return OperationResult<AuthResponseDto>.Fail("Invalid email or password");
            return OperationResult<AuthResponseDto>.Ok(new AuthResponseDto { Success = true, AccessToken = tokenResult.Token, Expiration = tokenResult.Expiration });
        }

        public async Task<OperationResult<AuthResponseDto>> RegisterAsync(RegisterUserDto registerDto)
        {
            var existingUser = await _userService.GetByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return OperationResult<AuthResponseDto>.Fail("User with this email already exists");
            }

            var isCreated = await _userService.CreateUserAsync(registerDto);
            if (!isCreated)
            {
                return OperationResult<AuthResponseDto>.Fail("Failed to register user");
            }
            var user = await _userService.GetByEmailAsync(registerDto.Email);
            var token = _jwtService.GenerateToken(user);

            var tokenResult = _jwtService.GenerateToken(user);
            if (string.IsNullOrEmpty(tokenResult.Token))
                return OperationResult<AuthResponseDto>.Fail("Failed to register user");
            return OperationResult<AuthResponseDto>.Ok(new AuthResponseDto { Success = true, AccessToken = tokenResult.Token, Expiration = tokenResult.Expiration });
        }
    }
}