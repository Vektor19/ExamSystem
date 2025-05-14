using ExamSystem.Application.DTOs;
using ExamSystem.Application.Common.Models;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IExamService
    {
        Task<ServiceOperationResult<ExamForExaminatorDto>> GetByIdAsync(Guid examId);
        Task<ServiceOperationResult<ExamUserDto>> GetExamUserByIdAsync(Guid examUserId);
        Task<ServiceOperationResult<IEnumerable<ExamForExaminatorDto>>> GetAllAsync();
        Task<ServiceOperationResult<IEnumerable<ExamForExaminatorDto>>> GetAllByCreatedUserIdAsync(Guid createdByUserId);
        Task<ServiceOperationResult<IEnumerable<ExamForStudentDto>>> GetAllByParticipantUserIdAsync(Guid participantUserId);
        Task<ServiceOperationResult> AddParticipantAsync(Guid examId, Guid userId);
        Task<ServiceOperationResult> RemoveParticipantAsync(Guid examId, Guid userId);
        Task<ServiceOperationResult> AddParticipantByEmailAsync(Guid examId, string email);
        Task<ServiceOperationResult> UpdateAsync(Guid examId, ExamUpdateDto examUpdateDto);
        Task<ServiceOperationResult> DeleteAsync(Guid examId);
        Task<ServiceOperationResult<ExamForExaminatorDto>> CreateAsync(ExamCreateDto createExamDto);
        Task<ServiceOperationResult<bool>> IsParticipantAsync(Guid examId, Guid userId);
        Task<ServiceOperationResult> JoinExam(JoinExamDto joinExamDto);
        Task<ServiceOperationResult> FinishExamAsync(Guid examId, Guid userId);
        Task<ServiceOperationResult> BlockExamUserByIdAsync(Guid examUserId);
        Task<ServiceOperationResult<bool>> IsUserBlockedInExamAsync(Guid examId, Guid userId);
        Task<ServiceOperationResult<bool>> IsExamInProgressAsync(Guid examId);
        Task<ServiceOperationResult<IEnumerable<ExamUserDto>>> GetExpiredNotFinishedExamUsersAsync(DateTime now);
    }
}
