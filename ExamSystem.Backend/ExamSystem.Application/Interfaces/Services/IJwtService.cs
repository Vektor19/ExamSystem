using ExamSystem.Application.DTOs.User;
using ExamSystem.Application.Models;
using System.Security.Claims;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IJwtService
    {
        TokenResult GenerateToken(UserDto user);
        ClaimsPrincipal? GetPrincipalFromToken(string token);
        bool ValidateToken(string token);
    }
}
