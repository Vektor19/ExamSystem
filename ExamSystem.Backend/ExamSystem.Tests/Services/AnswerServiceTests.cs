using AutoMapper;
using ExamSystem.Application.Common.Enums;
using ExamSystem.Application.DTOs;
using ExamSystem.Application.Services;
using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Interfaces.Repositories;
using Moq;

namespace ExamSystem.Tests.Services
{
    public class AnswerServiceTests
    {
        private readonly Mock<IAnswerRepository> _answerRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly AnswerService _service;

        public AnswerServiceTests()
        {
            _service = new AnswerService(_mapperMock.Object, _answerRepoMock.Object);
        }

        [Fact]
        public async Task CreateOpenAnswerAsync_ShouldReturnFail_WhenWrongType()
        {
            var dto = new CreateAnswerDto();

            var result = await _service.CreateOpenAnswerAsync(dto);

            Assert.False(result.Success);
            Assert.Equal(ServiceOperationErrorType.Internal, result.ErrorType);
        }

        [Fact]
        public async Task CreateOpenAnswerAsync_ShouldReturnOk_WhenValid()
        {
            var dto = new CreateOpenAnswerDto
            {
                AnswerText = "Test answer",
                ExamId = Guid.NewGuid(),
                QuestionId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            var answer = new Answer();
            _mapperMock.Setup(m => m.Map<Answer>(dto)).Returns(answer);
            _answerRepoMock.Setup(r => r.AddAsync(answer))
                .ReturnsAsync(RepositoryOperationResult.Ok());

            var result = await _service.CreateOpenAnswerAsync(dto);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task CreateOptionAnswerAsync_ShouldReturnFail_WhenWrongType()
        {
            var dto = new CreateAnswerDto();

            var result = await _service.CreateOptionAnswerAsync(dto);

            Assert.False(result.Success);
            Assert.Equal(ServiceOperationErrorType.Internal, result.ErrorType);
        }

        [Fact]
        public async Task CreateOptionAnswerAsync_ShouldReturnOk_WhenValid()
        {
            var dto = new CreateOptionAnswerDto
            {
                QuestionOptionId = Guid.NewGuid(),
                ExamId = Guid.NewGuid(),
                QuestionId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            var answer = new Answer();
            _mapperMock.Setup(m => m.Map<Answer>(dto)).Returns(answer);
            _answerRepoMock.Setup(r => r.AddAsync(answer))
                .ReturnsAsync(RepositoryOperationResult.Ok());

            var result = await _service.CreateOptionAnswerAsync(dto);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnOk_WhenDeleted()
        {
            var id = Guid.NewGuid();
            _answerRepoMock.Setup(r => r.DeleteAsync(id))
                .ReturnsAsync(RepositoryOperationResult.Ok());

            var result = await _service.DeleteAsync(id);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnDtos_WhenSuccess()
        {
            var answers = new List<Answer> { new() };
            var dtos = new List<AnswerDto> { new() };

            _answerRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(RepositoryOperationResult<IEnumerable<Answer>>.Ok(answers));
            _mapperMock.Setup(m => m.Map<IEnumerable<AnswerDto>>(answers))
                .Returns(dtos);

            var result = await _service.GetAllAsync();

            Assert.True(result.Success);
            Assert.Single(result.Data!);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnDto_WhenFound()
        {
            var id = Guid.NewGuid();
            var answer = new Answer();
            var dto = new AnswerDto { AnswerId = id };

            _answerRepoMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(RepositoryOperationResult<Answer>.Ok(answer));
            _mapperMock.Setup(m => m.Map<AnswerDto>(answer)).Returns(dto);

            var result = await _service.GetByIdAsync(id);

            Assert.True(result.Success);
            Assert.Equal(id, result.Data!.AnswerId);
        }

        [Fact]
        public async Task GetAllByExamIdAsync_ShouldReturnFilteredDtos()
        {
            var examId = Guid.NewGuid();
            var answers = new List<Answer> { new() { ExamId = examId }, new() { ExamId = Guid.NewGuid() } };
            var filtered = answers.Where(a => a.ExamId == examId);
            var dtos = filtered.Select(a => new AnswerDto { ExamId = a.ExamId });

            _answerRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(RepositoryOperationResult<IEnumerable<Answer>>.Ok(answers));
            _mapperMock.Setup(m => m.Map<IEnumerable<AnswerDto>>(It.IsAny<IEnumerable<Answer>>()))
                .Returns(dtos);

            var result = await _service.GetAllByExamIdAsync(examId);

            Assert.True(result.Success);
            Assert.All(result.Data!, d => Assert.Equal(examId, d.ExamId));
        }

        [Fact]
        public async Task GetAllByUserIdAsync_ShouldReturnFilteredDtos()
        {
            var userId = Guid.NewGuid();
            var answers = new List<Answer> { new() { UserId = userId }, new() { UserId = Guid.NewGuid() } };
            var filtered = answers.Where(a => a.UserId == userId);
            var dtos = filtered.Select(a => new AnswerDto { UserId = a.UserId });

            _answerRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(RepositoryOperationResult<IEnumerable<Answer>>.Ok(answers));
            _mapperMock.Setup(m => m.Map<IEnumerable<AnswerDto>>(It.IsAny<IEnumerable<Answer>>()))
                .Returns(dtos);

            var result = await _service.GetAllByUserIdAsync(userId);

            Assert.True(result.Success);
            Assert.All(result.Data!, d => Assert.Equal(userId, d.UserId));
        }

        [Fact]
        public async Task GetAllByExamUserIdAsync_ShouldReturnDtos_WhenSuccess()
        {
            var examUserId = Guid.NewGuid();
            var answers = new List<Answer> { new() };
            var dtos = new List<AnswerDto> { new() };

            _answerRepoMock.Setup(r => r.GetAllByExamUserIdAsync(examUserId))
                .ReturnsAsync(RepositoryOperationResult<IEnumerable<Answer>>.Ok(answers));
            _mapperMock.Setup(m => m.Map<IEnumerable<AnswerDto>>(answers)).Returns(dtos);

            var result = await _service.GetAllByExamUserIdAsync(examUserId);

            Assert.True(result.Success);
            Assert.Single(result.Data!);
        }
    }
}