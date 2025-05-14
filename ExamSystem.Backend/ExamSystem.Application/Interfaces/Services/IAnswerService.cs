using System;
using System.Collections.Generic;
using ExamSystem.Application.DTOs;
using ExamSystem.Core.Common;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IAnswerService
    {
        Task<RepositoryOperationResult<AnswerDto>> GetByIdAsync(Guid answerId);
        Task<RepositoryOperationResult<IEnumerable<AnswerDto>>> GetAllAsync();
        Task<RepositoryOperationResult<IEnumerable<AnswerDto>>> GetAllByExamIdAsync(Guid examId);
        Task<RepositoryOperationResult<IEnumerable<AnswerDto>>> GetAllByExamUserIdAsync(Guid examUserId);
        Task<RepositoryOperationResult<IEnumerable<AnswerDto>>> GetAllByUserIdAsync(Guid userId);
        Task<RepositoryOperationResult> DeleteAsync(Guid answerId);
        Task<RepositoryOperationResult> CreateOpenAnswerAsync(CreateAnswerDto createAnswerDto);
        Task<RepositoryOperationResult> CreateOptionAnswerAsync(CreateAnswerDto createAnswerDto);

    }
}
