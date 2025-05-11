using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Persistence.Repositories
{
    public class ExamRepository : IExamRepository
    {
        private readonly ExamSystemDbContext _dbContext;
        public ExamRepository(ExamSystemDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<OperationResult<IEnumerable<Exam>>> GetAllAsync()
        {
            var exams = await _dbContext.Exams.Include(e => e.ExamUsers)
                                                .ThenInclude(eu => eu.User)
                                              .Include(e => e.UserCreatedBy)
                                              .Include(e => e.Questions)
                                              .Include(e => e.Answers)
                                              .ToListAsync();
            return OperationResult<IEnumerable<Exam>>.Ok(exams);
        }
        public async Task<OperationResult<Exam>> GetByIdAsync(Guid id)
        {
            var exam = await _dbContext.Exams.Include(e => e.ExamUsers)
                                                .ThenInclude(eu => eu.User)
                                             .Include(e => e.UserCreatedBy)
                                             .Include(e => e.Questions)
                                                .ThenInclude(q => q.QuestionOptions)
                                             .Include(e => e.Questions)
                                                .ThenInclude(q => q.Answers)
                                                    .ThenInclude(a => a.QuestionOption)
                                             .FirstOrDefaultAsync(e => e.ExamId == id);
            if (exam == null)
                return OperationResult<Exam>.Fail("Exam not found.");
            return OperationResult<Exam>.Ok(exam);
        }

        public async Task<OperationResult> AddAsync(Exam entity)
        {
            await _dbContext.Exams.AddAsync(entity);
            var result = await _dbContext.SaveChangesAsync();

            if (result == 0)
                return OperationResult.Fail("Failed to add exam.");

            return OperationResult.Ok();
        }
        public async Task<OperationResult> UpdateAsync(Exam entity)
        {
            _dbContext.Exams.Update(entity);
            var result = await _dbContext.SaveChangesAsync();
            if (result == 0)
                return OperationResult.Fail("Failed to update exam.");
            return OperationResult.Ok();
        }

        public async Task<OperationResult> DeleteAsync(Guid id)
        {
            var exam = await _dbContext.Exams.FindAsync(id);
            if (exam == null)
                return OperationResult.Fail("Exam not found.");
            _dbContext.Exams.Remove(exam);
            var result = await _dbContext.SaveChangesAsync();
            if (result == 0)
                return OperationResult.Fail("Failed to delete exam.");
            return OperationResult.Ok();
        }

        public async Task<OperationResult<ExamUser>> GetExamUserByIdAsync(Guid id)
        {
            var examUser = await _dbContext.ExamUsers.Include(eu => eu.User)
                                                     .Include(eu => eu.Exam)
                                                     .Include(eu => eu.Violations)
                                                     .FirstOrDefaultAsync(eu => eu.ExamUserId == id);
            if (examUser == null)
                return OperationResult<ExamUser>.Fail("Exam user not found.");
            return OperationResult<ExamUser>.Ok(examUser);
        }
        public async Task<OperationResult<IEnumerable<ExamUser>>> GetExpiredNotFinishedExamUsersAsync(DateTime now)
        {
            var expiredExamUsers = await _dbContext.Exams
                .Where(e => e.EndDate < now)
                .SelectMany(e => e.ExamUsers
                    .Where(eu => !eu.CompleteStatus))
                .ToListAsync();

            return OperationResult<IEnumerable<ExamUser>>.Ok(expiredExamUsers);
        }
    }
}
