using ExamSystem.Application.DTOs.QuestionOption;
using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IQuestionOptionService
    {
        Task<OperationResult<QuestionOptionDto>> GetByIdAsync(Guid questionOptionId);
        Task<OperationResult<IEnumerable<QuestionOptionDto>>> GetAllAsync();
        Task<OperationResult<IEnumerable<QuestionOptionDto>>> GetAllByQuestionIdAsync(Guid questionId);
        Task<RepositoryOperationResult> UpdateAsync(Guid questionOptionId, QuestionOptionUpdateDto questionUpdateDto);
        Task<RepositoryOperationResult> DeleteAsync(Guid questionOptionId);
        Task<RepositoryOperationResult> CreateAsync(QuestionOptionCreateDto questionOptionCreateDto);
    }
}
