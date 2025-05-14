using AutoMapper;
using ExamSystem.Application.DTOs;
using ExamSystem.Application.DTOs.Question;
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
                return ServiceOperationResult.Fail("Exam not found.");
            if (string.IsNullOrWhiteSpace(questionCreateDto.QuestionText))
                return ServiceOperationResult.Fail("Question text is required.");
            if (string.IsNullOrWhiteSpace(questionCreateDto.Type))
                return ServiceOperationResult.Fail("Question type is required.");
            // TODO: Add validation for all models in services using special validators

            var question = _mapper.Map<Question>(questionCreateDto);
            question.QuestionId = Guid.NewGuid();
            // TODO: add switch case for question type

            question.Exam = examResult.Data;

            var result = await _questionRepository.AddAsync(question);
            return result.Success
                ? ServiceOperationResult.Ok()
                : ServiceOperationResult.Fail("Failed to create question.");
        }


        public async Task<ServiceOperationResult> DeleteAsync(Guid id)
        {
            var result = await _questionRepository.DeleteAsync(id);
            return result.Success
                ? ServiceOperationResult.Ok()
                : ServiceOperationResult.Fail("Failed to delete question.");
        }

        public async Task<ServiceOperationResult<IEnumerable<QuestionDto>>> GetAllAsync()
        {
            var result = await _questionRepository.GetAllAsync();
            if (!result.Success)
                return ServiceOperationResult<IEnumerable<QuestionDto>>.Fail(result.ErrorMessage!);
            if (!result.Data!.Any())
                return ServiceOperationResult<IEnumerable<QuestionDto>>.Fail("No questions found.");

            var questionDtos = _mapper.Map<IEnumerable<QuestionDto>>(result.Data);
            return ServiceOperationResult<IEnumerable<QuestionDto>>.Ok(questionDtos);
        }

        public async Task<ServiceOperationResult<IEnumerable<QuestionDto>>> GetAllByExamIdAsync(Guid examId)
        {
            var result = await _questionRepository.GetAllAsync();
            if (!result.Success)
                return ServiceOperationResult<IEnumerable<QuestionDto>>.Fail(result.ErrorMessage!);
            var questions = result.Data!;
            var filteredQuestions = questions.Where(q => q.ExamId == examId).ToList();
            if (!filteredQuestions.Any())
                return ServiceOperationResult<IEnumerable<QuestionDto>>.Fail("No questions found for this exam.");
            var questionDtos = _mapper.Map<IEnumerable<QuestionDto>>(filteredQuestions);
            return ServiceOperationResult<IEnumerable<QuestionDto>>.Ok(questionDtos);
        }

        public async Task<ServiceOperationResult<QuestionDto>> GetByIdAsync(Guid id)
        {
            var result = await _questionRepository.GetByIdAsync(id);
            if (!result.Success)
                return ServiceOperationResult<QuestionDto>.Fail(result.ErrorMessage!);

            var question = _mapper.Map<QuestionDto>(result.Data);
            return ServiceOperationResult<QuestionDto>.Ok(question);
        }

        public async Task<ServiceOperationResult> UpdateAsync(Guid questionId, QuestionUpdateDto updateDto)
        {
            var existingQuestionResult = await _questionRepository.GetByIdAsync(questionId);
            if (!existingQuestionResult.Success || existingQuestionResult.Data == null)
                return ServiceOperationResult.Fail("Question not found.");

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
                : ServiceOperationResult.Fail("Failed to update question.");
        }
        public async Task<ServiceOperationResult<IEnumerable<QuestionDto>>> GetAllUnansweredByUserAsync(Guid userId, Guid examId)
        {
            var result = await _questionRepository.GetUnansweredByUserAsync(examId, userId);
            if (!result.Success)
                return ServiceOperationResult<IEnumerable<QuestionDto>>.Fail(result.ErrorMessage!);
            var questions = result.Data!;
            if (!questions.Any())
                return ServiceOperationResult<IEnumerable<QuestionDto>>.Fail("No questions found for this exam and user.");
            var questionDtos = _mapper.Map<IEnumerable<QuestionDto>>(questions);
            return ServiceOperationResult<IEnumerable<QuestionDto>>.Ok(questionDtos);
        }
        public async Task<ServiceOperationResult> GradeTextQuestionAnswerAsync(Guid questionId, GradeOpenAnswerDto gradeOpenAnswerDto)
        {
            var answerResult = await _answerRepository.GetByIdAsync(gradeOpenAnswerDto.AnswerId);
            if (!answerResult.Success || answerResult.Data == null)
                return ServiceOperationResult.Fail("Answer not found.");
            if (answerResult.Data.IsGraded)
                return ServiceOperationResult.Fail("Answer is already graded.");
            var questionResult = await _questionRepository.GetByIdAsync(questionId);
            if (!questionResult.Success || questionResult.Data == null)
                return ServiceOperationResult.Fail("Question not found.");
            var question = questionResult.Data;
            if (question.Type != QuestionType.Text)
                return ServiceOperationResult.Fail("Question is not of type Text.");
            if (question.MaxPoints < gradeOpenAnswerDto.Grade)
                return ServiceOperationResult.Fail("Grade is higher than maximum");
            var examUserResult = await _examRepository.GetExamUserByIdAsync(gradeOpenAnswerDto.ExamUserId);
            if (!examUserResult.Success || examUserResult.Data == null)
                return ServiceOperationResult.Fail("Exam user not found.");

            var examResult = await _examRepository.GetByIdAsync(question.ExamId);

            if (!examResult.Success || examResult.Data == null)
                return ServiceOperationResult.Fail("Exam not found.");

            var exam = examResult.Data;

            var examUser = exam.ExamUsers.FirstOrDefault(eu => eu.UserId == examUserResult.Data.UserId)!;

            examUser.Grade += (int)gradeOpenAnswerDto.Grade;
            answerResult.Data.IsGraded = true;
            var updateResult = await _answerRepository.UpdateAsync(answerResult.Data);
            if (!updateResult.Success)
                return ServiceOperationResult.Fail("Failed to update answer.");
            var answersResult = await _answerRepository.GetAllByExamUserIdAsync(examUserResult.Data.ExamUserId);
            if (!answersResult.Success)
                return ServiceOperationResult.Fail("Failed to get answers.");
            var answers = answersResult.Data!;
            if (!answers.Any(a => !a.IsGraded))
            {
                examUser.IsChecked = true;
            }
            var result = await _examRepository.UpdateAsync(exam);
            if (!result.Success)
                return ServiceOperationResult.Fail("Failed to update exam user.");
            return ServiceOperationResult.Ok();
        }
    }
}
