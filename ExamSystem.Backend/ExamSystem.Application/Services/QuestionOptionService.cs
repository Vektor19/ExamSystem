using AutoMapper;
using ExamSystem.Application.DTOs.QuestionOption;
using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Enums;
using ExamSystem.Core.Interfaces.Repositories;

namespace ExamSystem.Application.Services
{
    public class QuestionOptionService : IQuestionOptionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuestionOptionRepository _questionOptionRepository;
        private readonly IMapper _mapper;
        public QuestionOptionService(IMapper mapper, IQuestionOptionRepository questionOptionRepository, IQuestionRepository questionRepository)
        {
            _questionOptionRepository = questionOptionRepository;
            _mapper = mapper;
            _questionRepository = questionRepository;
        }

        public async Task<RepositoryOperationResult> CreateAsync(QuestionOptionCreateDto questionCreateDto)
        {
            var questionOption = _mapper.Map<QuestionOption>(questionCreateDto);
            questionOption.QuestionOptionId = Guid.NewGuid();

            var questionResult = await _questionRepository.GetByIdAsync(questionCreateDto.QuestionId);
            if (!questionResult.Success || questionResult.Data == null)
                return RepositoryOperationResult.Fail("Question not found.");

            questionOption.Question = questionResult.Data;
            var result = await _questionOptionRepository.AddAsync(questionOption);
            return result.Success
                ? RepositoryOperationResult.Ok()
                : RepositoryOperationResult.Fail("Failed to create question option.");
        }


        public async Task<RepositoryOperationResult> DeleteAsync(Guid id)
        {
            var result = await _questionOptionRepository.DeleteAsync(id);
            return result.Success
                ? RepositoryOperationResult.Ok()
                : RepositoryOperationResult.Fail("Failed to delete question option.");
        }

        public async Task<RepositoryOperationResult<IEnumerable<QuestionOptionDto>>> GetAllAsync()
        {
            var result = await _questionOptionRepository.GetAllAsync();
            if (!result.Success)
                return RepositoryOperationResult<IEnumerable<QuestionOptionDto>>.Fail(result.ErrorMessage!);
            if (!result.Data!.Any())
                return RepositoryOperationResult<IEnumerable<QuestionOptionDto>>.Fail("No question options found.");

            var questionOptionsDtos = _mapper.Map<IEnumerable<QuestionOptionDto>>(result.Data);
            return RepositoryOperationResult<IEnumerable<QuestionOptionDto>>.Ok(questionOptionsDtos);
        }

        public async Task<RepositoryOperationResult<IEnumerable<QuestionOptionDto>>> GetAllByQuestionIdAsync(Guid questionId)
        {
            var result = await _questionOptionRepository.GetAllAsync();
            if (!result.Success)
                return RepositoryOperationResult<IEnumerable<QuestionOptionDto>>.Fail(result.ErrorMessage!);
            var questionOptions = result.Data!;
            var filteredQuestionOptions = questionOptions.Where(q => q.QuestionId == questionId).ToList();
            if (!filteredQuestionOptions.Any())
                return RepositoryOperationResult<IEnumerable<QuestionOptionDto>>.Fail("No question options found for this question.");
            var questionOptionsDtos = _mapper.Map<IEnumerable<QuestionOptionDto>>(filteredQuestionOptions);
            return RepositoryOperationResult<IEnumerable<QuestionOptionDto>>.Ok(questionOptionsDtos);
        }

        public async Task<RepositoryOperationResult<QuestionOptionDto>> GetByIdAsync(Guid id)
        {
            var result = await _questionOptionRepository.GetByIdAsync(id);
            if (!result.Success)
                return RepositoryOperationResult<QuestionOptionDto>.Fail(result.ErrorMessage!);

            var questionOption = _mapper.Map<QuestionOptionDto>(result.Data);
            return RepositoryOperationResult<QuestionOptionDto>.Ok(questionOption);
        }

        public async Task<RepositoryOperationResult> UpdateAsync(Guid questionOptionId, QuestionOptionUpdateDto updateDto)
        {
            var existingQuestionOptionResult = await _questionOptionRepository.GetByIdAsync(questionOptionId);
            if (!existingQuestionOptionResult.Success || existingQuestionOptionResult.Data == null)
                return RepositoryOperationResult.Fail("Question option not found.");

            var questionOption = existingQuestionOptionResult.Data;

            questionOption.OptionText = updateDto.OptionText;
            questionOption.Label = updateDto.Label;
            questionOption.IsCorrect = updateDto.IsCorrect;

            var updateResult = await _questionOptionRepository.UpdateAsync(questionOption);
            return updateResult.Success
                ? RepositoryOperationResult.Ok()
                : RepositoryOperationResult.Fail("Failed to update question option.");
        }
    }
}
