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
        public async Task<OperationResult<IEnumerable<Question>>> GetAllAsync()
        {
            var questions = await _dbContext.Questions.Include(q => q.Exam)
                                                      .Include(q => q.QuestionOptions)
                                                      .ToListAsync();
            return OperationResult<IEnumerable<Question>>.Ok(questions);
        }
        public async Task<OperationResult<Question>> GetByIdAsync(Guid id)
        {
            var question = await _dbContext.Questions.Include(q => q.Exam)
                                                     .Include(q => q.QuestionOptions)
                                                     .FirstOrDefaultAsync(q => q.QuestionId == id);
            if (question == null)
                return OperationResult<Question>.Fail("Question not found.");
            return OperationResult<Question>.Ok(question);
        }

        public async Task<OperationResult> AddAsync(Question entity)
        {
            await _dbContext.Questions.AddAsync(entity);
            var result = await _dbContext.SaveChangesAsync();

            if (result == 0)
                return OperationResult.Fail("Failed to add question.");

            return OperationResult.Ok();
        }
        public async Task<OperationResult> UpdateAsync(Question entity)
        {
            _dbContext.Questions.Update(entity);
            var result = await _dbContext.SaveChangesAsync();
            if (result == 0)
                return OperationResult.Fail("Failed to update question.");
            return OperationResult.Ok();
        }

        public async Task<OperationResult> DeleteAsync(Guid id)
        {
            var question = await _dbContext.Questions.FindAsync(id);
            if (question == null)
                return OperationResult.Fail("Question not found.");
            _dbContext.Questions.Remove(question);
            var result = await _dbContext.SaveChangesAsync();
            if (result == 0)
                return OperationResult.Fail("Failed to delete question.");
            return OperationResult.Ok();
        }
        public async Task<OperationResult<IEnumerable<Question>>> GetUnansweredByUserAsync(Guid examId, Guid userId)
        {
            var questions = await _dbContext.Questions
                .Include(q => q.QuestionOptions)
                .Where(q => q.ExamId == examId)
                .Where(q => !_dbContext.Answers
                    .Any(a => a.QuestionId == q.QuestionId && a.UserId == userId))
                .ToListAsync();

            if (!questions.Any())
                return OperationResult<IEnumerable<Question>>.Fail("User has answered all questions for this exam.");

            return OperationResult<IEnumerable<Question>>.Ok(questions);
        }
    }
}
