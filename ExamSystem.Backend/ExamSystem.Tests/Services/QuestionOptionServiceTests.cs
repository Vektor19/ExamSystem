using AutoMapper;
using ExamSystem.Application.Common.Enums;
using ExamSystem.Application.Common.Models;
using ExamSystem.Application.DTOs.QuestionOption;
using ExamSystem.Application.Services;
using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Interfaces.Repositories;
using Moq;
using Xunit;

namespace ExamSystem.Tests.Services
{
    public class QuestionOptionServiceTests
    {
        private readonly Mock<IQuestionOptionRepository> _optionRepoMock = new();
        private readonly Mock<IQuestionRepository> _questionRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly QuestionOptionService _service;

        public QuestionOptionServiceTests()
        {
            _service = new QuestionOptionService(
                _mapperMock.Object,
                _optionRepoMock.Object,
                _questionRepoMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldFail_IfQuestionNotFound()
        {
            var dto = new QuestionOptionCreateDto { QuestionId = Guid.NewGuid() };
            _questionRepoMock.Setup(r => r.GetByIdAsync(dto.QuestionId))
                .ReturnsAsync(RepositoryOperationResult<Question>.Fail("Not found"));

            var result = await _service.CreateAsync(dto);

            Assert.False(result.Success);
            Assert.Equal(ServiceOperationErrorType.BadRequest, result.ErrorType);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnOk_IfSuccess()
        {
            var question = new Question { QuestionId = Guid.NewGuid() };
            var dto = new QuestionOptionCreateDto { QuestionId = question.QuestionId };
            var option = new QuestionOption();

            _questionRepoMock.Setup(r => r.GetByIdAsync(dto.QuestionId))
                .ReturnsAsync(RepositoryOperationResult<Question>.Ok(question));
            _mapperMock.Setup(m => m.Map<QuestionOption>(dto)).Returns(option);
            _optionRepoMock.Setup(r => r.AddAsync(It.IsAny<QuestionOption>()))
                .ReturnsAsync(RepositoryOperationResult.Ok());

            var result = await _service.CreateAsync(dto);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnOk_IfSuccess()
        {
            var id = Guid.NewGuid();
            _optionRepoMock.Setup(r => r.DeleteAsync(id))
                .ReturnsAsync(RepositoryOperationResult.Ok());

            var result = await _service.DeleteAsync(id);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnDtos()
        {
            var options = new List<QuestionOption> { new() };
            var dtos = new List<QuestionOptionDto> { new() };

            _optionRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(RepositoryOperationResult<IEnumerable<QuestionOption>>.Ok(options));
            _mapperMock.Setup(m => m.Map<IEnumerable<QuestionOptionDto>>(options)).Returns(dtos);

            var result = await _service.GetAllAsync();

            Assert.True(result.Success);
            Assert.Single(result.Data!);
        }

        [Fact]
        public async Task GetAllByQuestionIdAsync_ShouldFilterByQuestion()
        {
            var questionId = Guid.NewGuid();
            var allOptions = new List<QuestionOption>
            {
                new() { QuestionId = questionId },
                new() { QuestionId = Guid.NewGuid() }
            };
            var filteredDtos = new List<QuestionOptionDto> { new() };

            _optionRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(RepositoryOperationResult<IEnumerable<QuestionOption>>.Ok(allOptions));
            _mapperMock.Setup(m => m.Map<IEnumerable<QuestionOptionDto>>(It.Is<IEnumerable<QuestionOption>>(l => l.All(q => q.QuestionId == questionId))))
                .Returns(filteredDtos);

            var result = await _service.GetAllByQuestionIdAsync(questionId);

            Assert.True(result.Success);
            Assert.Single(result.Data!);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnDto_IfFound()
        {
            var id = Guid.NewGuid();
            var option = new QuestionOption { QuestionOptionId = id };
            var dto = new QuestionOptionDto { QuestionOptionId = id };

            _optionRepoMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(RepositoryOperationResult<QuestionOption>.Ok(option));
            _mapperMock.Setup(m => m.Map<QuestionOptionDto>(option)).Returns(dto);

            var result = await _service.GetByIdAsync(id);

            Assert.True(result.Success);
            Assert.Equal(id, result.Data!.QuestionOptionId);
        }

        [Fact]
        public async Task UpdateAsync_ShouldFail_IfNotFound()
        {
            var id = Guid.NewGuid();
            _optionRepoMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(RepositoryOperationResult<QuestionOption>.Fail("Not found"));

            var result = await _service.UpdateAsync(id, new QuestionOptionUpdateDto());

            Assert.False(result.Success);
            Assert.Equal(ServiceOperationErrorType.NotFound, result.ErrorType);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnOk_IfSuccess()
        {
            var id = Guid.NewGuid();
            var existing = new QuestionOption { QuestionOptionId = id };
            var updateDto = new QuestionOptionUpdateDto
            {
                OptionText = "Updated",
                Label = "A",
                IsCorrect = true
            };

            _optionRepoMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(RepositoryOperationResult<QuestionOption>.Ok(existing));
            _optionRepoMock.Setup(r => r.UpdateAsync(It.IsAny<QuestionOption>()))
                .ReturnsAsync(RepositoryOperationResult.Ok());

            var result = await _service.UpdateAsync(id, updateDto);

            Assert.True(result.Success);
        }
    }
}