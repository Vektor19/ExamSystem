using ExamSystem.Application.DTOs.QuestionOption;
using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IQuestionOptionService
    {
        Task<ServiceOperationResult<QuestionOptionDto>> GetByIdAsync(Guid questionOptionId);
        Task<ServiceOperationResult<IEnumerable<QuestionOptionDto>>> GetAllAsync();
        Task<ServiceOperationResult<IEnumerable<QuestionOptionDto>>> GetAllByQuestionIdAsync(Guid questionId);
        Task<ServiceOperationResult> UpdateAsync(Guid questionOptionId, QuestionOptionUpdateDto questionUpdateDto);
        Task<ServiceOperationResult> DeleteAsync(Guid questionOptionId);
        Task<ServiceOperationResult> CreateAsync(QuestionOptionCreateDto questionOptionCreateDto);
    }
}
