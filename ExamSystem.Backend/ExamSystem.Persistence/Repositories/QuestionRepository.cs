using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Persistence.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly ExamSystemDbContext _dbContext;
        public QuestionRepository(ExamSystemDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<RepositoryOperationResult<IEnumerable<Question>>> GetAllAsync()
        {
            var questions = await _dbContext.Questions.Include(q => q.Exam)
                                                      .Include(q => q.QuestionOptions)
                                                      .ToListAsync();
            return RepositoryOperationResult<IEnumerable<Question>>.Ok(questions);
        }
        public async Task<RepositoryOperationResult<Question>> GetByIdAsync(Guid id)
        {
            var question = await _dbContext.Questions.Include(q => q.Exam)
                                                     .Include(q => q.QuestionOptions)
                                                     .FirstOrDefaultAsync(q => q.QuestionId == id);
            if (question == null)
                return RepositoryOperationResult<Question>.Fail("Question not found.");
            return RepositoryOperationResult<Question>.Ok(question);
        }

        public async Task<RepositoryOperationResult> AddAsync(Question entity)
        {
            await _dbContext.Questions.AddAsync(entity);
            var result = await _dbContext.SaveChangesAsync();

            if (result == 0)
                return RepositoryOperationResult.Fail("Failed to add question.");

            return RepositoryOperationResult.Ok();
        }
        public async Task<RepositoryOperationResult> UpdateAsync(Question entity)
        {
            _dbContext.Questions.Update(entity);
            var result = await _dbContext.SaveChangesAsync();
            if (result == 0)
                return RepositoryOperationResult.Fail("Failed to update question.");
            return RepositoryOperationResult.Ok();
        }

        public async Task<RepositoryOperationResult> DeleteAsync(Guid id)
        {
            var question = await _dbContext.Questions.FindAsync(id);
            if (question == null)
                return RepositoryOperationResult.Fail("Question not found.");
            _dbContext.Questions.Remove(question);
            var result = await _dbContext.SaveChangesAsync();
            if (result == 0)
                return RepositoryOperationResult.Fail("Failed to delete question.");
            return RepositoryOperationResult.Ok();
        }
        public async Task<RepositoryOperationResult<IEnumerable<Question>>> GetUnansweredByUserAsync(Guid examId, Guid userId)
        {
            var questions = await _dbContext.Questions
                .Include(q => q.QuestionOptions)
                .Where(q => q.ExamId == examId)
                .Where(q => !_dbContext.Answers
                    .Any(a => a.QuestionId == q.QuestionId && a.UserId == userId))
                .ToListAsync();

            return RepositoryOperationResult<IEnumerable<Question>>.Ok(questions);
        }
    }
}
