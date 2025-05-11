using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;

namespace ExamSystem.Core.Interfaces.Repositories
{
    public interface IAnswerRepository : ICrudRepository<Answer>
    {
        Task<OperationResult<IEnumerable<Answer>>> GetAllByExamUserIdAsync(Guid examUserId);
    }
}
