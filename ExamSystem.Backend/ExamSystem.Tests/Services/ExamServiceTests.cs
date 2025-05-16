using AutoMapper;
using ExamSystem.Application.Common.Enums;
using ExamSystem.Application.Common.Models;
using ExamSystem.Application.DTOs;
using ExamSystem.Application.Services;
using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Enums;
using ExamSystem.Core.Interfaces.Repositories;
using Moq;
using Xunit;

namespace ExamSystem.Tests.Services
{
    public class ExamServiceTests
    {
        private readonly Mock<IExamRepository> _examRepoMock = new();
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly ExamService _service;

        public ExamServiceTests()
        {
            _service = new ExamService(_examRepoMock.Object, _mapperMock.Object, _userRepoMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldFail_WhenUserNotFound()
        {
            var dto = new ExamCreateDto { CreatedByUserId = Guid.NewGuid() };
            _userRepoMock.Setup(r => r.GetByIdAsync(dto.CreatedByUserId))
                         .ReturnsAsync(RepositoryOperationResult<User>.Fail("User not found"));

            var result = await _service.CreateAsync(dto);

            Assert.False(result.Success);
            Assert.Equal(ServiceOperationErrorType.BadRequest, result.ErrorType);
        }

        [Fact]
        public async Task DeleteAsync_ShouldFail_WhenExamInProgress()
        {
            var examId = Guid.NewGuid();
            var exam = new Exam
            {
                ExamId = examId,
                StartDate = DateTime.UtcNow.AddMinutes(-5),
                EndDate = DateTime.UtcNow.AddMinutes(5)
            };

            _examRepoMock.Setup(r => r.GetByIdAsync(examId))
                         .ReturnsAsync(RepositoryOperationResult<Exam>.Ok(exam));

            var result = await _service.DeleteAsync(examId);

            Assert.False(result.Success);
            Assert.Equal(ServiceOperationErrorType.Forbidden, result.ErrorType);
        }

        [Fact]
        public async Task DeleteAsync_ShouldSucceed_WhenAllowed()
        {
            var examId = Guid.NewGuid();
            var exam = new Exam
            {
                ExamId = examId,
                StartDate = DateTime.UtcNow.AddHours(1),
                EndDate = DateTime.UtcNow.AddHours(2)
            };

            _examRepoMock.Setup(r => r.GetByIdAsync(examId))
                         .ReturnsAsync(RepositoryOperationResult<Exam>.Ok(exam));
            _examRepoMock.Setup(r => r.DeleteAsync(examId))
                         .ReturnsAsync(RepositoryOperationResult.Ok());

            var result = await _service.DeleteAsync(examId);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnDtos()
        {
            var exams = new List<Exam> { new Exam { Name = "Test" } };
            var dtos = new List<ExamForExaminatorDto> { new ExamForExaminatorDto { Name = "Test" } };

            _examRepoMock.Setup(r => r.GetAllAsync())
                         .ReturnsAsync(RepositoryOperationResult<IEnumerable<Exam>>.Ok(exams));
            _mapperMock.Setup(m => m.Map<IEnumerable<ExamForExaminatorDto>>(exams))
                       .Returns(dtos);

            var result = await _service.GetAllAsync();

            Assert.True(result.Success);
            Assert.Single(result.Data!);
        }

        [Fact]
        public async Task AddParticipantAsync_ShouldFail_WhenExamNotFound()
        {
            var examId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            _examRepoMock.Setup(r => r.GetByIdAsync(examId))
                         .ReturnsAsync(RepositoryOperationResult<Exam>.Fail("Exam not found"));

            var result = await _service.AddParticipantAsync(examId, userId);

            Assert.False(result.Success);
            Assert.Equal(ServiceOperationErrorType.NotFound, result.ErrorType);
        }

        [Fact]
        public async Task RemoveParticipantAsync_ShouldFail_WhenUserNotInExam()
        {
            var examId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var exam = new Exam
            {
                ExamId = examId,
                StartDate = DateTime.UtcNow.AddHours(1),
                EndDate = DateTime.UtcNow.AddHours(2),
                ExamUsers = new List<ExamUser>()
            };
            var user = new User { UserId = userId };

            _examRepoMock.Setup(r => r.GetByIdAsync(examId))
                         .ReturnsAsync(RepositoryOperationResult<Exam>.Ok(exam));
            _userRepoMock.Setup(r => r.GetByIdAsync(userId))
                         .ReturnsAsync(RepositoryOperationResult<User>.Ok(user));

            var result = await _service.RemoveParticipantAsync(examId, userId);

            Assert.False(result.Success);
            Assert.Equal(ServiceOperationErrorType.NotFound, result.ErrorType);
        }
        [Fact]
        public async Task AddParticipantByEmailAsync_ShouldAddParticipant_WhenValid()
        {
            var examId = Guid.NewGuid();
            var user = new User { UserId = Guid.NewGuid(), Email = "user@example.com" };
            var exam = new Exam { ExamId = examId, ExamUsers = new List<ExamUser>() };

            _examRepoMock.Setup(r => r.GetByIdAsync(examId))
                .ReturnsAsync(RepositoryOperationResult<Exam>.Ok(exam));
            _userRepoMock.Setup(r => r.GetByEmailAsync(user.Email))
                .ReturnsAsync(RepositoryOperationResult<User>.Ok(user));
            _examRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Exam>()))
                .ReturnsAsync(RepositoryOperationResult.Ok());

            var result = await _service.AddParticipantByEmailAsync(examId, user.Email);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task AddParticipantByEmailAsync_ShouldFail_WhenExamNotFound()
        {
            var examId = Guid.NewGuid();
            _examRepoMock.Setup(r => r.GetByIdAsync(examId))
                .ReturnsAsync(RepositoryOperationResult<Exam>.Fail("Exam not found"));

            var result = await _service.AddParticipantByEmailAsync(examId, "test@example.com");

            Assert.False(result.Success);
            Assert.Equal(ServiceOperationErrorType.NotFound, result.ErrorType);
        }

        [Fact]
        public async Task RemoveParticipantByEmailAsync_ShouldRemove_WhenValid()
        {
            var examId = Guid.NewGuid();
            var user = new User { UserId = Guid.NewGuid(), Email = "user@example.com" };
            var examUser = new ExamUser { UserId = user.UserId };
            var exam = new Exam
            {
                ExamId = examId,
                StartDate = DateTime.UtcNow.AddHours(1),
                EndDate = DateTime.UtcNow.AddHours(2),
                ExamUsers = new List<ExamUser> { examUser }
            };

            _examRepoMock.Setup(r => r.GetByIdAsync(examId)).ReturnsAsync(RepositoryOperationResult<Exam>.Ok(exam));
            _userRepoMock.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync(RepositoryOperationResult<User>.Ok(user));
            _examRepoMock.Setup(r => r.UpdateAsync(exam)).ReturnsAsync(RepositoryOperationResult.Ok());

            var result = await _service.RemoveParticipantByEmailAsync(examId, user.Email);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task RemoveParticipantByEmailAsync_ShouldFail_WhenExamInProgress()
        {
            var examId = Guid.NewGuid();
            var user = new User { UserId = Guid.NewGuid(), Email = "user@example.com" };
            var examUser = new ExamUser { UserId = user.UserId };
            var exam = new Exam
            {
                ExamId = examId,
                StartDate = DateTime.UtcNow.AddMinutes(-1),
                EndDate = DateTime.UtcNow.AddMinutes(10),
                ExamUsers = new List<ExamUser> { examUser }
            };

            _examRepoMock.Setup(r => r.GetByIdAsync(examId)).ReturnsAsync(RepositoryOperationResult<Exam>.Ok(exam));
            _userRepoMock.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync(RepositoryOperationResult<User>.Ok(user));

            var result = await _service.RemoveParticipantByEmailAsync(examId, user.Email);

            Assert.False(result.Success);
            Assert.Equal(ServiceOperationErrorType.Forbidden, result.ErrorType);
        }

        [Fact]
        public async Task JoinExam_ShouldJoin_WhenValid()
        {
            var userId = Guid.NewGuid();
            var exam = new Exam
            {
                JoinCode = "XYZ123",
                StartDate = DateTime.UtcNow.AddHours(1),
                EndDate = DateTime.UtcNow.AddHours(2),
                ExamUsers = new List<ExamUser>()
            };
            var user = new User { UserId = userId };
            var dto = new JoinExamDto { JoinCode = "XYZ123", UserId = userId };

            _examRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(RepositoryOperationResult<IEnumerable<Exam>>.Ok(new[] { exam }));
            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(RepositoryOperationResult<User>.Ok(user));
            _examRepoMock.Setup(r => r.UpdateAsync(exam)).ReturnsAsync(RepositoryOperationResult.Ok());

            var result = await _service.JoinExam(dto);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task JoinExam_ShouldFail_WhenExamNotFound()
        {
            _examRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(RepositoryOperationResult<IEnumerable<Exam>>.Ok([]));

            var result = await _service.JoinExam(new JoinExamDto { JoinCode = "invalid", UserId = Guid.NewGuid() });

            Assert.False(result.Success);
            Assert.Equal(ServiceOperationErrorType.BadRequest, result.ErrorType);
        }

        [Fact]
        public async Task FinishExamAsync_ShouldMarkAsComplete_WhenValid()
        {
            var examId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var examUser = new ExamUser { UserId = userId };
            var exam = new Exam
            {
                ExamId = examId,
                ExamUsers = new List<ExamUser> { examUser },
                Questions = new List<Question>()
            };

            _examRepoMock.Setup(r => r.GetByIdAsync(examId)).ReturnsAsync(RepositoryOperationResult<Exam>.Ok(exam));
            _examRepoMock.Setup(r => r.UpdateAsync(exam)).ReturnsAsync(RepositoryOperationResult.Ok());

            var result = await _service.FinishExamAsync(examId, userId);

            Assert.True(result.Success);
            Assert.True(examUser.CompleteStatus);
        }

        [Fact]
        public async Task BlockExamUserByIdAsync_ShouldBlockUser_WhenValid()
        {
            var examUserId = Guid.NewGuid();
            var exam = new Exam
            {
                ExamId = Guid.NewGuid(),
                ExamUsers = new List<ExamUser> { new ExamUser { ExamUserId = examUserId } }
            };

            _examRepoMock.Setup(r => r.GetExamUserByIdAsync(examUserId))
                .ReturnsAsync(RepositoryOperationResult<ExamUser>.Ok(new ExamUser { ExamUserId = examUserId, ExamId = exam.ExamId }));
            _examRepoMock.Setup(r => r.GetByIdAsync(exam.ExamId))
                .ReturnsAsync(RepositoryOperationResult<Exam>.Ok(exam));
            _examRepoMock.Setup(r => r.UpdateAsync(exam))
                .ReturnsAsync(RepositoryOperationResult.Ok());

            var result = await _service.BlockExamUserByIdAsync(examUserId);

            Assert.True(result.Success);
            Assert.True(exam.ExamUsers.First().IsBlocked);
        }

        [Fact]
        public async Task IsUserBlockedInExamAsync_ShouldReturnTrue_IfBlocked()
        {
            var examId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var exam = new Exam
            {
                ExamUsers = new List<ExamUser>
            {
                new ExamUser { UserId = userId, IsBlocked = true }
            }
            };

            _examRepoMock.Setup(r => r.GetByIdAsync(examId))
                .ReturnsAsync(RepositoryOperationResult<Exam>.Ok(exam));

            var result = await _service.IsUserBlockedInExamAsync(examId, userId);

            Assert.True(result.Success);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task IsExamInProgressAsync_ShouldReturnTrue_IfStatusStarted()
        {
            var examId = Guid.NewGuid();
            var exam = new Exam { StartDate = DateTime.UtcNow.AddMinutes(-5), EndDate = DateTime.UtcNow.AddMinutes(5) };

            _examRepoMock.Setup(r => r.GetByIdAsync(examId))
                .ReturnsAsync(RepositoryOperationResult<Exam>.Ok(exam));

            var result = await _service.IsExamInProgressAsync(examId);

            Assert.True(result.Success);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task GetExpiredNotFinishedExamUsersAsync_ShouldReturnDtos()
        {
            var now = DateTime.UtcNow;
            var examUsers = new List<ExamUser> { new ExamUser { Grade = 10 } };
            var dtos = new List<ExamUserDto> { new ExamUserDto { Grade = 10 } };

            _examRepoMock.Setup(r => r.GetExpiredNotFinishedExamUsersAsync(now))
                .ReturnsAsync(RepositoryOperationResult<IEnumerable<ExamUser>>.Ok(examUsers));
            _mapperMock.Setup(m => m.Map<IEnumerable<ExamUserDto>>(examUsers))
                .Returns(dtos);

            var result = await _service.GetExpiredNotFinishedExamUsersAsync(now);

            Assert.True(result.Success);
            Assert.Equal(10, result.Data!.First().Grade);
        }
    }
}
