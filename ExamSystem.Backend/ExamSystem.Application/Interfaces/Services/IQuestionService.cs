using ExamSystem.Application.DTOs;
using ExamSystem.Application.DTOs.Question;
using ExamSystem.Core.Common;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IQuestionService
    {
        Task<RepositoryOperationResult<QuestionDto>> GetByIdAsync(Guid questionId);
        Task<RepositoryOperationResult<IEnumerable<QuestionDto>>> GetAllAsync();
        Task<RepositoryOperationResult<IEnumerable<QuestionDto>>> GetAllByExamIdAsync(Guid examId);
        Task<RepositoryOperationResult> UpdateAsync(Guid questionId, QuestionUpdateDto questionUpdateDto);
        Task<RepositoryOperationResult> DeleteAsync(Guid questionId);
        Task<RepositoryOperationResult> CreateAsync(QuestionCreateDto questionCreateDto);
        Task<RepositoryOperationResult<IEnumerable<QuestionDto>>> GetAllUnansweredByUserAsync(Guid userId, Guid examId);
        Task<RepositoryOperationResult> GradeTextQuestionAnswerAsync(Guid questionId, GradeOpenAnswerDto gradeOpenAnswerDto);
    }
}
