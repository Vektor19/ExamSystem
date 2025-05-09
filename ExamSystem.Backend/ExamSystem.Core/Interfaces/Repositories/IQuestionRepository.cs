using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;

namespace ExamSystem.Core.Interfaces.Repositories
{
    public interface IQuestionRepository : ICrudRepository<Question>
    {
        Task<OperationResult<IEnumerable<Question>>> GetUnansweredByUserAsync(Guid examId, Guid userId);
    }
}
