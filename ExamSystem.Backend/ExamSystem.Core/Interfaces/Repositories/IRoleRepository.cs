using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;

namespace ExamSystem.Core.Interfaces.Repositories
{
    public interface IRoleRepository : ICrudRepository<Role>
    {
        Task<RepositoryOperationResult<IEnumerable<Role>>> GetRolesByNamesAsync(IEnumerable<string> names);
    }
}
