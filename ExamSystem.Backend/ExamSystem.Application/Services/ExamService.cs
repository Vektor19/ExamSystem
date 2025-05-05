using AutoMapper;
using ExamSystem.Application.DTOs;
using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Enums;
using ExamSystem.Core.Interfaces.Repositories;

namespace ExamSystem.Application.Services
{
    public class ExamService : IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public ExamService(IExamRepository examRepository, IMapper mapper, IUserRepository userRepository)
        {
            _examRepository = examRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<ExamDto>> CreateAsync(ExamCreateDto examCreateDto)
        {
            var existingUserResult = await _userRepository.GetByIdAsync(examCreateDto.CreatedByUserId);
            if (!existingUserResult.Success || existingUserResult.Data == null)
                return OperationResult<ExamDto>.Fail("User who creates exam not found.");
            if (string.IsNullOrWhiteSpace(examCreateDto.Name))
                return OperationResult<ExamDto>.Fail("Exam name is required.");

            var exam = _mapper.Map<Exam>(examCreateDto);
            exam.ExamId = Guid.NewGuid();
            exam.Status = ExamStatus.NotStarted;
            exam.UserCreatedBy = existingUserResult.Data;
            exam.CreatedDate = DateTime.UtcNow;
            exam.JoinCode = Guid.NewGuid().ToString();

            var result = await _examRepository.AddAsync(exam);
            if (!result.Success)
                return OperationResult<ExamDto>.Fail(result.ErrorMessage!);
            var examResult = await _examRepository.GetByIdAsync(exam.ExamId);
            if (!examResult.Success || examResult.Data == null)
                return OperationResult<ExamDto>.Fail("Created exam not found");
            var examDto = _mapper.Map<ExamDto>(examResult.Data);
            return OperationResult<ExamDto>.Ok(examDto);
        }


        public async Task<OperationResult> DeleteAsync(Guid id)
        {
            var result = await _examRepository.DeleteAsync(id);
            return result.Success
                ? OperationResult.Ok()
                : OperationResult.Fail("Failed to delete exam.");
        }

        public async Task<OperationResult<IEnumerable<ExamDto>>> GetAllAsync()
        {
            var result = await _examRepository.GetAllAsync();
            if (!result.Success)
                return OperationResult<IEnumerable<ExamDto>>.Fail(result.ErrorMessage!);
            if (!result.Data!.Any())
                return OperationResult<IEnumerable<ExamDto>>.Fail("No exams found.");

            var examDtos = _mapper.Map<IEnumerable<ExamDto>>(result.Data);
            return OperationResult<IEnumerable<ExamDto>>.Ok(examDtos);
        }

        public async Task<OperationResult<IEnumerable<ExamDto>>> GetAllByCreatedUserIdAsync(Guid createdByUserId)
        {
            var result = await _examRepository.GetAllAsync();
            if (!result.Success)
                return OperationResult<IEnumerable<ExamDto>>.Fail(result.ErrorMessage!);
            var exams = result.Data!;
            var filteredExams = exams.Where(e => e.CreatedByUserId == createdByUserId).ToList();
            if (!filteredExams.Any())
                return OperationResult<IEnumerable<ExamDto>>.Fail("No exams found for this user.");
            var examDtos = _mapper.Map<IEnumerable<ExamDto>>(filteredExams);
            return OperationResult<IEnumerable<ExamDto>>.Ok(examDtos);
        }

        public async Task<OperationResult<IEnumerable<ExamDto>>> GetAllByParticipantUserIdAsync(Guid participantUserId)
        {
            var result = await _examRepository.GetAllAsync();
            if (!result.Success)
                return OperationResult<IEnumerable<ExamDto>>.Fail(result.ErrorMessage!);
            var exams = result.Data!;
            var filteredExams = exams.Where(e => e.ExamUsers.Any(eu => eu.UserId == participantUserId));
            if (!filteredExams.Any())
                return OperationResult<IEnumerable<ExamDto>>.Fail("No exams found for this user.");
            var examDtos = _mapper.Map<IEnumerable<ExamDto>>(filteredExams);
            return OperationResult<IEnumerable<ExamDto>>.Ok(examDtos);
        }

        public async Task<OperationResult<ExamDto>> GetByIdAsync(Guid id)
        {
            var result = await _examRepository.GetByIdAsync(id);
            if (!result.Success)
                return OperationResult<ExamDto>.Fail(result.ErrorMessage!);

            var examDto = _mapper.Map<ExamDto>(result.Data);
            return OperationResult<ExamDto>.Ok(examDto);
        }

        public async Task<OperationResult> UpdateAsync(Guid examId, ExamUpdateDto updateDto)
        {
            var existingExamResult = await _examRepository.GetByIdAsync(examId);
            if (!existingExamResult.Success || existingExamResult.Data == null)
                return OperationResult.Fail("Exam not found.");

            var exam = existingExamResult.Data;

            exam.Name = updateDto.Name;
            exam.StartDate = updateDto.StartDate;
            exam.EndDate = updateDto.EndDate;
            exam.Status = Enum.TryParse<ExamStatus>(updateDto.Status, out var status) ? status : ExamStatus.NotStarted;

            var updateResult = await _examRepository.UpdateAsync(exam);
            return updateResult.Success
                ? OperationResult.Ok()
                : OperationResult.Fail("Failed to update exam.");
        }

        public async Task<OperationResult<bool>> IsParticipantAsync(Guid examId, Guid userId)
        {
            var examResult = await _examRepository.GetByIdAsync(examId);
            if (!examResult.Success || examResult.Data == null)
                return OperationResult<bool>.Fail("Exam not found.");
            var exam = examResult.Data;
            var isParticipant = exam.ExamUsers.Any(eu => eu.UserId == userId);
            return OperationResult<bool>.Ok(isParticipant);
        }

        public async Task<OperationResult> AddParticipantAsync(Guid examId, Guid userId)
        {
            var existingExamResult = await _examRepository.GetByIdAsync(examId);
            if (!existingExamResult.Success || existingExamResult.Data == null)
                return OperationResult.Fail("Exam not found.");
            var existingUserResult = await _userRepository.GetByIdAsync(userId);
            if (!existingUserResult.Success || existingUserResult.Data == null)
                return OperationResult.Fail("User not found.");
            var exam = existingExamResult.Data;
            var user = existingUserResult.Data;
            exam.ExamUsers.Add(new ExamUser
            {
                ExamId = examId,
                UserId = userId
               ,
                User = user,
                Exam = exam,
                CompleteStatus = false
            });
            var result = await _examRepository.UpdateAsync(exam);
            return result.Success
                ? OperationResult.Ok()
                : OperationResult.Fail("Failed to add participant.");
        }
    }
}
