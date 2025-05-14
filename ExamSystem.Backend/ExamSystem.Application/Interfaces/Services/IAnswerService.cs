using ExamSystem.Application.DTOs;
using ExamSystem.Application.Common.Models;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IAnswerService
    {
        Task<ServiceOperationResult<AnswerDto>> GetByIdAsync(Guid answerId);
        Task<ServiceOperationResult<IEnumerable<AnswerDto>>> GetAllAsync();
        Task<ServiceOperationResult<IEnumerable<AnswerDto>>> GetAllByExamIdAsync(Guid examId);
        Task<ServiceOperationResult<IEnumerable<AnswerDto>>> GetAllByExamUserIdAsync(Guid examUserId);
        Task<ServiceOperationResult<IEnumerable<AnswerDto>>> GetAllByUserIdAsync(Guid userId);
        Task<ServiceOperationResult> DeleteAsync(Guid answerId);
        Task<ServiceOperationResult> CreateOpenAnswerAsync(CreateAnswerDto createAnswerDto);
        Task<ServiceOperationResult> CreateOptionAnswerAsync(CreateAnswerDto createAnswerDto);

    }
}
