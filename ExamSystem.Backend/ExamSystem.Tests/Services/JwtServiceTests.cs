using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExamSystem.Application.Common.Models;
using ExamSystem.Application.DTOs.User;
using ExamSystem.Application.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Xunit;
namespace ExamSystem.Tests.Services
{
    public class JwtServiceTests
    {
        private readonly JwtService _jwtService;
        private readonly JwtSettings _jwtSettings;

        public JwtServiceTests()
        {
            _jwtSettings = new JwtSettings
            {
                Secret = "super_secret_key_1234567890_ABCDEFG",
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                ExpiryMinutes = 30
            };

            var options = Options.Create(_jwtSettings);
            _jwtService = new JwtService(options);
        }

        [Fact]
        public void GenerateToken_ShouldReturnValidToken_WithExpiration()
        {
            var user = new UserDto
            {
                UserId = Guid.NewGuid(),
                Email = "user@example.com",
                Roles = new List<string> { "Admin", "Student" }
            };

            var result = _jwtService.GenerateToken(user);

            Assert.False(string.IsNullOrEmpty(result.Token));
            Assert.True(result.Expiration > DateTime.Now);
        }

        [Fact]
        public void GetPrincipalFromToken_ShouldReturnPrincipal_ForValidToken()
        {
            var user = new UserDto
            {
                UserId = Guid.NewGuid(),
                Email = "user@example.com",
                Roles = new List<string> { "Admin" }
            };

            var token = _jwtService.GenerateToken(user).Token;

            var principal = _jwtService.GetPrincipalFromToken(token);

            Assert.NotNull(principal);
            Assert.Equal(user.Email, principal!.FindFirst(ClaimTypes.Email)?.Value);
            Assert.Contains(principal.Claims, c => c.Type == ClaimTypes.Role && c.Value == "Admin");
        }

        [Fact]
        public void GetPrincipalFromToken_ShouldReturnNull_ForInvalidToken()
        {
            var invalidToken = "this.is.not.a.valid.token";

            var principal = _jwtService.GetPrincipalFromToken(invalidToken);

            Assert.Null(principal);
        }

        [Fact]
        public void ValidateToken_ShouldReturnTrue_ForValidToken()
        {
            var user = new UserDto
            {
                UserId = Guid.NewGuid(),
                Email = "user@example.com"
            };

            var token = _jwtService.GenerateToken(user).Token;

            var isValid = _jwtService.ValidateToken(token);

            Assert.True(isValid);
        }

        [Fact]
        public void ValidateToken_ShouldReturnFalse_ForInvalidToken()
        {
            var invalidToken = "fake.invalid.token";

            var isValid = _jwtService.ValidateToken(invalidToken);

            Assert.False(isValid);
        }

        [Fact]
        public void GenerateToken_ShouldContainAllClaims()
        {
            var userId = Guid.NewGuid();
            var user = new UserDto
            {
                UserId = userId,
                Email = "claims@test.com",
                Roles = new List<string> { "Student", "Lecturer" }
            };

            var tokenResult = _jwtService.GenerateToken(user);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(tokenResult.Token);

            var claims = jwtToken.Claims.ToList();

            Assert.Contains(claims, c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == userId.ToString());
            Assert.Contains(claims, c => c.Type == JwtRegisteredClaimNames.Email && c.Value == "claims@test.com");
            Assert.Contains(claims, c => c.Type == ClaimTypes.Role && c.Value == "Student");
            Assert.Contains(claims, c => c.Type == ClaimTypes.Role && c.Value == "Lecturer");
        }
    }
}