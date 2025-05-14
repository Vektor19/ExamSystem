using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Persistence.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ExamSystemDbContext _dbContext;

        public RoleRepository(ExamSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RepositoryOperationResult<IEnumerable<Role>>> GetAllAsync()
        {
            var roles = await _dbContext.Roles.ToListAsync();
            return RepositoryOperationResult<IEnumerable<Role>>.Ok(roles);
        }

        public async Task<RepositoryOperationResult<Role>> GetByIdAsync(Guid id)
        {
            var role = await _dbContext.Roles.FindAsync(id);
            if (role == null)
                return RepositoryOperationResult<Role>.Fail("Role not found.");
            return RepositoryOperationResult<Role>.Ok(role);
        }

        public async Task<RepositoryOperationResult> AddAsync(Role entity)
        {
            await _dbContext.Roles.AddAsync(entity);
            var result = await _dbContext.SaveChangesAsync();

            if (result == 0)
                return RepositoryOperationResult.Fail("Failed to add role.");

            return RepositoryOperationResult.Ok();
        }

        public async Task<RepositoryOperationResult> UpdateAsync(Role entity)
        {
            var existing = await _dbContext.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.RoleId == entity.RoleId);
            if (existing == null)
                return RepositoryOperationResult.Fail("Role not found.");

            if (existing.IsSystem)
                return RepositoryOperationResult.Fail("System role cannot be updated.");

            _dbContext.Roles.Update(entity);
            var result = await _dbContext.SaveChangesAsync();

            if (result == 0)
                return RepositoryOperationResult.Fail("Failed to update role.");

            return RepositoryOperationResult.Ok();
        }

        public async Task<RepositoryOperationResult> DeleteAsync(Guid id)
        {
            var role = await _dbContext.Roles.FindAsync(id);
            if (role == null)
                return RepositoryOperationResult.Fail("Role not found.");

            if (role.IsSystem)
                return RepositoryOperationResult.Fail("System role cannot be deleted.");

            _dbContext.Roles.Remove(role);
            var result = await _dbContext.SaveChangesAsync();

            if (result == 0)
                return RepositoryOperationResult.Fail("Failed to delete role.");

            return RepositoryOperationResult.Ok();
        }

        public async Task<RepositoryOperationResult<IEnumerable<Role>>> GetRolesByNamesAsync(IEnumerable<string> names)
        {
            var roles = await _dbContext.Roles
                                        .Where(r => names.Contains(r.Name))
                                        .ToListAsync();

            return RepositoryOperationResult<IEnumerable<Role>>.Ok(roles);
        }

    }
}
