using Microsoft.EntityFrameworkCore;
using ExamSystem.Core.Entities;
using ExamSystem.Persistence.Repositories;
using ExamSystem.Persistence;

namespace ExamSystem.Tests.Repositories
{
    public class ViolationRepositoryTests
    {
        private async Task<ExamSystemDbContext> GetDbContextWithData()
        {
            var options = new DbContextOptionsBuilder<ExamSystemDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ExamSystemDbContext(options);

            var examUser = new ExamUser
            {
                ExamUserId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ExamId = Guid.NewGuid()
            };

            var violations = new List<Violation>
            {
                new Violation { ViolationId = Guid.NewGuid(), ExamUserId = examUser.ExamUserId, Description = "Cheating", ExamUser = examUser },
                new Violation { ViolationId = Guid.NewGuid(), ExamUserId = examUser.ExamUserId, Description = "Talking", ExamUser = examUser }
            };

            await context.ExamUsers.AddAsync(examUser);
            await context.Violations.AddRangeAsync(violations);
            await context.SaveChangesAsync();

            return context;
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllViolations()
        {
            var context = await GetDbContextWithData();
            var repo = new ViolationRepository(context);

            var result = await repo.GetAllAsync();

            Assert.True(result.Success);
            Assert.Equal(2, result.Data!.Count());
            Assert.All(result.Data!, v => Assert.NotNull(v.ExamUser));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsViolation()
        {
            var context = await GetDbContextWithData();
            var violation = context.Violations.First();
            var repo = new ViolationRepository(context);

            var result = await repo.GetByIdAsync(violation.ViolationId);

            Assert.True(result.Success);
            Assert.Equal(violation.Description, result.Data!.Description);
            Assert.NotNull(result.Data.ExamUser);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsFail_WhenNotFound()
        {
            var context = await GetDbContextWithData();
            var repo = new ViolationRepository(context);

            var result = await repo.GetByIdAsync(Guid.NewGuid());

            Assert.False(result.Success);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task GetAllByExamUserIdAsync_ReturnsViolationsForExamUser()
        {
            var context = await GetDbContextWithData();
            var examUserId = context.ExamUsers.First().ExamUserId;
            var repo = new ViolationRepository(context);

            var result = await repo.GetAllByExamUserIdAsync(examUserId);

            Assert.True(result.Success);
            Assert.Equal(2, result.Data!.Count());
            Assert.All(result.Data!, v => Assert.Equal(examUserId, v.ExamUserId));
        }

        [Fact]
        public async Task AddAsync_AddsViolation()
        {
            var context = await GetDbContextWithData();
            var examUser = context.ExamUsers.First();
            var repo = new ViolationRepository(context);

            var newViolation = new Violation
            {
                ViolationId = Guid.NewGuid(),
                ExamUserId = examUser.ExamUserId,
                Description = "Looking around"
            };

            var result = await repo.AddAsync(newViolation);

            Assert.True(result.Success);
            Assert.Contains(context.Violations, v => v.Description == "Looking around");
        }

        [Fact]
        public async Task UpdateAsync_UpdatesViolation()
        {
            var context = await GetDbContextWithData();
            var violation = context.Violations.First();
            violation.Description = "Updated";

            var repo = new ViolationRepository(context);
            var result = await repo.UpdateAsync(violation);

            Assert.True(result.Success);
            Assert.Equal("Updated", context.Violations.First(v => v.ViolationId == violation.ViolationId).Description);
        }

        [Fact]
        public async Task DeleteAsync_DeletesViolation()
        {
            var context = await GetDbContextWithData();
            var violation = context.Violations.First();

            var repo = new ViolationRepository(context);
            var result = await repo.DeleteAsync(violation.ViolationId);

            Assert.True(result.Success);
            Assert.DoesNotContain(context.Violations, v => v.ViolationId == violation.ViolationId);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFail_WhenNotFound()
        {
            var context = await GetDbContextWithData();
            var repo = new ViolationRepository(context);

            var result = await repo.DeleteAsync(Guid.NewGuid());

            Assert.False(result.Success);
        }
    }
}
