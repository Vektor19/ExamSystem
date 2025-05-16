using Microsoft.EntityFrameworkCore;
using ExamSystem.Core.Entities;
using ExamSystem.Persistence.Repositories;
using ExamSystem.Persistence;

namespace ExamSystem.Tests.Repositories
{
    public class RoleRepositoryTests
    {
        private async Task<ExamSystemDbContext> GetDbContextWithData()
        {
            var options = new DbContextOptionsBuilder<ExamSystemDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ExamSystemDbContext(options);

            var roles = new List<Role>
            {
                new Role { RoleId = Guid.NewGuid(), Name = "Admin", IsSystem = true },
                new Role { RoleId = Guid.NewGuid(), Name = "User", IsSystem = false },
                new Role { RoleId = Guid.NewGuid(), Name = "Moderator", IsSystem = false }
            };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();

            return context;
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllRoles()
        {
            var context = await GetDbContextWithData();
            var repo = new RoleRepository(context);

            var result = await repo.GetAllAsync();

            Assert.True(result.Success);
            Assert.Equal(3, result.Data!.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsRole()
        {
            var context = await GetDbContextWithData();
            var role = context.Roles.First();
            var repo = new RoleRepository(context);

            var result = await repo.GetByIdAsync(role.RoleId);

            Assert.True(result.Success);
            Assert.Equal(role.Name, result.Data!.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsFail_IfNotFound()
        {
            var context = await GetDbContextWithData();
            var repo = new RoleRepository(context);

            var result = await repo.GetByIdAsync(Guid.NewGuid());

            Assert.False(result.Success);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task AddAsync_AddsRole()
        {
            var context = await GetDbContextWithData();
            var repo = new RoleRepository(context);

            var newRole = new Role
            {
                RoleId = Guid.NewGuid(),
                Name = "Tester",
                IsSystem = false
            };

            var result = await repo.AddAsync(newRole);

            Assert.True(result.Success);
            Assert.Contains(context.Roles, r => r.Name == "Tester");
        }

        [Fact]
        public async Task UpdateAsync_UpdatesRole()
        {
            var context = await GetDbContextWithData();
            var role = context.Roles.First(r => !r.IsSystem);
            role.Name = "UpdatedRole";
            var repo = new RoleRepository(context);

            var result = await repo.UpdateAsync(role);

            Assert.True(result.Success);
            Assert.Equal("UpdatedRole", context.Roles.First(r => r.RoleId == role.RoleId).Name);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFail_IfRoleNotFound()
        {
            var context = await GetDbContextWithData();
            var repo = new RoleRepository(context);

            var role = new Role
            {
                RoleId = Guid.NewGuid(),
                Name = "Ghost",
                IsSystem = false
            };

            var result = await repo.UpdateAsync(role);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFail_IfSystemRole()
        {
            var context = await GetDbContextWithData();
            var role = context.Roles.First(r => r.IsSystem);
            role.Name = "HackedSystemRole";
            var repo = new RoleRepository(context);

            var result = await repo.UpdateAsync(role);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task DeleteAsync_DeletesRole()
        {
            var context = await GetDbContextWithData();
            var role = context.Roles.First(r => !r.IsSystem);
            var repo = new RoleRepository(context);

            var result = await repo.DeleteAsync(role.RoleId);

            Assert.True(result.Success);
            Assert.DoesNotContain(context.Roles, r => r.RoleId == role.RoleId);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFail_IfNotFound()
        {
            var context = await GetDbContextWithData();
            var repo = new RoleRepository(context);

            var result = await repo.DeleteAsync(Guid.NewGuid());

            Assert.False(result.Success);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFail_IfSystemRole()
        {
            var context = await GetDbContextWithData();
            var role = context.Roles.First(r => r.IsSystem);
            var repo = new RoleRepository(context);

            var result = await repo.DeleteAsync(role.RoleId);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task GetRolesByNamesAsync_ReturnsMatchingRoles()
        {
            var context = await GetDbContextWithData();
            var repo = new RoleRepository(context);

            var names = new[] { "Admin", "User" };
            var result = await repo.GetRolesByNamesAsync(names);

            Assert.True(result.Success);
            Assert.Equal(2, result.Data!.Count());
            Assert.All(result.Data!, r => Assert.Contains(r.Name, names));
        }

        [Fact]
        public async Task GetRolesByNamesAsync_ReturnsEmpty_WhenNoneMatch()
        {
            var context = await GetDbContextWithData();
            var repo = new RoleRepository(context);

            var result = await repo.GetRolesByNamesAsync(new[] { "Nonexistent" });

            Assert.True(result.Success);
            Assert.Empty(result.Data!);
        }
    }
}
