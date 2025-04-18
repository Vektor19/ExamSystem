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

        public async Task<OperationResult<IEnumerable<Role>>> GetAllAsync()
        {
            var roles = await _dbContext.Roles.ToListAsync();
            return OperationResult<IEnumerable<Role>>.Ok(roles);
        }

        public async Task<OperationResult<Role>> GetByIdAsync(Guid id)
        {
            var role = await _dbContext.Roles.FindAsync(id);
            if (role == null)
                return OperationResult<Role>.Fail("Role not found.");
            return OperationResult<Role>.Ok(role);
        }

        public async Task<OperationResult> AddAsync(Role entity)
        {
            await _dbContext.Roles.AddAsync(entity);
            var result = await _dbContext.SaveChangesAsync();

            if (result == 0)
                return OperationResult.Fail("Failed to add role.");

            return OperationResult.Ok();
        }

        public async Task<OperationResult> UpdateAsync(Role entity)
        {
            var existing = await _dbContext.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.RoleId == entity.RoleId);
            if (existing == null)
                return OperationResult.Fail("Role not found.");

            if (existing.IsSystem)
                return OperationResult.Fail("System role cannot be updated.");

            _dbContext.Roles.Update(entity);
            var result = await _dbContext.SaveChangesAsync();

            if (result == 0)
                return OperationResult.Fail("Failed to update role.");

            return OperationResult.Ok();
        }

        public async Task<OperationResult> DeleteAsync(Guid id)
        {
            var role = await _dbContext.Roles.FindAsync(id);
            if (role == null)
                return OperationResult.Fail("Role not found.");

            if (role.IsSystem)
                return OperationResult.Fail("System role cannot be deleted.");

            _dbContext.Roles.Remove(role);
            var result = await _dbContext.SaveChangesAsync();

            if (result == 0)
                return OperationResult.Fail("Failed to delete role.");

            return OperationResult.Ok();
        }
    }
}
