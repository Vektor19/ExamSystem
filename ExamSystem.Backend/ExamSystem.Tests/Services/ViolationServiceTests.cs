using AutoMapper;
using ExamSystem.Application.Common.Enums;
using ExamSystem.Application.DTOs.Violation;
using ExamSystem.Application.Services;
using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Interfaces.Repositories;
using Moq;

public class ViolationServiceTests
{
    private readonly Mock<IViolationRepository> _violationRepoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly ViolationService _service;

    public ViolationServiceTests()
    {
        _service = new ViolationService(_mapperMock.Object, _violationRepoMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnViolation_WhenFound()
    {
        var id = Guid.NewGuid();
        var violation = new Violation { ViolationId = id };
        var dto = new ViolationDto { ViolationId = id };

        _violationRepoMock.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(RepositoryOperationResult<Violation>.Ok(violation));
        _mapperMock.Setup(m => m.Map<ViolationDto>(violation)).Returns(dto);

        var result = await _service.GetByIdAsync(id);

        Assert.True(result.Success);
        Assert.Equal(id, result.Data!.ViolationId);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldFail_WhenNotFound()
    {
        var id = Guid.NewGuid();
        _violationRepoMock.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(RepositoryOperationResult<Violation>.Fail("Not found"));

        var result = await _service.GetByIdAsync(id);

        Assert.False(result.Success);
        Assert.Equal(ServiceOperationErrorType.NotFound, result.ErrorType);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnViolations()
    {
        var violations = new List<Violation> { new Violation() };
        var dtos = new List<ViolationDto> { new ViolationDto() };

        _violationRepoMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(RepositoryOperationResult<IEnumerable<Violation>>.Ok(violations));
        _mapperMock.Setup(m => m.Map<IEnumerable<ViolationDto>>(violations)).Returns(dtos);

        var result = await _service.GetAllAsync();

        Assert.True(result.Success);
        Assert.Single(result.Data!);
    }

    [Fact]
    public async Task GetAllAsync_ShouldFail_WhenRepoFails()
    {
        _violationRepoMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(RepositoryOperationResult<IEnumerable<Violation>>.Fail("Error"));

        var result = await _service.GetAllAsync();

        Assert.False(result.Success);
        Assert.Equal(ServiceOperationErrorType.Internal, result.ErrorType);
    }

    [Fact]
    public async Task GetAllByExamUserIdAsync_ShouldReturnViolations_WhenValid()
    {
        var examUserId = Guid.NewGuid();
        var violations = new List<Violation> { new Violation() };
        var dtos = new List<ViolationDto> { new ViolationDto() };

        _violationRepoMock.Setup(r => r.GetAllByExamUserIdAsync(examUserId))
            .ReturnsAsync(RepositoryOperationResult<IEnumerable<Violation>>.Ok(violations));
        _mapperMock.Setup(m => m.Map<IEnumerable<ViolationDto>>(violations)).Returns(dtos);

        var result = await _service.GetAllByExamUserIdAsync(examUserId);

        Assert.True(result.Success);
        Assert.Single(result.Data!);
    }

    [Fact]
    public async Task GetAllByExamUserIdAsync_ShouldFail_WhenRepoFails()
    {
        var examUserId = Guid.NewGuid();

        _violationRepoMock.Setup(r => r.GetAllByExamUserIdAsync(examUserId))
            .ReturnsAsync(RepositoryOperationResult<IEnumerable<Violation>>.Fail("Not found"));

        var result = await _service.GetAllByExamUserIdAsync(examUserId);

        Assert.False(result.Success);
        Assert.Equal(ServiceOperationErrorType.NotFound, result.ErrorType);
    }

    [Fact]
    public async Task DeleteAsync_ShouldSucceed_WhenDeleted()
    {
        var id = Guid.NewGuid();

        _violationRepoMock.Setup(r => r.DeleteAsync(id))
            .ReturnsAsync(RepositoryOperationResult.Ok());

        var result = await _service.DeleteAsync(id);

        Assert.True(result.Success);
    }

    [Fact]
    public async Task DeleteAsync_ShouldFail_WhenRepoFails()
    {
        var id = Guid.NewGuid();

        _violationRepoMock.Setup(r => r.DeleteAsync(id))
            .ReturnsAsync(RepositoryOperationResult.Fail("Delete error"));

        var result = await _service.DeleteAsync(id);

        Assert.False(result.Success);
        Assert.Equal(ServiceOperationErrorType.Internal, result.ErrorType);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateViolation_WhenValid()
    {
        var createDto = new CreateViolationDto();
        var violation = new Violation();
        var dto = new ViolationDto();

        _mapperMock.Setup(m => m.Map<Violation>(createDto)).Returns(violation);
        _violationRepoMock.Setup(r => r.AddAsync(It.IsAny<Violation>()))
            .ReturnsAsync(RepositoryOperationResult.Ok());
        _mapperMock.Setup(m => m.Map<ViolationDto>(violation)).Returns(dto);

        var result = await _service.CreateAsync(createDto);

        Assert.True(result.Success);
    }

    [Fact]
    public async Task CreateAsync_ShouldFail_WhenRepoFails()
    {
        var createDto = new CreateViolationDto();
        var violation = new Violation();

        _mapperMock.Setup(m => m.Map<Violation>(createDto)).Returns(violation);
        _violationRepoMock.Setup(r => r.AddAsync(It.IsAny<Violation>()))
            .ReturnsAsync(RepositoryOperationResult.Fail("Create error"));

        var result = await _service.CreateAsync(createDto);

        Assert.False(result.Success);
        Assert.Equal(ServiceOperationErrorType.Internal, result.ErrorType);
    }
}
