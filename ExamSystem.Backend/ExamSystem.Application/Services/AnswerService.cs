using AutoMapper;
using ExamSystem.Application.DTOs;
using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Application.Common.Models;
using ExamSystem.Application.Common.Enums;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Interfaces.Repositories;

namespace ExamSystem.Application.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IMapper _mapper;
        public AnswerService(IMapper mapper, IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
            _mapper = mapper;
        }

        public async Task<ServiceOperationResult> CreateOpenAnswerAsync(CreateAnswerDto createAnswerDto)
        {
            if (createAnswerDto is not CreateOpenAnswerDto openAnswerDto)
                return ServiceOperationResult.Fail("Invalid answer type. Expected OpenAnswerDto.", ServiceOperationErrorType.Internal);
            var answer = _mapper.Map<Answer>(openAnswerDto);
            answer.AnswerId = Guid.NewGuid();
            answer.IsGraded = false;

            var result = await _answerRepository.AddAsync(answer);
            return result.Success
                ? ServiceOperationResult.Ok()
                : ServiceOperationResult.Fail("Failed to create answer.", ServiceOperationErrorType.Internal);
        }

        public async Task<ServiceOperationResult> CreateOptionAnswerAsync(CreateAnswerDto createAnswerDto)
        {
            if (createAnswerDto is not CreateOptionAnswerDto optionAnswerDto)
                return ServiceOperationResult.Fail("Invalid answer type. Expected OptionAnswerDto.", ServiceOperationErrorType.Internal);

            var answer = _mapper.Map<Answer>(optionAnswerDto);
            answer.AnswerId = Guid.NewGuid();
            answer.IsGraded = false;
            var result = await _answerRepository.AddAsync(answer);
            return result.Success
                ? ServiceOperationResult.Ok()
                : ServiceOperationResult.Fail("Failed to create answer.", ServiceOperationErrorType.Internal);
        }


        public async Task<ServiceOperationResult> DeleteAsync(Guid id)
        {
            var result = await _answerRepository.DeleteAsync(id);
            return result.Success
                ? ServiceOperationResult.Ok()
                : ServiceOperationResult.Fail("Failed to delete answer.", ServiceOperationErrorType.Internal);
        }

        public async Task<ServiceOperationResult<IEnumerable<AnswerDto>>> GetAllAsync()
        {
            var result = await _answerRepository.GetAllAsync();
            if (!result.Success)
                return ServiceOperationResult<IEnumerable<AnswerDto>>.Fail(result.ErrorMessage!, ServiceOperationErrorType.Internal);

            var examDtos = _mapper.Map<IEnumerable<AnswerDto>>(result.Data);
            return ServiceOperationResult<IEnumerable<AnswerDto>>.Ok(examDtos);
        }

        public async Task<ServiceOperationResult<IEnumerable<AnswerDto>>> GetAllByExamIdAsync(Guid examId)
        {
            var result = await _answerRepository.GetAllAsync();
            if (!result.Success)
                return ServiceOperationResult<IEnumerable<AnswerDto>>.Fail(result.ErrorMessage!, ServiceOperationErrorType.Internal);
            var answers = result.Data!;
            var filteredAnswers = answers.Where(e => e.ExamId == examId).ToList();
            var answerDtos = _mapper.Map<IEnumerable<AnswerDto>>(filteredAnswers);
            return ServiceOperationResult<IEnumerable<AnswerDto>>.Ok(answerDtos);
        }
        public async Task<ServiceOperationResult<IEnumerable<AnswerDto>>> GetAllByUserIdAsync(Guid userId)
        {
            var result = await _answerRepository.GetAllAsync();
            if (!result.Success)
                return ServiceOperationResult<IEnumerable<AnswerDto>>.Fail(result.ErrorMessage!, ServiceOperationErrorType.Internal);
            var answers = result.Data!;
            var filteredAnswers = answers.Where(e => e.UserId == userId).ToList();
            var answerDtos = _mapper.Map<IEnumerable<AnswerDto>>(filteredAnswers);
            return ServiceOperationResult<IEnumerable<AnswerDto>>.Ok(answerDtos);
        }

        public async Task<ServiceOperationResult<AnswerDto>> GetByIdAsync(Guid id)
        {
            var result = await _answerRepository.GetByIdAsync(id);
            if (!result.Success)
                return ServiceOperationResult<AnswerDto>.Fail(result.ErrorMessage!, ServiceOperationErrorType.NotFound);

            var answerDto = _mapper.Map<AnswerDto>(result.Data);
            return ServiceOperationResult<AnswerDto>.Ok(answerDto);
        }
        public async Task<ServiceOperationResult<IEnumerable<AnswerDto>>> GetAllByExamUserIdAsync(Guid examUserId)
        {
            var result = await _answerRepository.GetAllByExamUserIdAsync(examUserId);
            if (!result.Success)
                return ServiceOperationResult<IEnumerable<AnswerDto>>.Fail(result.ErrorMessage!, ServiceOperationErrorType.NotFound);
            var answerDtos = _mapper.Map<IEnumerable<AnswerDto>>(result.Data);
            return ServiceOperationResult<IEnumerable<AnswerDto>>.Ok(answerDtos);
        }
    }
}
