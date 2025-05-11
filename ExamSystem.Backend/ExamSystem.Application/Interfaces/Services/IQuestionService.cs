using ExamSystem.Application.DTOs;
using ExamSystem.Core.Common;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IQuestionService
    {
        Task<OperationResult<QuestionDto>> GetByIdAsync(Guid questionId);
        Task<OperationResult<IEnumerable<QuestionDto>>> GetAllAsync();
        Task<OperationResult<IEnumerable<QuestionDto>>> GetAllByExamIdAsync(Guid examId);
        Task<OperationResult> UpdateAsync(Guid questionId, QuestionUpdateDto questionUpdateDto);
        Task<OperationResult> DeleteAsync(Guid questionId);
        Task<OperationResult> CreateAsync(QuestionCreateDto questionCreateDto);
        Task<OperationResult<IEnumerable<QuestionDto>>> GetAllUnansweredByUserAsync(Guid userId, Guid examId);
        Task<OperationResult> GradeTextQuestionAnswerAsync(Guid questionId, GradeOpenAnswerDto gradeOpenAnswerDto);
    }
}
