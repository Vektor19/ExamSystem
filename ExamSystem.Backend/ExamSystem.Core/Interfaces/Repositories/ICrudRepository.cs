using ExamSystem.Core.Common;

namespace ExamSystem.Core.Interfaces.Repositories
{
    public interface ICrudRepository<T>
    {
        Task<RepositoryOperationResult<T>> GetByIdAsync(Guid id);
        Task<RepositoryOperationResult<IEnumerable<T>>> GetAllAsync();
        Task<RepositoryOperationResult> AddAsync(T entity);
        Task<RepositoryOperationResult> UpdateAsync(T entity);
        Task<RepositoryOperationResult> DeleteAsync(Guid id);
    }
}
