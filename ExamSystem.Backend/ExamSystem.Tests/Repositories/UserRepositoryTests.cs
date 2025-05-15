using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;
using ExamSystem.Persistence;
using ExamSystem.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ExamSystem.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private ExamSystemDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ExamSystemDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ExamSystemDbContext(options);
        }

        private User CreateTestUser()
        {
            return new User
            {
                UserId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PasswordHash = "hashed"
            };
        }

        [Fact]
        public async Task AddAsync_ShouldAddUser()
        {
            var context = GetDbContext();
            var repo = new UserRepository(context);
            var user = CreateTestUser();

            var result = await repo.AddAsync(user);

            Assert.True(result.Success);
            Assert.Single(context.Users);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            var context = GetDbContext();
            var repo = new UserRepository(context);

            context.Users.Add(CreateTestUser());
            context.Users.Add(CreateTestUser());
            await context.SaveChangesAsync();

            var result = await repo.GetAllAsync();

            Assert.True(result.Success);
            Assert.Equal(2, result.Data!.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser_WhenExists()
        {
            var context = GetDbContext();
            var repo = new UserRepository(context);
            var user = CreateTestUser();
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var result = await repo.GetByIdAsync(user.UserId);

            Assert.True(result.Success);
            Assert.Equal(user.UserId, result.Data!.UserId);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldFail_WhenNotExists()
        {
            var context = GetDbContext();
            var repo = new UserRepository(context);

            var result = await repo.GetByIdAsync(Guid.NewGuid());

            Assert.False(result.Success);
            Assert.Equal("User not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateUser()
        {
            var context = GetDbContext();
            var repo = new UserRepository(context);
            var user = CreateTestUser();
            context.Users.Add(user);
            await context.SaveChangesAsync();

            user.FirstName = "Jane";
            var result = await repo.UpdateAsync(user);

            Assert.True(result.Success);
            Assert.Equal("Jane", context.Users.First().FirstName);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveUser_WhenExists()
        {
            var context = GetDbContext();
            var repo = new UserRepository(context);
            var user = CreateTestUser();
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var result = await repo.DeleteAsync(user.UserId);

            Assert.True(result.Success);
            Assert.Empty(context.Users);
        }

        [Fact]
        public async Task DeleteAsync_ShouldFail_WhenNotExists()
        {
            var context = GetDbContext();
            var repo = new UserRepository(context);

            var result = await repo.DeleteAsync(Guid.NewGuid());

            Assert.False(result.Success);
            Assert.Equal("User not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnUser_WhenExists()
        {
            var context = GetDbContext();
            var repo = new UserRepository(context);
            var user = CreateTestUser();
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var result = await repo.GetByEmailAsync(user.Email);

            Assert.True(result.Success);
            Assert.Equal(user.Email, result.Data!.Email);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldFail_WhenNotExists()
        {
            var context = GetDbContext();
            var repo = new UserRepository(context);

            var result = await repo.GetByEmailAsync("notfound@example.com");

            Assert.False(result.Success);
            Assert.Equal("User not found.", result.ErrorMessage);
        }
    }
}
