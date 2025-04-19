using ExamSystem.Application.DTOs;
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
        public async Task<OperationResult<AuthResponseDto>> LoginAsync(LoginUserDto loginDto)
        {
            bool isValid = (await _userService.ValidateCredentialsAsync(loginDto.Email, loginDto.Password)).Success;
            if (!isValid)
                return OperationResult<AuthResponseDto>.Fail("Invalid email or password");
            var result = await _userService.GetByEmailAsync(loginDto.Email);
            if (!result.Success)
                return OperationResult<AuthResponseDto>.Fail(result.ErrorMessage!);
            var tokenResult = _jwtService.GenerateToken(result.Data!);
            if (string.IsNullOrEmpty(tokenResult.Token))
                return OperationResult<AuthResponseDto>.Fail("Invalid email or password");
            return OperationResult<AuthResponseDto>.Ok(new AuthResponseDto { Success = true, AccessToken = tokenResult.Token, Expiration = tokenResult.Expiration });
        }

        public async Task<OperationResult<AuthResponseDto>> RegisterAsync(RegisterUserDto registerDto)
        {
            var existingUserResult = await _userService.GetByEmailAsync(registerDto.Email);
            if (existingUserResult.Success || existingUserResult.Data != null)
            {
                return OperationResult<AuthResponseDto>.Fail(existingUserResult.ErrorMessage!);
            }

            var isCreatedResult = await _userService.CreateUserAsync(registerDto);
            if (!isCreatedResult.Success)
            {
                return OperationResult<AuthResponseDto>.Fail("Failed to register user");
            }
            var result = await _userService.GetByEmailAsync(registerDto.Email);
            if (!result.Success)
                return OperationResult<AuthResponseDto>.Fail(result.ErrorMessage!);
            var user = result.Data!;
            var token = _jwtService.GenerateToken(user);

            var tokenResult = _jwtService.GenerateToken(user);
            if (string.IsNullOrEmpty(tokenResult.Token))
                return OperationResult<AuthResponseDto>.Fail("Failed to register user");
            return OperationResult<AuthResponseDto>.Ok(new AuthResponseDto { Success = true, AccessToken = tokenResult.Token, Expiration = tokenResult.Expiration });
        }
    }
}