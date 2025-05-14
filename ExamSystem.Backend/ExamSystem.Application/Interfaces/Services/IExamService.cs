using ExamSystem.Application.DTOs;
using ExamSystem.Core.Common;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IExamService
    {
        Task<OperationResult<ExamForExaminatorDto>> GetByIdAsync(Guid examId);
        Task<OperationResult<ExamUserDto>> GetExamUserByIdAsync(Guid examUserId);
        Task<OperationResult<IEnumerable<ExamForExaminatorDto>>> GetAllAsync();
        Task<OperationResult<IEnumerable<ExamForExaminatorDto>>> GetAllByCreatedUserIdAsync(Guid createdByUserId);
        Task<OperationResult<IEnumerable<ExamForStudentDto>>> GetAllByParticipantUserIdAsync(Guid participantUserId);
        Task<RepositoryOperationResult> AddParticipantAsync(Guid examId, Guid userId);
        Task<RepositoryOperationResult> RemoveParticipantAsync(Guid examId, Guid userId);
        Task<RepositoryOperationResult> AddParticipantByEmailAsync(Guid examId, string email);
        Task<RepositoryOperationResult> UpdateAsync(Guid examId, ExamUpdateDto examUpdateDto);
        Task<RepositoryOperationResult> DeleteAsync(Guid examId);
        Task<OperationResult<ExamForExaminatorDto>> CreateAsync(ExamCreateDto createExamDto);
        Task<OperationResult<bool>> IsParticipantAsync(Guid examId, Guid userId);
        Task<RepositoryOperationResult> JoinExam(JoinExamDto joinExamDto);
        Task<RepositoryOperationResult> FinishExamAsync(Guid examId, Guid userId);
        Task<RepositoryOperationResult> BlockExamUserByIdAsync(Guid examUserId);
        Task<OperationResult<bool>> IsUserBlockedInExamAsync(Guid examId, Guid userId);
        Task<OperationResult<bool>> IsExamInProgressAsync(Guid examId);
        Task<OperationResult<IEnumerable<ExamUserDto>>> GetExpiredNotFinishedExamUsersAsync(DateTime now);
    }
}
