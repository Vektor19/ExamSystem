using Xunit;
using Microsoft.EntityFrameworkCore;
using ExamSystem.Persistence.Repositories;
using ExamSystem.Core.Entities;
using ExamSystem.Persistence;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ExamSystem.Core.Common;
using System.Linq;

namespace ExamSystem.Tests.Repositories
{
    public class AnswerRepositoryTests
    {
        private ExamSystemDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ExamSystemDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ExamSystemDbContext(options);

            var user = new User
            {
                UserId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PasswordHash = "hashed_pw"
            };

            var exam = new Exam
            {
                ExamId = Guid.NewGuid(),
                CreatedByUserId = user.UserId,
                Name = "Math Test",
                CreatedDate = DateTime.UtcNow.AddDays(-2),
                StartDate = DateTime.UtcNow.AddHours(-1),
                EndDate = DateTime.UtcNow.AddHours(1),
                JoinCode = "JOIN123",
                UserCreatedBy = user
            };

            var question = new Question
            {
                QuestionId = Guid.NewGuid(),
                ExamId = exam.ExamId,
                QuestionText = "2+2?",
                Type = ExamSystem.Core.Enums.QuestionType.SingleChoice,
                MaxPoints = 5,
                Exam = exam
            };

            var option = new QuestionOption
            {
                QuestionOptionId = Guid.NewGuid(),
                OptionText = "4",
                QuestionId = question.QuestionId
            };

            var examUser = new ExamUser
            {
                ExamUserId = Guid.NewGuid(),
                UserId = user.UserId,
                ExamId = exam.ExamId,
                IsBlocked = false,
                CompleteStatus = false,
                Grade = 0,
                IsChecked = false
            };

            var answer = new Answer
            {
                AnswerId = Guid.NewGuid(),
                UserId = user.UserId,
                ExamId = exam.ExamId,
                QuestionId = question.QuestionId,
                QuestionOptionId = option.QuestionOptionId,
                AnswerText = "4",
                IsGraded = true,
                User = user,
                Exam = exam,
                Question = question,
                QuestionOption = option
            };

            context.Users.Add(user);
            context.Exams.Add(exam);
            context.Questions.Add(question);
            context.QuestionOptions.Add(option);
            context.ExamUsers.Add(examUser);
            context.Answers.Add(answer);
            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllAnswers()
        {
            using var context = GetDbContext();
            var repo = new AnswerRepository(context);

            var result = await repo.GetAllAsync();

            Assert.True(result.Success);
            Assert.Single(result.Data!);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectAnswer()
        {
            using var context = GetDbContext();
            var repo = new AnswerRepository(context);
            var existing = context.Answers.First();

            var result = await repo.GetByIdAsync(existing.AnswerId);

            Assert.True(result.Success);
            Assert.Equal(existing.AnswerId, result.Data!.AnswerId);
        }

        [Fact]
        public async Task AddAsync_AddsNewAnswer()
        {
            using var context = GetDbContext();
            var repo = new AnswerRepository(context);

            var user = context.Users.First();
            var exam = context.Exams.First();
            var question = context.Questions.First();

            var newAnswer = new Answer
            {
                AnswerId = Guid.NewGuid(),
                UserId = user.UserId,
                ExamId = exam.ExamId,
                QuestionId = question.QuestionId,
                AnswerText = "2",
                IsGraded = false
            };

            var result = await repo.AddAsync(newAnswer);

            Assert.True(result.Success);
            Assert.Equal(2, context.Answers.Count());
        }

        [Fact]
        public async Task UpdateAsync_UpdatesAnswer()
        {
            using var context = GetDbContext();
            var repo = new AnswerRepository(context);

            var answer = context.Answers.First();
            answer.AnswerText = "Updated";

            var result = await repo.UpdateAsync(answer);

            Assert.True(result.Success);
            var updated = await context.Answers.FindAsync(answer.AnswerId);
            Assert.Equal("Updated", updated!.AnswerText);
        }

        [Fact]
        public async Task DeleteAsync_RemovesAnswer()
        {
            using var context = GetDbContext();
            var repo = new AnswerRepository(context);

            var answer = context.Answers.First();

            var result = await repo.DeleteAsync(answer.AnswerId);

            Assert.True(result.Success);
            Assert.Empty(context.Answers);
        }

        [Fact]
        public async Task GetAllByExamUserIdAsync_ReturnsCorrectAnswers()
        {
            using var context = GetDbContext();
            var repo = new AnswerRepository(context);

            var examUser = context.ExamUsers.First();

            var result = await repo.GetAllByExamUserIdAsync(examUser.ExamUserId);

            Assert.True(result.Success);
            Assert.Single(result.Data!);
        }

        [Fact]
        public async Task GetByIdAsync_FailsIfNotFound()
        {
            using var context = GetDbContext();
            var repo = new AnswerRepository(context);

            var result = await repo.GetByIdAsync(Guid.NewGuid());

            Assert.False(result.Success);
            Assert.Equal("Answer not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task DeleteAsync_FailsIfNotFound()
        {
            using var context = GetDbContext();
            var repo = new AnswerRepository(context);

            var result = await repo.DeleteAsync(Guid.NewGuid());

            Assert.False(result.Success);
            Assert.Equal("Answer not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task GetAllByExamUserIdAsync_FailsIfExamUserNotFound()
        {
            using var context = GetDbContext();
            var repo = new AnswerRepository(context);

            var result = await repo.GetAllByExamUserIdAsync(Guid.NewGuid());

            Assert.False(result.Success);
            Assert.Equal("ExamUser not found.", result.ErrorMessage);
        }
    }
}
