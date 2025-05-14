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
        public async Task<RepositoryOperationResult<IEnumerable<Answer>>> GetAllAsync()
        {
            var answers = await _dbContext.Answers.Include(a => a.QuestionOption)
                                                  .Include(a => a.User)
                                                  .Include(a => a.Exam)
                                                  .ToListAsync();
            return RepositoryOperationResult<IEnumerable<Answer>>.Ok(answers);
        }
        public async Task<RepositoryOperationResult<Answer>> GetByIdAsync(Guid id)
        {
            var answer = await _dbContext.Answers.Include(a => a.QuestionOption)
                                                 .Include(a => a.User)
                                                 .Include(a => a.Exam)
                                                 .FirstOrDefaultAsync(a => a.AnswerId == id);
            if (answer == null)
                return RepositoryOperationResult<Answer>.Fail("Answer not found.");
            return RepositoryOperationResult<Answer>.Ok(answer);
        }

        public async Task<RepositoryOperationResult> AddAsync(Answer entity)
        {
            await _dbContext.Answers.AddAsync(entity);
            var result = await _dbContext.SaveChangesAsync();

            if (result == 0)
                return RepositoryOperationResult.Fail("Failed to add answer.");

            return RepositoryOperationResult.Ok();
        }
        public async Task<RepositoryOperationResult> UpdateAsync(Answer entity)
        {
            _dbContext.Answers.Update(entity);
            var result = await _dbContext.SaveChangesAsync();
            if (result == 0)
                return RepositoryOperationResult.Fail("Failed to update answer.");
            return RepositoryOperationResult.Ok();
        }

        public async Task<RepositoryOperationResult> DeleteAsync(Guid id)
        {
            var answer = await _dbContext.Answers.FindAsync(id);
            if (answer == null)
                return RepositoryOperationResult.Fail("Answer not found.");
            _dbContext.Answers.Remove(answer);
            var result = await _dbContext.SaveChangesAsync();
            if (result == 0)
                return RepositoryOperationResult.Fail("Failed to delete answer.");
            return RepositoryOperationResult.Ok();
        }
        public async Task<RepositoryOperationResult<IEnumerable<Answer>>> GetAllByExamUserIdAsync(Guid examUserId)
        {
            var examUser = await _dbContext.ExamUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(eu => eu.ExamUserId == examUserId);

            if (examUser == null)
                return RepositoryOperationResult<IEnumerable<Answer>>.Fail("ExamUser not found.");

            var answers = await _dbContext.Answers
                .Include(a => a.QuestionOption)
                .Include(a => a.User)
                .Include(a => a.Exam)
                .Where(a => a.UserId == examUser.UserId && a.ExamId == examUser.ExamId)
                .ToListAsync();

            return RepositoryOperationResult<IEnumerable<Answer>>.Ok(answers);
        }

    }
}
