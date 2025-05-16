using AutoMapper;
using ExamSystem.Application.Common.Enums;
using ExamSystem.Application.Common.Models;
using ExamSystem.Application.DTOs.Question;
using ExamSystem.Application.DTOs.QuestionOption;
using ExamSystem.Application.Services;
using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Enums;
using ExamSystem.Core.Interfaces.Repositories;
using Moq;
using Xunit;

namespace ExamSystem.Tests.Services
{
    public class QuestionServiceTests
    {
        private readonly Mock<IExamRepository> _examRepoMock = new();
        private readonly Mock<IQuestionRepository> _questionRepoMock = new();
        private readonly Mock<IAnswerRepository> _answerRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly QuestionService _service;

        public QuestionServiceTests()
        {
            _service = new QuestionService(
                _examRepoMock.Object,
                _mapperMock.Object,
                _questionRepoMock.Object,
                _answerRepoMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnBadRequest_WhenExamNotFound()
        {
            var dto = new QuestionCreateDto { ExamId = Guid.NewGuid() };
            _examRepoMock.Setup(r => r.GetByIdAsync(dto.ExamId))
                .ReturnsAsync(RepositoryOperationResult<Exam>.Fail("not found"));

            var result = await _service.CreateAsync(dto);

            Assert.False(result.Success);
            Assert.Equal(ServiceOperationErrorType.BadRequest, result.ErrorType);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnOk_WhenQuestionCreated()
        {
            var examId = Guid.NewGuid();
            var dto = new QuestionCreateDto { ExamId = examId };
            var exam = new Exam { ExamId = examId, StartDate = DateTime.UtcNow.AddHours(1), EndDate = DateTime.UtcNow.AddHours(2) };

            _examRepoMock.Setup(r => r.GetByIdAsync(examId))
                .ReturnsAsync(RepositoryOperationResult<Exam>.Ok(exam));

            _mapperMock.Setup(m => m.Map<Question>(dto))
                .Returns(new Question { ExamId = examId });

            _questionRepoMock.Setup(r => r.AddAsync(It.IsAny<Question>()))
                .ReturnsAsync(RepositoryOperationResult.Ok());

            var result = await _service.CreateAsync(dto);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnOk_WhenDeleted()
        {
            var id = Guid.NewGuid();
            _questionRepoMock.Setup(r => r.DeleteAsync(id))
                .ReturnsAsync(RepositoryOperationResult.Ok());

            var result = await _service.DeleteAsync(id);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnQuestions()
        {
            var questions = new List<Question> { new() { QuestionId = Guid.NewGuid() } };
            var dtos = new List<QuestionDto> { new() { QuestionId = questions[0].QuestionId } };

            _questionRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(RepositoryOperationResult<IEnumerable<Question>>.Ok(questions));
            _mapperMock.Setup(m => m.Map<IEnumerable<QuestionDto>>(questions))
                .Returns(dtos);

            var result = await _service.GetAllAsync();

            Assert.True(result.Success);
            Assert.Single(result.Data!);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnQuestionDto_WhenExists()
        {
            var id = Guid.NewGuid();
            var question = new Question { QuestionId = id };
            var dto = new QuestionDto { QuestionId = id };

            _questionRepoMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(RepositoryOperationResult<Question>.Ok(question));
            _mapperMock.Setup(m => m.Map<QuestionDto>(question)).Returns(dto);

            var result = await _service.GetByIdAsync(id);

            Assert.True(result.Success);
            Assert.Equal(id, result.Data!.QuestionId);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNotFound_WhenQuestionNotExists()
        {
            var id = Guid.NewGuid();
            var dto = new QuestionUpdateDto();

            _questionRepoMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(RepositoryOperationResult<Question>.Fail("not found"));

            var result = await _service.UpdateAsync(id, dto);

            Assert.False(result.Success);
            Assert.Equal(ServiceOperationErrorType.NotFound, result.ErrorType);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnOk_WhenUpdated()
        {
            var id = Guid.NewGuid();
            var dto = new QuestionUpdateDto
            {
                QuestionText = "Updated",
                Type = QuestionType.Text.ToString(),
                MaxPoints = 10,
                Options = new List<QuestionOptionUpdateDto>()
            };
            var question = new Question { QuestionId = id, QuestionOptions = new List<QuestionOption>() };

            _questionRepoMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(RepositoryOperationResult<Question>.Ok(question));
            _questionRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Question>()))
                .ReturnsAsync(RepositoryOperationResult.Ok());

            var result = await _service.UpdateAsync(id, dto);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetAllByExamIdAsync_ShouldFilterByExamId()
        {
            var examId = Guid.NewGuid();
            var questions = new List<Question> {
                new() { ExamId = examId },
                new() { ExamId = Guid.NewGuid() } 
            };
            var expected = new List<QuestionDto> { new() { Exam = new() { ExamId = examId } } };

            _questionRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(RepositoryOperationResult<IEnumerable<Question>>.Ok(questions));
            _mapperMock.Setup(m => m.Map<IEnumerable<QuestionDto>>(It.Is<IEnumerable<Question>>(q => q.All(x => x.ExamId == examId))))
                .Returns(expected);

            var result = await _service.GetAllByExamIdAsync(examId);

            Assert.True(result.Success);
            Assert.Single(result.Data!);
        }
    }
}
