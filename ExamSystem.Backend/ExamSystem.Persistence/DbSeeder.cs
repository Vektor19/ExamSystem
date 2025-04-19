using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Interfaces.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExamSystem.Persistence
{
    public class DbSeeder
    {
        private readonly ExamSystemDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher _passwordHasher;

        public DbSeeder(ExamSystemDbContext dbContext, IConfiguration configuration, IPasswordHasher passwordHasher)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
        }

        public async Task SeedAsync()
        {
            var adminRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == SystemRoles.Admin);
            if (adminRole == null)
                throw new InvalidOperationException("Admin role not found in database.");

            bool adminExists = await _dbContext.UserRoles.AnyAsync(ur => ur.RoleId == adminRole.RoleId);

            if (!adminExists)
            {
                var adminEmail = _configuration["Admin:Email"];
                var adminPassword = _configuration["Admin:Password"];

                if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
                {
                    throw new InvalidOperationException("Admin email and password must be set in configuration.");
                }

                var admin = new User
                {
                    UserId = Guid.NewGuid(),
                    FirstName = "System",
                    LastName = "Administrator",
                    Email = adminEmail,
                    PasswordHash = _passwordHasher.HashPassword(adminPassword),
                };
                
                var adminUserRole = new UserRole
                {
                    UserRoleId = Guid.NewGuid(),
                    UserId = admin.UserId,
                    RoleId = adminRole.RoleId
                };

                admin.UserRoles.Add(adminUserRole);

                await _dbContext.Users.AddAsync(admin);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
