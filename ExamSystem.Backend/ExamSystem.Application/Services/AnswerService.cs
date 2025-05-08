using AutoMapper;
using ExamSystem.Application.DTOs;
using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Enums;
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

        public async Task<OperationResult> CreateOpenAnswerAsync(CreateAnswerDto createAnswerDto)
        {
            if (createAnswerDto is not CreateOpenAnswerDto openAnswerDto)
                return OperationResult.Fail("Invalid answer type. Expected OpenAnswerDto.");
            var answer = _mapper.Map<Answer>(openAnswerDto);
            answer.AnswerId = Guid.NewGuid();

            var result = await _answerRepository.AddAsync(answer);
            return result.Success
                ? OperationResult.Ok()
                : OperationResult.Fail("Failed to create answer.");
        }

        public async Task<OperationResult> CreateOptionAnswerAsync(CreateAnswerDto createAnswerDto)
        {
            if (createAnswerDto is not CreateOptionAnswerDto optionAnswerDto)
                return OperationResult.Fail("Invalid answer type. Expected OptionAnswerDto.");

            var answer = _mapper.Map<Answer>(optionAnswerDto);
            answer.AnswerId = Guid.NewGuid();
            var result = await _answerRepository.AddAsync(answer);
            return result.Success
                ? OperationResult.Ok()
                : OperationResult.Fail("Failed to create answer.");
        }


        public async Task<OperationResult> DeleteAsync(Guid id)
        {
            var result = await _answerRepository.DeleteAsync(id);
            return result.Success
                ? OperationResult.Ok()
                : OperationResult.Fail("Failed to delete answer.");
        }

        public async Task<OperationResult<IEnumerable<AnswerDto>>> GetAllAsync()
        {
            var result = await _answerRepository.GetAllAsync();
            if (!result.Success)
                return OperationResult<IEnumerable<AnswerDto>>.Fail(result.ErrorMessage!);
            if (!result.Data!.Any())
                return OperationResult<IEnumerable<AnswerDto>>.Fail("No answers found.");

            var examDtos = _mapper.Map<IEnumerable<AnswerDto>>(result.Data);
            return OperationResult<IEnumerable<AnswerDto>>.Ok(examDtos);
        }

        public async Task<OperationResult<IEnumerable<AnswerDto>>> GetAllByExamIdAsync(Guid examId)
        {
            var result = await _answerRepository.GetAllAsync();
            if (!result.Success)
                return OperationResult<IEnumerable<AnswerDto>>.Fail(result.ErrorMessage!);
            var answers = result.Data!;
            var filteredAnswers = answers.Where(e => e.ExamId == examId).ToList();
            if (!filteredAnswers.Any())
                return OperationResult<IEnumerable<AnswerDto>>.Fail("No answers found for this exam.");
            var answerDtos = _mapper.Map<IEnumerable<AnswerDto>>(filteredAnswers);
            return OperationResult<IEnumerable<AnswerDto>>.Ok(answerDtos);
        }
        public async Task<OperationResult<IEnumerable<AnswerDto>>> GetAllByUserIdAsync(Guid userId)
        {
            var result = await _answerRepository.GetAllAsync();
            if (!result.Success)
                return OperationResult<IEnumerable<AnswerDto>>.Fail(result.ErrorMessage!);
            var answers = result.Data!;
            var filteredAnswers = answers.Where(e => e.UserId == userId).ToList();
            if (!filteredAnswers.Any())
                return OperationResult<IEnumerable<AnswerDto>>.Fail("No answers found for this user.");
            var answerDtos = _mapper.Map<IEnumerable<AnswerDto>>(filteredAnswers);
            return OperationResult<IEnumerable<AnswerDto>>.Ok(answerDtos);
        }

        public async Task<OperationResult<AnswerDto>> GetByIdAsync(Guid id)
        {
            var result = await _answerRepository.GetByIdAsync(id);
            if (!result.Success)
                return OperationResult<AnswerDto>.Fail(result.ErrorMessage!);

            var answerDto = _mapper.Map<AnswerDto>(result.Data);
            return OperationResult<AnswerDto>.Ok(answerDto);
        }
    }
}
