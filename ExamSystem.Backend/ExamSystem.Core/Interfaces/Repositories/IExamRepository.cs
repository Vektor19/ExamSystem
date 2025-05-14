using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;

namespace ExamSystem.Core.Interfaces.Repositories
{
    public interface IExamRepository : ICrudRepository<Exam>
    {
        Task<RepositoryOperationResult<ExamUser>> GetExamUserByIdAsync(Guid id);
        Task<RepositoryOperationResult<IEnumerable<ExamUser>>> GetExpiredNotFinishedExamUsersAsync(DateTime now);
    }
}
