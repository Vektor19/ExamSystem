using ExamSystem.Application.DTOs;
using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IQuestionOptionService
    {
        Task<OperationResult<QuestionOptionDto>> GetByIdAsync(Guid questionOptionId);
        Task<OperationResult<IEnumerable<QuestionOptionDto>>> GetAllAsync();
        Task<OperationResult<IEnumerable<QuestionOptionDto>>> GetAllByQuestionIdAsync(Guid questionId);
        Task<OperationResult> UpdateAsync(Guid questionOptionId, QuestionOptionUpdateDto questionUpdateDto);
        Task<OperationResult> DeleteAsync(Guid questionOptionId);
        Task<OperationResult> CreateAsync(QuestionOptionCreateDto questionOptionCreateDto);
    }
}
