using ExamSystem.Application.DTOs.QuestionOption;
using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IQuestionOptionService
    {
        Task<RepositoryOperationResult<QuestionOptionDto>> GetByIdAsync(Guid questionOptionId);
        Task<RepositoryOperationResult<IEnumerable<QuestionOptionDto>>> GetAllAsync();
        Task<RepositoryOperationResult<IEnumerable<QuestionOptionDto>>> GetAllByQuestionIdAsync(Guid questionId);
        Task<RepositoryOperationResult> UpdateAsync(Guid questionOptionId, QuestionOptionUpdateDto questionUpdateDto);
        Task<RepositoryOperationResult> DeleteAsync(Guid questionOptionId);
        Task<RepositoryOperationResult> CreateAsync(QuestionOptionCreateDto questionOptionCreateDto);
    }
}
