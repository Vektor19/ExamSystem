using ExamSystem.Application.DTOs;
using System.Security.Claims;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateToken(UserDto user);
        ClaimsPrincipal? GetPrincipalFromToken(string token);
        bool ValidateToken(string token);
    }
}
