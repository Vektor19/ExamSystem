using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;

namespace ExamSystem.Core.Interfaces.Repositories
{
    public interface IViolationRepository : ICrudRepository<Violation>
    {
        Task<RepositoryOperationResult<IEnumerable<Violation>>> GetAllByExamUserIdAsync(Guid examUserId);
    }
}
