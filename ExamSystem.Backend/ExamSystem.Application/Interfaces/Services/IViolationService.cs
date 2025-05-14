using ExamSystem.Application.DTOs.Violation;
using ExamSystem.Core.Common;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IViolationService
    {
        Task<RepositoryOperationResult<ViolationDto>> GetByIdAsync(Guid violationId);
        Task<RepositoryOperationResult<IEnumerable<ViolationDto>>> GetAllAsync();
        Task<RepositoryOperationResult<IEnumerable<ViolationDto>>> GetAllByExamUserIdAsync(Guid examUserId);
        Task<RepositoryOperationResult> DeleteAsync(Guid violationId);
        Task<RepositoryOperationResult<ViolationDto>> CreateAsync(CreateViolationDto createViolationDto);
    }
}
