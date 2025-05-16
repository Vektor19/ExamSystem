using Xunit;
using Microsoft.EntityFrameworkCore;
using ExamSystem.Core.Entities;
using ExamSystem.Persistence.Repositories;
using ExamSystem.Core.Common;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ExamSystem.Persistence;

namespace ExamSystem.Tests.Repositories
{
    public class ExamRepositoryTests
    {
        private async Task<ExamSystemDbContext> GetDbContextWithData()
        {
            var options = new DbContextOptionsBuilder<ExamSystemDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ExamSystemDbContext(options);

            var user = new User { UserId = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Email = "john@example.com" };
            var exam = new Exam
            {
                ExamId = Guid.NewGuid(),
                Name = "Sample Exam",
                CreatedByUserId = user.UserId,
                CreatedDate = DateTime.UtcNow.AddDays(-1),
                StartDate = DateTime.UtcNow.AddHours(-1),
                EndDate = DateTime.UtcNow.AddHours(1),
                JoinCode = "JOIN123",
                UserCreatedBy = user
            };
            var examUser = new ExamUser
            {
                ExamUserId = Guid.NewGuid(),
                ExamId = exam.ExamId,
                UserId = user.UserId,
                Exam = exam,
                User = user,
                CompleteStatus = false
            };

            await context.Users.AddAsync(user);
            await context.Exams.AddAsync(exam);
            await context.ExamUsers.AddAsync(examUser);
            await context.SaveChangesAsync();

            return context;
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllExams()
        {
            var context = await GetDbContextWithData();
            var repo = new ExamRepository(context);

            var result = await repo.GetAllAsync();

            Assert.True(result.Success);
            Assert.NotEmpty(result.Data!);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectExam()
        {
            var context = await GetDbContextWithData();
            var examId = context.Exams.First().ExamId;
            var repo = new ExamRepository(context);

            var result = await repo.GetByIdAsync(examId);

            Assert.True(result.Success);
            Assert.Equal(examId, result.Data!.ExamId);
        }

        [Fact]
        public async Task AddAsync_AddsExam()
        {
            var context = await GetDbContextWithData();
            var repo = new ExamRepository(context);

            var newExam = new Exam
            {
                ExamId = Guid.NewGuid(),
                CreatedByUserId = context.Users.First().UserId,
                Name = "New Test",
                CreatedDate = DateTime.UtcNow,
                StartDate = DateTime.UtcNow.AddHours(1),
                EndDate = DateTime.UtcNow.AddHours(2),
                JoinCode = "NEWCODE",
                UserCreatedBy = context.Users.First()
            };

            var result = await repo.AddAsync(newExam);
            Assert.True(result.Success);
            Assert.Contains(context.Exams, e => e.Name == "New Test");
        }

        [Fact]
        public async Task DeleteAsync_RemovesExam()
        {
            var context = await GetDbContextWithData();
            var repo = new ExamRepository(context);
            var examId = context.Exams.First().ExamId;

            var result = await repo.DeleteAsync(examId);

            Assert.True(result.Success);
            Assert.DoesNotContain(context.Exams, e => e.ExamId == examId);
        }

        [Fact]
        public async Task GetExamUserByIdAsync_ReturnsExamUser()
        {
            var context = await GetDbContextWithData();
            var repo = new ExamRepository(context);
            var examUserId = context.ExamUsers.First().ExamUserId;

            var result = await repo.GetExamUserByIdAsync(examUserId);

            Assert.True(result.Success);
            Assert.Equal(examUserId, result.Data!.ExamUserId);
        }

        [Fact]
        public async Task GetExpiredNotFinishedExamUsersAsync_ReturnsCorrectUsers()
        {
            var context = await GetDbContextWithData();

            var exam = context.Exams.First();
            exam.EndDate = DateTime.UtcNow.AddMinutes(-1);
            await context.SaveChangesAsync();

            var repo = new ExamRepository(context);

            var result = await repo.GetExpiredNotFinishedExamUsersAsync(DateTime.UtcNow);

            Assert.True(result.Success);
            Assert.NotEmpty(result.Data!);
        }
    }
}
