using ExamSystem.Application.DTOs;
using ExamSystem.Application.DTOs.Question;
using ExamSystem.Core.Common;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IQuestionService
    {
        Task<ServiceOperationResult<QuestionDto>> GetByIdAsync(Guid questionId);
        Task<ServiceOperationResult<IEnumerable<QuestionDto>>> GetAllAsync();
        Task<ServiceOperationResult<IEnumerable<QuestionDto>>> GetAllByExamIdAsync(Guid examId);
        Task<ServiceOperationResult> UpdateAsync(Guid questionId, QuestionUpdateDto questionUpdateDto);
        Task<ServiceOperationResult> DeleteAsync(Guid questionId);
        Task<ServiceOperationResult> CreateAsync(QuestionCreateDto questionCreateDto);
        Task<ServiceOperationResult<IEnumerable<QuestionDto>>> GetAllUnansweredByUserAsync(Guid userId, Guid examId);
        Task<ServiceOperationResult> GradeTextQuestionAnswerAsync(Guid questionId, GradeOpenAnswerDto gradeOpenAnswerDto);
    }
}
