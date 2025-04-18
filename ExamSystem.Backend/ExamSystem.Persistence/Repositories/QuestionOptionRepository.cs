using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Persistence.Repositories
{
    public class QuestionOptionRepository : IQuestionOptionRepository
    {
        private readonly ExamSystemDbContext _dbContext;
        public QuestionOptionRepository(ExamSystemDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<OperationResult<IEnumerable<QuestionOption>>> GetAllAsync()
        {
            var questionOptions = await _dbContext.QuestionOptions.Include(qo => qo.Question)
                                                                  .Include(qo => qo.Answers)
                                                                  .ToListAsync();
            return OperationResult<IEnumerable<QuestionOption>>.Ok(questionOptions);
        }
        public async Task<OperationResult<QuestionOption>> GetByIdAsync(Guid id)
        {
            var questionOption = await _dbContext.QuestionOptions.Include(qo => qo.Question)
                                                                 .Include(qo => qo.Answers)
                                                                 .FirstOrDefaultAsync();
            if (questionOption == null)
                return OperationResult<QuestionOption>.Fail("Question option not found.");
            return OperationResult<QuestionOption>.Ok(questionOption);
        }

        public async Task<OperationResult> AddAsync(QuestionOption entity)
        {
            await _dbContext.QuestionOptions.AddAsync(entity);
            var result = await _dbContext.SaveChangesAsync();

            if (result == 0)
                return OperationResult.Fail("Failed to add question option.");

            return OperationResult.Ok();
        }
        public async Task<OperationResult> UpdateAsync(QuestionOption entity)
        {
            _dbContext.QuestionOptions.Update(entity);
            var result = await _dbContext.SaveChangesAsync();
            if (result == 0)
                return OperationResult.Fail("Failed to update question option.");
            return OperationResult.Ok();
        }

        public async Task<OperationResult> DeleteAsync(Guid id)
        {
            var questionOption = await _dbContext.QuestionOptions.FindAsync(id);
            if (questionOption == null)
                return OperationResult.Fail("Question option not found.");
            _dbContext.QuestionOptions.Remove(questionOption);
            var result = await _dbContext.SaveChangesAsync();
            if (result == 0)
                return OperationResult.Fail("Failed to delete question option.");
            return OperationResult.Ok();
        }
    }
}
