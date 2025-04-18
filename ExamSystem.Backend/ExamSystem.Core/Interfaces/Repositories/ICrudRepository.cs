using ExamSystem.Core.Common;

namespace ExamSystem.Core.Interfaces.Repositories
{
    public interface ICrudRepository<T>
    {
        Task<OperationResult<T>> GetByIdAsync(Guid id);
        Task<OperationResult<IEnumerable<T>>> GetAllAsync();
        Task<OperationResult> AddAsync(T entity);
        Task<OperationResult> UpdateAsync(T entity);
        Task<OperationResult> DeleteAsync(Guid id);
    }
}
