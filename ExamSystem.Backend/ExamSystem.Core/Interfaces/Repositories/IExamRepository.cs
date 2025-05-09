using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;

namespace ExamSystem.Core.Interfaces.Repositories
{
    public interface IExamRepository : ICrudRepository<Exam>
    {
        Task<OperationResult<ExamUser>> GetExamUserByIdAsync(Guid id);
    }
}
