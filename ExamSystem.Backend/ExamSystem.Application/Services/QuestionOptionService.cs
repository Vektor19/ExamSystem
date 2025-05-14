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

        public async Task<ServiceOperationResult> CreateAsync(QuestionOptionCreateDto questionCreateDto)
        {
            var questionOption = _mapper.Map<QuestionOption>(questionCreateDto);
            questionOption.QuestionOptionId = Guid.NewGuid();

            var questionResult = await _questionRepository.GetByIdAsync(questionCreateDto.QuestionId);
            if (!questionResult.Success || questionResult.Data == null)
                return ServiceOperationResult.Fail("Question not found.");

            questionOption.Question = questionResult.Data;
            var result = await _questionOptionRepository.AddAsync(questionOption);
            return result.Success
                ? ServiceOperationResult.Ok()
                : ServiceOperationResult.Fail("Failed to create question option.");
        }


        public async Task<ServiceOperationResult> DeleteAsync(Guid id)
        {
            var result = await _questionOptionRepository.DeleteAsync(id);
            return result.Success
                ? ServiceOperationResult.Ok()
                : ServiceOperationResult.Fail("Failed to delete question option.");
        }

        public async Task<ServiceOperationResult<IEnumerable<QuestionOptionDto>>> GetAllAsync()
        {
            var result = await _questionOptionRepository.GetAllAsync();
            if (!result.Success)
                return ServiceOperationResult<IEnumerable<QuestionOptionDto>>.Fail(result.ErrorMessage!);
            if (!result.Data!.Any())
                return ServiceOperationResult<IEnumerable<QuestionOptionDto>>.Fail("No question options found.");

            var questionOptionsDtos = _mapper.Map<IEnumerable<QuestionOptionDto>>(result.Data);
            return ServiceOperationResult<IEnumerable<QuestionOptionDto>>.Ok(questionOptionsDtos);
        }

        public async Task<ServiceOperationResult<IEnumerable<QuestionOptionDto>>> GetAllByQuestionIdAsync(Guid questionId)
        {
            var result = await _questionOptionRepository.GetAllAsync();
            if (!result.Success)
                return ServiceOperationResult<IEnumerable<QuestionOptionDto>>.Fail(result.ErrorMessage!);
            var questionOptions = result.Data!;
            var filteredQuestionOptions = questionOptions.Where(q => q.QuestionId == questionId).ToList();
            if (!filteredQuestionOptions.Any())
                return ServiceOperationResult<IEnumerable<QuestionOptionDto>>.Fail("No question options found for this question.");
            var questionOptionsDtos = _mapper.Map<IEnumerable<QuestionOptionDto>>(filteredQuestionOptions);
            return ServiceOperationResult<IEnumerable<QuestionOptionDto>>.Ok(questionOptionsDtos);
        }

        public async Task<ServiceOperationResult<QuestionOptionDto>> GetByIdAsync(Guid id)
        {
            var result = await _questionOptionRepository.GetByIdAsync(id);
            if (!result.Success)
                return ServiceOperationResult<QuestionOptionDto>.Fail(result.ErrorMessage!);

            var questionOption = _mapper.Map<QuestionOptionDto>(result.Data);
            return ServiceOperationResult<QuestionOptionDto>.Ok(questionOption);
        }

        public async Task<ServiceOperationResult> UpdateAsync(Guid questionOptionId, QuestionOptionUpdateDto updateDto)
        {
            var existingQuestionOptionResult = await _questionOptionRepository.GetByIdAsync(questionOptionId);
            if (!existingQuestionOptionResult.Success || existingQuestionOptionResult.Data == null)
                return ServiceOperationResult.Fail("Question option not found.");

            var questionOption = existingQuestionOptionResult.Data;

            questionOption.OptionText = updateDto.OptionText;
            questionOption.Label = updateDto.Label;
            questionOption.IsCorrect = updateDto.IsCorrect;

            var updateResult = await _questionOptionRepository.UpdateAsync(questionOption);
            return updateResult.Success
                ? ServiceOperationResult.Ok()
                : ServiceOperationResult.Fail("Failed to update question option.");
        }
    }
}
