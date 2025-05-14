using AutoMapper;
using ExamSystem.Application.DTOs;
using ExamSystem.Application.DTOs.Question;
using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Application.Common.Models;
using ExamSystem.Application.Common.Enums;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Enums;
using ExamSystem.Core.Interfaces.Repositories;
using ExamSystem.Application.Utils.Validators;

namespace ExamSystem.Application.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IExamRepository _examRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IMapper _mapper;
        public QuestionService(IExamRepository examRepository, IMapper mapper, IQuestionRepository questionRepository, IAnswerRepository answerRepository)
        {
            _examRepository = examRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _mapper = mapper;
        }

        public async Task<ServiceOperationResult> CreateAsync(QuestionCreateDto questionCreateDto)
        {
            var examResult = await _examRepository.GetByIdAsync(questionCreateDto.ExamId);
            if (!examResult.Success || examResult.Data == null)
                return ServiceOperationResult.Fail("Exam not found.", ServiceOperationErrorType.BadRequest);

            if(!ExamValidator.IsModifyAllowed(examResult.Data))
                return ServiceOperationResult.Fail("Exam in progress. Cannot add questions.", ServiceOperationErrorType.BadRequest);

            var question = _mapper.Map<Question>(questionCreateDto);
            question.QuestionId = Guid.NewGuid();
            // TODO: add switch case for question type

            question.Exam = examResult.Data;

            var result = await _questionRepository.AddAsync(question);
            return result.Success
                ? ServiceOperationResult.Ok()
                : ServiceOperationResult.Fail("Failed to create question.", ServiceOperationErrorType.Internal);
        }


        public async Task<ServiceOperationResult> DeleteAsync(Guid id)
        {
            var result = await _questionRepository.DeleteAsync(id);
            return result.Success
                ? ServiceOperationResult.Ok()
                : ServiceOperationResult.Fail("Failed to delete question.", ServiceOperationErrorType.Internal);
        }

        public async Task<ServiceOperationResult<IEnumerable<QuestionDto>>> GetAllAsync()
        {
            var result = await _questionRepository.GetAllAsync();
            if (!result.Success)
                return ServiceOperationResult<IEnumerable<QuestionDto>>.Fail(result.ErrorMessage!, ServiceOperationErrorType.Internal);

            var questionDtos = _mapper.Map<IEnumerable<QuestionDto>>(result.Data);
            return ServiceOperationResult<IEnumerable<QuestionDto>>.Ok(questionDtos);
        }

        public async Task<ServiceOperationResult<IEnumerable<QuestionDto>>> GetAllByExamIdAsync(Guid examId)
        {
            var result = await _questionRepository.GetAllAsync();
            if (!result.Success)
                return ServiceOperationResult<IEnumerable<QuestionDto>>.Fail(result.ErrorMessage!, ServiceOperationErrorType.Internal);
            var questions = result.Data!;
            var filteredQuestions = questions.Where(q => q.ExamId == examId).ToList();
            var questionDtos = _mapper.Map<IEnumerable<QuestionDto>>(filteredQuestions);
            return ServiceOperationResult<IEnumerable<QuestionDto>>.Ok(questionDtos);
        }

        public async Task<ServiceOperationResult<QuestionDto>> GetByIdAsync(Guid id)
        {
            var result = await _questionRepository.GetByIdAsync(id);
            if (!result.Success)
                return ServiceOperationResult<QuestionDto>.Fail(result.ErrorMessage!, ServiceOperationErrorType.Internal);

            var question = _mapper.Map<QuestionDto>(result.Data);
            return ServiceOperationResult<QuestionDto>.Ok(question);
        }

        public async Task<ServiceOperationResult> UpdateAsync(Guid questionId, QuestionUpdateDto updateDto)
        {
            var existingQuestionResult = await _questionRepository.GetByIdAsync(questionId);
            if (!existingQuestionResult.Success || existingQuestionResult.Data == null)
                return ServiceOperationResult.Fail("Question not found.", ServiceOperationErrorType.NotFound);

            var question = existingQuestionResult.Data;

            question.QuestionText = updateDto.QuestionText;
            question.Type = Enum.Parse<QuestionType>(updateDto.Type);
            question.ImageUrl = updateDto.ImageUrl;
            question.MaxPoints = updateDto.MaxPoints;

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
                ? ServiceOperationResult.Ok()
                : ServiceOperationResult.Fail("Failed to update question.", ServiceOperationErrorType.Internal);
        }
        public async Task<ServiceOperationResult<IEnumerable<QuestionDto>>> GetAllUnansweredByUserAsync(Guid userId, Guid examId)
        {
            var result = await _questionRepository.GetUnansweredByUserAsync(examId, userId);
            if (!result.Success)
                return ServiceOperationResult<IEnumerable<QuestionDto>>.Fail(result.ErrorMessage!, ServiceOperationErrorType.Internal);
            var questions = result.Data!;
            var questionDtos = _mapper.Map<IEnumerable<QuestionDto>>(questions);
            return ServiceOperationResult<IEnumerable<QuestionDto>>.Ok(questionDtos);
        }
        public async Task<ServiceOperationResult> GradeTextQuestionAnswerAsync(Guid questionId, GradeOpenAnswerDto gradeOpenAnswerDto)
        {
            var questionResult = await _questionRepository.GetByIdAsync(questionId);
            if (!questionResult.Success || questionResult.Data == null)
                return ServiceOperationResult.Fail("Question not found.", ServiceOperationErrorType.NotFound);

            var answerResult = await _answerRepository.GetByIdAsync(gradeOpenAnswerDto.AnswerId);
            if (!answerResult.Success || answerResult.Data == null)
                return ServiceOperationResult.Fail("Answer not found.", ServiceOperationErrorType.BadRequest);
            if (answerResult.Data.IsGraded)
                return ServiceOperationResult.Fail("Answer is already graded.", ServiceOperationErrorType.Conflict);
            
            var question = questionResult.Data;
            if (question.Type != QuestionType.Text)
                return ServiceOperationResult.Fail("Question is not of type Text.", ServiceOperationErrorType.BadRequest);
            if (question.MaxPoints < gradeOpenAnswerDto.Grade)
                return ServiceOperationResult.Fail("Grade is higher than maximum", ServiceOperationErrorType.BadRequest);
            var examUserResult = await _examRepository.GetExamUserByIdAsync(gradeOpenAnswerDto.ExamUserId);
            if (!examUserResult.Success || examUserResult.Data == null)
                return ServiceOperationResult.Fail("Exam user not found.", ServiceOperationErrorType.BadRequest);

            var examResult = await _examRepository.GetByIdAsync(question.ExamId);

            if (!examResult.Success || examResult.Data == null)
                return ServiceOperationResult.Fail("Exam not found.", ServiceOperationErrorType.BadRequest);

            var exam = examResult.Data;

            var examUser = exam.ExamUsers.FirstOrDefault(eu => eu.UserId == examUserResult.Data.UserId)!;

            examUser.Grade += (int)gradeOpenAnswerDto.Grade;
            answerResult.Data.IsGraded = true;
            var updateResult = await _answerRepository.UpdateAsync(answerResult.Data);
            if (!updateResult.Success)
                return ServiceOperationResult.Fail("Failed to update answer.", ServiceOperationErrorType.Internal);
            var answersResult = await _answerRepository.GetAllByExamUserIdAsync(examUserResult.Data.ExamUserId);
            if (!answersResult.Success || !answersResult.Data!.Any())
                return ServiceOperationResult.Fail("Failed to get answers.", ServiceOperationErrorType.BadRequest);
            var answers = answersResult.Data!;
            if (!answers.Any(a => !a.IsGraded))
            {
                examUser.IsChecked = true;
            }
            var result = await _examRepository.UpdateAsync(exam);
            if (!result.Success)
                return ServiceOperationResult.Fail("Failed to update exam user.", ServiceOperationErrorType.Internal);
            return ServiceOperationResult.Ok();
        }
    }
}
