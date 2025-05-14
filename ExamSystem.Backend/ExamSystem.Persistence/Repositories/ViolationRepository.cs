using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Persistence.Repositories
{
    public class ViolationRepository : IViolationRepository
    {
        private readonly ExamSystemDbContext _dbContext;
        public ViolationRepository(ExamSystemDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<OperationResult<IEnumerable<Violation>>> GetAllAsync()
        {
            var violations = await _dbContext.Violations.Include(v => v.ExamUser)
                                                        .ToListAsync();
            return OperationResult<IEnumerable<Violation>>.Ok(violations);
        }
        public async Task<OperationResult<Violation>> GetByIdAsync(Guid id)
        {
            var violation = await _dbContext.Violations.Include(v => v.ExamUser)
                                             .FirstOrDefaultAsync(v => v.ViolationId == id);
            if (violation == null)
                return OperationResult<Violation>.Fail("Violation not found.");
            return OperationResult<Violation>.Ok(violation);
        }
        public async Task<OperationResult<IEnumerable<Violation>>> GetAllByExamUserIdAsync(Guid examUserId)
        {
            var violations = await _dbContext.Violations.Include(v => v.ExamUser)
                                                        .Where(v => v.ExamUserId == examUserId)
                                                        .ToListAsync();
            return OperationResult<IEnumerable<Violation>>.Ok(violations);
        }
        public async Task<RepositoryOperationResult> AddAsync(Violation entity)
        {
            await _dbContext.Violations.AddAsync(entity);
            var result = await _dbContext.SaveChangesAsync();

            if (result == 0)
                return RepositoryOperationResult.Fail("Failed to add violation.");

            return RepositoryOperationResult.Ok();
        }
        public async Task<RepositoryOperationResult> UpdateAsync(Violation entity)
        {
            _dbContext.Violations.Update(entity);
            var result = await _dbContext.SaveChangesAsync();
            if (result == 0)
                return RepositoryOperationResult.Fail("Failed to update violation.");
            return RepositoryOperationResult.Ok();
        }

        public async Task<RepositoryOperationResult> DeleteAsync(Guid id)
        {
            var violation = await _dbContext.Violations.FindAsync(id);
            if (violation == null)
                return RepositoryOperationResult.Fail("Violation not found.");
            _dbContext.Violations.Remove(violation);
            var result = await _dbContext.SaveChangesAsync();
            if (result == 0)
                return RepositoryOperationResult.Fail("Failed to delete violation.");
            return RepositoryOperationResult.Ok();
        }
    }
}
