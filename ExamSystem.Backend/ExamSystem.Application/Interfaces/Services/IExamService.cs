using ExamSystem.Application.DTOs;
using ExamSystem.Core.Common;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IExamService
    {
        Task<OperationResult<ExamDto>> GetByIdAsync(Guid examId);
        Task<OperationResult<IEnumerable<ExamDto>>> GetAllAsync();
        Task<OperationResult<IEnumerable<ExamDto>>> GetAllByCreatedUserIdAsync(Guid createdByUserId);
        Task<OperationResult<IEnumerable<ExamDto>>> GetAllByParticipantUserIdAsync(Guid participantUserId);
        Task<OperationResult> UpdateAsync(Guid examId, ExamUpdateDto examUpdateDto);
        Task<OperationResult> DeleteAsync(Guid examId);
        Task<OperationResult> CreateAsync(ExamCreateDto createExamDto);
    }
}
