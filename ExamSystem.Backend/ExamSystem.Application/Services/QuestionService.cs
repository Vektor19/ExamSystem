using AutoMapper;
using ExamSystem.Application.DTOs;
using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Enums;
using ExamSystem.Core.Interfaces.Repositories;

namespace ExamSystem.Application.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IExamRepository _examRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;
        public QuestionService(IExamRepository examRepository, IMapper mapper, IQuestionRepository questionRepository)
        {
            _examRepository = examRepository;
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult> CreateAsync(QuestionCreateDto questionCreateDto)
        {
            var examResult = await _examRepository.GetByIdAsync(questionCreateDto.ExamId);
            if (!examResult.Success || examResult.Data == null)
                return OperationResult.Fail("Exam not found.");
            if (string.IsNullOrWhiteSpace(questionCreateDto.QuestionText))
                return OperationResult.Fail("Question text is required.");
            if (string.IsNullOrWhiteSpace(questionCreateDto.Type))
                return OperationResult.Fail("Question type is required.");
            // TODO: Add validation for all models in services using special validators

            var question = _mapper.Map<Question>(questionCreateDto);
            question.QuestionId = Guid.NewGuid();
            // TODO: add switch case for question type

            question.Exam = examResult.Data;

            var result = await _questionRepository.AddAsync(question);
            return result.Success
                ? OperationResult.Ok()
                : OperationResult.Fail("Failed to create question.");
        }


        public async Task<OperationResult> DeleteAsync(Guid id)
        {
            var result = await _questionRepository.DeleteAsync(id);
            return result.Success
                ? OperationResult.Ok()
                : OperationResult.Fail("Failed to delete question.");
        }

        public async Task<OperationResult<IEnumerable<QuestionDto>>> GetAllAsync()
        {
            var result = await _questionRepository.GetAllAsync();
            if (!result.Success)
                return OperationResult<IEnumerable<QuestionDto>>.Fail(result.ErrorMessage!);
            if (!result.Data!.Any())
                return OperationResult<IEnumerable<QuestionDto>>.Fail("No questions found.");

            var questionDtos = _mapper.Map<IEnumerable<QuestionDto>>(result.Data);
            return OperationResult<IEnumerable<QuestionDto>>.Ok(questionDtos);
        }

        public async Task<OperationResult<IEnumerable<QuestionDto>>> GetAllByExamIdAsync(Guid examId)
        {
            var result = await _questionRepository.GetAllAsync();
            if (!result.Success)
                return OperationResult<IEnumerable<QuestionDto>>.Fail(result.ErrorMessage!);
            var questions = result.Data!;
            var filteredQuestions = questions.Where(q => q.ExamId == examId).ToList();
            if (!filteredQuestions.Any())
                return OperationResult<IEnumerable<QuestionDto>>.Fail("No questions found for this exam.");
            var questionDtos = _mapper.Map<IEnumerable<QuestionDto>>(filteredQuestions);
            return OperationResult<IEnumerable<QuestionDto>>.Ok(questionDtos);
        }

        public async Task<OperationResult<QuestionDto>> GetByIdAsync(Guid id)
        {
            var result = await _questionRepository.GetByIdAsync(id);
            if (!result.Success)
                return OperationResult<QuestionDto>.Fail(result.ErrorMessage!);

            var question = _mapper.Map<QuestionDto>(result.Data);
            return OperationResult<QuestionDto>.Ok(question);
        }

        public async Task<OperationResult> UpdateAsync(Guid questionId, QuestionUpdateDto updateDto)
        {
            var existingQuestionResult = await _questionRepository.GetByIdAsync(questionId);
            if (!existingQuestionResult.Success || existingQuestionResult.Data == null)
                return OperationResult.Fail("Question not found.");

            var question = existingQuestionResult.Data;

            question.QuestionText = updateDto.QuestionText;
            question.Type = Enum.Parse<QuestionType>(updateDto.Type);
            question.ImageUrl = updateDto.ImageUrl;

            if (updateDto.Options != null || updateDto!.Options!.Any())
            {
                question.QuestionOptions.Clear();
                foreach (var option in updateDto.Options!)
                {
                    var questionOption = new QuestionOption
                    {
                        QuestionOptionId = Guid.NewGuid(),
                        OptionText = option.OptionText,
                        IsCorrect = option.IsCorrect,
                        Label = option.Label,
                    };
                    question.QuestionOptions.Add(questionOption);
                }
            }

            var updateResult = await _questionRepository.UpdateAsync(question);
            return updateResult.Success
                ? OperationResult.Ok()
                : OperationResult.Fail("Failed to update question.");
        }
    }
}
