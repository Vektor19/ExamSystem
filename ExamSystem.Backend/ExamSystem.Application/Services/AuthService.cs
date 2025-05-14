using ExamSystem.Application.DTOs.User;
using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Common;

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
            if (!isValid)
                return ServiceOperationResult<AuthResponseDto>.Fail("Invalid email or password");
            var result = await _userService.GetByEmailAsync(loginDto.Email);
            if (!result.Success)
                return ServiceOperationResult<AuthResponseDto>.Fail(result.ErrorMessage!);
            var tokenResult = _jwtService.GenerateToken(result.Data!);
            if (string.IsNullOrEmpty(tokenResult.Token))
                return ServiceOperationResult<AuthResponseDto>.Fail("Invalid email or password");
            return ServiceOperationResult<AuthResponseDto>.Ok(new AuthResponseDto { Success = true, AccessToken = tokenResult.Token, Expiration = tokenResult.Expiration });
        }

        public async Task<ServiceOperationResult<AuthResponseDto>> RegisterAsync(RegisterUserDto registerDto)
        {
            var existingUserResult = await _userService.GetByEmailAsync(registerDto.Email);
            if (existingUserResult.Success || existingUserResult.Data != null)
            {
                return ServiceOperationResult<AuthResponseDto>.Fail(existingUserResult.ErrorMessage!);
            }

            var isCreatedResult = await _userService.CreateUserAsync(registerDto);
            if (!isCreatedResult.Success)
            {
                return ServiceOperationResult<AuthResponseDto>.Fail("Failed to register user");
            }
            var result = await _userService.GetByEmailAsync(registerDto.Email);
            if (!result.Success)
                return ServiceOperationResult<AuthResponseDto>.Fail(result.ErrorMessage!);
            var user = result.Data!;
            var token = _jwtService.GenerateToken(user);

            var tokenResult = _jwtService.GenerateToken(user);
            if (string.IsNullOrEmpty(tokenResult.Token))
                return ServiceOperationResult<AuthResponseDto>.Fail("Failed to register user");
            return ServiceOperationResult<AuthResponseDto>.Ok(new AuthResponseDto { Success = true, AccessToken = tokenResult.Token, Expiration = tokenResult.Expiration });
        }
        public async Task<ServiceOperationResult<bool>> ValidateTokenAsync(string token)
        {
            bool isValid = _jwtService.ValidateToken(token);
            if (!isValid)
                return ServiceOperationResult<bool>.Fail("Invalid token");
            return ServiceOperationResult<bool>.Ok(true);
        }
    }
}