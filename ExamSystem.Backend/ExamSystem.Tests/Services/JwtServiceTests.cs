using ExamSystem.Application.Common.Models;
using ExamSystem.Application.DTOs.User;
using ExamSystem.Application.Services;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ExamSystem.Tests.Services
{
    public class JwtServiceTests
    {
        private readonly JwtSettings _settings = new()
        {
            Secret = "supersecretkey1234567890",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpiryMinutes = 60
        };

        private readonly JwtService _service;

        public JwtServiceTests()
        {
            var options = Options.Create(_settings);
            _service = new JwtService(options);
        }

        [Fact]
        public void GenerateToken_ShouldReturnValidToken()
        {
            var user = new UserDto
            {
                UserId = Guid.NewGuid(),
                Email = "user@example.com",
                Roles = new List<string> { "Admin", "User" }
            };

            var result = _service.GenerateToken(user);

            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.Token));
            Assert.True(result.Expiration > DateTime.Now);
        }

        [Fact]
        public void ValidateToken_ShouldReturnTrue_ForValidToken()
        {
            var user = new UserDto
            {
                UserId = Guid.NewGuid(),
                Email = "valid@example.com",
                Roles = new List<string> { "User" }
            };

            var tokenResult = _service.GenerateToken(user);

            var isValid = _service.ValidateToken(tokenResult.Token);

            Assert.True(isValid);
        }

        [Fact]
        public void ValidateToken_ShouldReturnFalse_ForInvalidToken()
        {
            var invalidToken = "invalid.token.string";

            var isValid = _service.ValidateToken(invalidToken);

            Assert.False(isValid);
        }

        [Fact]
        public void GetPrincipalFromToken_ShouldReturnClaimsPrincipal()
        {
            var user = new UserDto
            {
                UserId = Guid.NewGuid(),
                Email = "claim@example.com",
                Roles = new List<string> { "User" }
            };

            var token = _service.GenerateToken(user).Token;

            var principal = _service.GetPrincipalFromToken(token);

            Assert.NotNull(principal);
            Assert.Equal(user.Email, principal?.FindFirst(JwtRegisteredClaimNames.Email)?.Value);
            Assert.Contains(principal!.Claims, c => c.Type == ClaimTypes.Role && c.Value == "User");
        }

        [Fact]
        public void GetPrincipalFromToken_ShouldReturnNull_ForInvalidToken()
        {
            var principal = _service.GetPrincipalFromToken("bad.token");

            Assert.Null(principal);
        }
    }
}
