using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Persistence.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly ExamSystemDbContext _dbContext;
        public AnswerRepository(ExamSystemDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<OperationResult<IEnumerable<Answer>>> GetAllAsync()
        {
            var answers = await _dbContext.Answers.Include(a => a.QuestionOption)
                                                  .Include(a => a.User)
                                                  .ToListAsync();
            return OperationResult<IEnumerable<Answer>>.Ok(answers);
        }
        public async Task<OperationResult<Answer>> GetByIdAsync(Guid id)
        {
            var answer = await _dbContext.Answers.Include(a => a.QuestionOption)
                                                 .Include(a => a.User)
                                                 .FirstOrDefaultAsync();
            if (answer == null)
                return OperationResult<Answer>.Fail("Answer not found.");
            return OperationResult<Answer>.Ok(answer);
        }

        public async Task<OperationResult> AddAsync(Answer entity)
        {
            await _dbContext.Answers.AddAsync(entity);
            var result = await _dbContext.SaveChangesAsync();

            if (result == 0)
                return OperationResult.Fail("Failed to add answer.");

            return OperationResult.Ok();
        }
        public async Task<OperationResult> UpdateAsync(Answer entity)
        {
            _dbContext.Answers.Update(entity);
            var result = await _dbContext.SaveChangesAsync();
            if (result == 0)
                return OperationResult.Fail("Failed to update answer.");
            return OperationResult.Ok();
        }

        public async Task<OperationResult> DeleteAsync(Guid id)
        {
            var answer = await _dbContext.Answers.FindAsync(id);
            if (answer == null)
                return OperationResult.Fail("Answer not found.");
            _dbContext.Answers.Remove(answer);
            var result = await _dbContext.SaveChangesAsync();
            if (result == 0)
                return OperationResult.Fail("Failed to delete answer.");
            return OperationResult.Ok();
        }
    }
}
