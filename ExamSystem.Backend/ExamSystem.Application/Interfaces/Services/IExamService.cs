using ExamSystem.Application.DTOs;
using ExamSystem.Core.Common;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IExamService
    {
        Task<RepositoryOperationResult<ExamForExaminatorDto>> GetByIdAsync(Guid examId);
        Task<RepositoryOperationResult<ExamUserDto>> GetExamUserByIdAsync(Guid examUserId);
        Task<RepositoryOperationResult<IEnumerable<ExamForExaminatorDto>>> GetAllAsync();
        Task<RepositoryOperationResult<IEnumerable<ExamForExaminatorDto>>> GetAllByCreatedUserIdAsync(Guid createdByUserId);
        Task<RepositoryOperationResult<IEnumerable<ExamForStudentDto>>> GetAllByParticipantUserIdAsync(Guid participantUserId);
        Task<RepositoryOperationResult> AddParticipantAsync(Guid examId, Guid userId);
        Task<RepositoryOperationResult> RemoveParticipantAsync(Guid examId, Guid userId);
        Task<RepositoryOperationResult> AddParticipantByEmailAsync(Guid examId, string email);
        Task<RepositoryOperationResult> UpdateAsync(Guid examId, ExamUpdateDto examUpdateDto);
        Task<RepositoryOperationResult> DeleteAsync(Guid examId);
        Task<RepositoryOperationResult<ExamForExaminatorDto>> CreateAsync(ExamCreateDto createExamDto);
        Task<RepositoryOperationResult<bool>> IsParticipantAsync(Guid examId, Guid userId);
        Task<RepositoryOperationResult> JoinExam(JoinExamDto joinExamDto);
        Task<RepositoryOperationResult> FinishExamAsync(Guid examId, Guid userId);
        Task<RepositoryOperationResult> BlockExamUserByIdAsync(Guid examUserId);
        Task<RepositoryOperationResult<bool>> IsUserBlockedInExamAsync(Guid examId, Guid userId);
        Task<RepositoryOperationResult<bool>> IsExamInProgressAsync(Guid examId);
        Task<RepositoryOperationResult<IEnumerable<ExamUserDto>>> GetExpiredNotFinishedExamUsersAsync(DateTime now);
    }
}
