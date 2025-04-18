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
                                              .Include(e => e.UserCreatedBy)
                                              .Include(e => e.Questions)
                                              .ToListAsync();
            return OperationResult<IEnumerable<Exam>>.Ok(exams);
        }
        public async Task<OperationResult<Exam>> GetByIdAsync(Guid id)
        {
            var exam = await _dbContext.Exams.Include(e => e.ExamUsers)
                                             .Include(e => e.UserCreatedBy)
                                             .Include(e => e.Questions)
                                             .FirstOrDefaultAsync();
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
    }
}
