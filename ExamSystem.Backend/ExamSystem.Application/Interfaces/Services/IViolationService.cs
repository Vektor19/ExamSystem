using ExamSystem.Application.DTOs.Violation;
using ExamSystem.Application.Common.Models;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IViolationService
    {
        Task<ServiceOperationResult<ViolationDto>> GetByIdAsync(Guid violationId);
        Task<ServiceOperationResult<IEnumerable<ViolationDto>>> GetAllAsync();
        Task<ServiceOperationResult<IEnumerable<ViolationDto>>> GetAllByExamUserIdAsync(Guid examUserId);
        Task<ServiceOperationResult> DeleteAsync(Guid violationId);
        Task<ServiceOperationResult<ViolationDto>> CreateAsync(CreateViolationDto createViolationDto);
    }
}
