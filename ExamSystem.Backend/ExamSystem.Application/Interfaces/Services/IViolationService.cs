using ExamSystem.Application.DTOs;
using ExamSystem.Core.Common;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IViolationService
    {
        Task<OperationResult<ViolationDto>> GetByIdAsync(Guid violationId);
        Task<OperationResult<IEnumerable<ViolationDto>>> GetAllAsync();
        Task<OperationResult<IEnumerable<ViolationDto>>> GetAllByExamUserIdAsync(Guid examUserId);
        Task<OperationResult> DeleteAsync(Guid violationId);
        Task<OperationResult<ViolationDto>> CreateAsync(CreateViolationDto createViolationDto);
    }
}
