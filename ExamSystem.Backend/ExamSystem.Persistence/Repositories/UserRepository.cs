using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ExamSystemDbContext _dbContext;
        public UserRepository(ExamSystemDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<OperationResult<IEnumerable<User>>> GetAllAsync()
        {
            var users = await _dbContext.Users.Include(u => u.UserRoles)
                                                .ThenInclude(ur => ur.Role)
                                              .Include(u => u.ExamUsers)
                                              .Include(u => u.CreatedExams)
                                              .Include(u => u.Answers)
                                              .ToListAsync();
            return OperationResult<IEnumerable<User>>.Ok(users);
        }
        public async Task<OperationResult<User>> GetByIdAsync(Guid id)
        {
            var user = await _dbContext.Users.Include(u => u.UserRoles)
                                                .ThenInclude(ur => ur.Role)
                                             .Include(u => u.ExamUsers)
                                             .Include(u => u.CreatedExams)
                                             .Include(u => u.Answers)
                                             .FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
                return OperationResult<User>.Fail("User not found.");
            return OperationResult<User>.Ok(user);
        }

        public async Task<OperationResult> AddAsync(User entity)
        {
            await _dbContext.Users.AddAsync(entity);
            var result = await _dbContext.SaveChangesAsync();

            if (result == 0)
                return OperationResult.Fail("Failed to add user.");

            return OperationResult.Ok();
        }
        public async Task<OperationResult> UpdateAsync(User entity)
        {
            _dbContext.Users.Update(entity);
            var result = await _dbContext.SaveChangesAsync();
            if (result == 0)
                return OperationResult.Fail("Failed to update user.");
            return OperationResult.Ok();
        }

        public async Task<OperationResult> DeleteAsync(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
                return OperationResult.Fail("User not found.");
            _dbContext.Users.Remove(user);
            var result = await _dbContext.SaveChangesAsync();
            if (result == 0)
                return OperationResult.Fail("Failed to delete user.");
            return OperationResult.Ok();
        }
        public async Task<OperationResult<User>> GetByEmailAsync(string email)
        {
            var user = await _dbContext.Users.Include(u => u.UserRoles)
                                                .ThenInclude(ur => ur.Role)
                                             .Include(u => u.ExamUsers)
                                             .Include(u => u.CreatedExams)
                                             .Include(u => u.Answers)
                                             .FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return OperationResult<User>.Fail("User not found.");
            return OperationResult<User>.Ok(user);
        }
    }
}
