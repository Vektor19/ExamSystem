using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;

namespace ExamSystem.Core.Interfaces.Repositories
{
    public interface IUserRepository : ICrudRepository<User>
    {
        Task<RepositoryOperationResult<User>> GetByEmailAsync(string email);
    }
}
