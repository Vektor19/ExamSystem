using System;
using System.Collections.Generic;
using ExamSystem.Application.DTOs;
using ExamSystem.Core.Common;

namespace ExamSystem.Application.Interfaces.Services
{
    public interface IAnswerService
    {
        Task<OperationResult<AnswerDto>> GetByIdAsync(Guid answerId);
        Task<OperationResult<IEnumerable<AnswerDto>>> GetAllAsync();
        Task<OperationResult<IEnumerable<AnswerDto>>> GetAllByExamIdAsync(Guid examId);
        Task<OperationResult<IEnumerable<AnswerDto>>> GetAllByUserIdAsync(Guid userId);
        Task<OperationResult> DeleteAsync(Guid answerId);
        Task<OperationResult> CreateAsync(AnswerDto createAnswerDto);
    }
}
