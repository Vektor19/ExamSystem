using Microsoft.EntityFrameworkCore;
using ExamSystem.Core.Entities;
using ExamSystem.Persistence.Repositories;
using ExamSystem.Persistence;

namespace ExamSystem.Tests.Repositories
{
    public class QuestionRepositoryTests
    {
        private async Task<ExamSystemDbContext> GetDbContextWithData()
        {
            var options = new DbContextOptionsBuilder<ExamSystemDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ExamSystemDbContext(options);

            var exam = new Exam
            {
                ExamId = Guid.NewGuid(),
                Name = "Sample Exam",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(1)
            };

            var question1 = new Question
            {
                QuestionId = Guid.NewGuid(),
                QuestionText = "Question 1",
                ExamId = exam.ExamId,
                Exam = exam
            };

            var question2 = new Question
            {
                QuestionId = Guid.NewGuid(),
                QuestionText = "Question 2",
                ExamId = exam.ExamId,
                Exam = exam
            };

            var userId = Guid.NewGuid();

            var answer = new Answer
            {
                AnswerId = Guid.NewGuid(),
                QuestionId = question1.QuestionId,
                UserId = userId,
                AnswerText = "Answer 1"
            };

            await context.Exams.AddAsync(exam);
            await context.Questions.AddRangeAsync(question1, question2);
            await context.Answers.AddAsync(answer);
            await context.SaveChangesAsync();

            return context;
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllQuestions()
        {
            var context = await GetDbContextWithData();
            var repo = new QuestionRepository(context);

            var result = await repo.GetAllAsync();

            Assert.True(result.Success);
            Assert.NotEmpty(result.Data!);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectQuestion()
        {
            var context = await GetDbContextWithData();
            var id = context.Questions.First().QuestionId;
            var repo = new QuestionRepository(context);

            var result = await repo.GetByIdAsync(id);

            Assert.True(result.Success);
            Assert.Equal(id, result.Data!.QuestionId);
        }

        [Fact]
        public async Task AddAsync_AddsQuestion()
        {
            var context = await GetDbContextWithData();
            var repo = new QuestionRepository(context);

            var newQuestion = new Question
            {
                QuestionId = Guid.NewGuid(),
                QuestionText = "New Question",
                ExamId = context.Exams.First().ExamId
            };

            var result = await repo.AddAsync(newQuestion);

            Assert.True(result.Success);
            Assert.Contains(context.Questions, q => q.QuestionText == "New Question");
        }

        [Fact]
        public async Task UpdateAsync_UpdatesQuestion()
        {
            var context = await GetDbContextWithData();
            var repo = new QuestionRepository(context);
            var question = context.Questions.First();
            question.QuestionText = "Updated Text";

            var result = await repo.UpdateAsync(question);

            Assert.True(result.Success);
            Assert.Equal("Updated Text", context.Questions.First().QuestionText);
        }

        [Fact]
        public async Task DeleteAsync_DeletesQuestion()
        {
            var context = await GetDbContextWithData();
            var repo = new QuestionRepository(context);
            var id = context.Questions.First().QuestionId;

            var result = await repo.DeleteAsync(id);

            Assert.True(result.Success);
            Assert.DoesNotContain(context.Questions, q => q.QuestionId == id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsFail_WhenNotFound()
        {
            var context = await GetDbContextWithData();
            var repo = new QuestionRepository(context);

            var result = await repo.GetByIdAsync(Guid.NewGuid());

            Assert.False(result.Success);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFail_WhenNotFound()
        {
            var context = await GetDbContextWithData();
            var repo = new QuestionRepository(context);

            var result = await repo.DeleteAsync(Guid.NewGuid());

            Assert.False(result.Success);
        }

        [Fact]
        public async Task GetUnansweredByUserAsync_ReturnsCorrectQuestions()
        {
            var context = await GetDbContextWithData();
            var repo = new QuestionRepository(context);
            var examId = context.Exams.First().ExamId;
            var userId = context.Answers.First().UserId;

            var result = await repo.GetUnansweredByUserAsync(examId, userId);

            Assert.True(result.Success);
            Assert.Single(result.Data!);
        }

        [Fact]
        public async Task GetUnansweredByUserAsync_ReturnsAll_WhenNoAnswers()
        {
            var context = await GetDbContextWithData();
            var repo = new QuestionRepository(context);
            var examId = context.Exams.First().ExamId;
            var unknownUserId = Guid.NewGuid();

            var result = await repo.GetUnansweredByUserAsync(examId, unknownUserId);

            Assert.True(result.Success);
            Assert.Equal(2, result.Data!.Count());
        }
    }
}