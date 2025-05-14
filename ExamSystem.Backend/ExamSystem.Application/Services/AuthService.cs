using ExamSystem.Application.DTOs.User;
using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Application.Common.Models;
using ExamSystem.Application.Common.Enums;

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
        public async Task<ServiceOperationResult<AuthResponseDto>> LoginAsync(LoginUserDto loginDto)
        {
            bool isValid = (await _userService.ValidateCredentialsAsync(loginDto.Email, loginDto.Password)).Success;
            var result = await _userService.GetByEmailAsync(loginDto.Email);
            if (!isValid || !result.Success)
                return ServiceOperationResult<AuthResponseDto>.Fail("Invalid email or password", ServiceOperationErrorType.Unauthorized);
            
            var tokenResult = _jwtService.GenerateToken(result.Data!);
            if (string.IsNullOrEmpty(tokenResult.Token))
                return ServiceOperationResult<AuthResponseDto>.Fail("Invalid email or password", ServiceOperationErrorType.Unauthorized);
            return ServiceOperationResult<AuthResponseDto>.Ok(new AuthResponseDto { Success = true, AccessToken = tokenResult.Token, Expiration = tokenResult.Expiration });
        }

        public async Task<ServiceOperationResult<AuthResponseDto>> RegisterAsync(RegisterUserDto registerDto)
        {
            var existingUserResult = await _userService.GetByEmailAsync(registerDto.Email);
            if (existingUserResult.Success || existingUserResult.Data != null)
            {
                return ServiceOperationResult<AuthResponseDto>.Fail(existingUserResult.ErrorMessage!, ServiceOperationErrorType.Conflict);
            }

            var isCreatedResult = await _userService.CreateUserAsync(registerDto);
            if (!isCreatedResult.Success)
            {
                return ServiceOperationResult<AuthResponseDto>.Fail("Failed to register user", ServiceOperationErrorType.Internal);
            }
            var result = await _userService.GetByEmailAsync(registerDto.Email);
            if (!result.Success)
                return ServiceOperationResult<AuthResponseDto>.Fail(result.ErrorMessage!, ServiceOperationErrorType.Internal);
            var user = result.Data!;
            var token = _jwtService.GenerateToken(user);

            var tokenResult = _jwtService.GenerateToken(user);
            if (string.IsNullOrEmpty(tokenResult.Token))
                return ServiceOperationResult<AuthResponseDto>.Fail("Failed to register user", ServiceOperationErrorType.Internal);
            return ServiceOperationResult<AuthResponseDto>.Ok(new AuthResponseDto { Success = true, AccessToken = tokenResult.Token, Expiration = tokenResult.Expiration });
        }
        public async Task<ServiceOperationResult<bool>> ValidateTokenAsync(string token)
        {
            bool isValid = _jwtService.ValidateToken(token);
            if (!isValid)
                return ServiceOperationResult<bool>.Fail("Invalid token", ServiceOperationErrorType.Unauthorized);
            return ServiceOperationResult<bool>.Ok(true);
        }
    }
}