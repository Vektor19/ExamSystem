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
        Task<OperationResult> AddParticipantAsync(Guid examId, Guid userId);
        Task<OperationResult> RemoveParticipantAsync(Guid examId, Guid userId);
        Task<OperationResult> AddParticipantByEmailAsync(Guid examId, string email);
        Task<OperationResult> UpdateAsync(Guid examId, ExamUpdateDto examUpdateDto);
        Task<OperationResult> DeleteAsync(Guid examId);
        Task<OperationResult<ExamDto>> CreateAsync(ExamCreateDto createExamDto);
        Task<OperationResult<bool>> IsParticipantAsync(Guid examId, Guid userId);
    }
}
