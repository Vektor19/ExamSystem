using Microsoft.EntityFrameworkCore;
using ExamSystem.Core.Entities;
using ExamSystem.Persistence.Repositories;
using ExamSystem.Persistence;

namespace ExamSystem.Tests.Repositories
{
    public class QuestionOptionRepositoryTests
    {
        private async Task<ExamSystemDbContext> GetDbContextWithData()
        {
            var options = new DbContextOptionsBuilder<ExamSystemDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ExamSystemDbContext(options);

            var question = new Question
            {
                QuestionId = Guid.NewGuid(),
                QuestionText = "Sample Question"
            };

            var questionOption = new QuestionOption
            {
                QuestionOptionId = Guid.NewGuid(),
                OptionText = "Option A",
                IsCorrect = true,
                QuestionId = question.QuestionId,
                Question = question
            };

            await context.Questions.AddAsync(question);
            await context.QuestionOptions.AddAsync(questionOption);
            await context.SaveChangesAsync();

            return context;
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllQuestionOptions()
        {
            var context = await GetDbContextWithData();
            var repo = new QuestionOptionRepository(context);

            var result = await repo.GetAllAsync();

            Assert.True(result.Success);
            Assert.NotEmpty(result.Data!);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectQuestionOption()
        {
            var context = await GetDbContextWithData();
            var id = context.QuestionOptions.First().QuestionOptionId;
            var repo = new QuestionOptionRepository(context);

            var result = await repo.GetByIdAsync(id);

            Assert.True(result.Success);
            Assert.Equal(id, result.Data!.QuestionOptionId);
        }

        [Fact]
        public async Task AddAsync_AddsQuestionOption()
        {
            var context = await GetDbContextWithData();
            var repo = new QuestionOptionRepository(context);
            var question = context.Questions.First();

            var newOption = new QuestionOption
            {
                QuestionOptionId = Guid.NewGuid(),
                OptionText = "Option B",
                IsCorrect = false,
                QuestionId = question.QuestionId,
                Question = question
            };

            var result = await repo.AddAsync(newOption);

            Assert.True(result.Success);
            Assert.Contains(context.QuestionOptions, o => o.OptionText == "Option B");
        }

        [Fact]
        public async Task UpdateAsync_UpdatesQuestionOption()
        {
            var context = await GetDbContextWithData();
            var repo = new QuestionOptionRepository(context);
            var option = context.QuestionOptions.First();
            option.OptionText = "Updated Option";

            var result = await repo.UpdateAsync(option);

            Assert.True(result.Success);
            Assert.Equal("Updated Option", context.QuestionOptions.First().OptionText);
        }

        [Fact]
        public async Task DeleteAsync_RemovesQuestionOption()
        {
            var context = await GetDbContextWithData();
            var repo = new QuestionOptionRepository(context);
            var optionId = context.QuestionOptions.First().QuestionOptionId;

            var result = await repo.DeleteAsync(optionId);

            Assert.True(result.Success);
            Assert.DoesNotContain(context.QuestionOptions, o => o.QuestionOptionId == optionId);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsFail_WhenNotFound()
        {
            var context = await GetDbContextWithData();
            var repo = new QuestionOptionRepository(context);
            var randomId = Guid.NewGuid();

            var result = await repo.GetByIdAsync(randomId);

            Assert.False(result.Success);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFail_WhenNotFound()
        {
            var context = await GetDbContextWithData();
            var repo = new QuestionOptionRepository(context);
            var randomId = Guid.NewGuid();

            var result = await repo.DeleteAsync(randomId);

            Assert.False(result.Success);
        }
    }
}
