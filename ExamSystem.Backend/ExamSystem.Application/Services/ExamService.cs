using AutoMapper;
using ExamSystem.Application.DTOs;
using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Application.Utils.Validators;
using ExamSystem.Application.Common.Models;
using ExamSystem.Application.Common.Enums;
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

        public async Task<ServiceOperationResult<ExamForExaminatorDto>> CreateAsync(ExamCreateDto examCreateDto)
        {
            var existingUserResult = await _userRepository.GetByIdAsync(examCreateDto.CreatedByUserId);
            if (!existingUserResult.Success || existingUserResult.Data == null)
                return ServiceOperationResult<ExamForExaminatorDto>.Fail("User who creates exam not found.", ServiceOperationErrorType.BadRequest);

            var exam = _mapper.Map<Exam>(examCreateDto);
            exam.ExamId = Guid.NewGuid();
            exam.UserCreatedBy = existingUserResult.Data;
            exam.CreatedDate = DateTime.UtcNow;
            exam.JoinCode = Guid.NewGuid().ToString();

            var result = await _examRepository.AddAsync(exam);
            if (!result.Success)
                return ServiceOperationResult<ExamForExaminatorDto>.Fail(result.ErrorMessage!, ServiceOperationErrorType.Internal);
            var examResult = await _examRepository.GetByIdAsync(exam.ExamId);
            if (!examResult.Success || examResult.Data == null)
                return ServiceOperationResult<ExamForExaminatorDto>.Fail("Created exam not found", ServiceOperationErrorType.Internal);
            var examDto = _mapper.Map<ExamForExaminatorDto>(examResult.Data);
            return ServiceOperationResult<ExamForExaminatorDto>.Ok(examDto);
        }


        public async Task<ServiceOperationResult> DeleteAsync(Guid id)
        {
            var existingExamResult = await _examRepository.GetByIdAsync(id);

            if (!existingExamResult.Success || existingExamResult.Data == null)
                return ServiceOperationResult.Fail("Exam not found.", ServiceOperationErrorType.NotFound);

            if (!ExamValidator.IsModifyAllowed(existingExamResult.Data))
                return ServiceOperationResult.Fail("Exam is in progress. Cannot delete.", ServiceOperationErrorType.Forbidden);

            var result = await _examRepository.DeleteAsync(id);
            return result.Success
                ? ServiceOperationResult.Ok()
                : ServiceOperationResult.Fail("Failed to delete exam.", ServiceOperationErrorType.Internal);
        }

        public async Task<ServiceOperationResult<IEnumerable<ExamForExaminatorDto>>> GetAllAsync()
        {
            var result = await _examRepository.GetAllAsync();
            var examDtos = _mapper.Map<IEnumerable<ExamForExaminatorDto>>(result.Data);
            return ServiceOperationResult<IEnumerable<ExamForExaminatorDto>>.Ok(examDtos);
        }

        public async Task<ServiceOperationResult<IEnumerable<ExamForExaminatorDto>>> GetAllByCreatedUserIdAsync(Guid createdByUserId)
        {
            var result = await _examRepository.GetAllAsync();
            if (!result.Success)
                return ServiceOperationResult<IEnumerable<ExamForExaminatorDto>>.Fail(result.ErrorMessage!, ServiceOperationErrorType.Internal);
            var exams = result.Data!;
            if (!exams.Any())
                return ServiceOperationResult<IEnumerable<ExamForExaminatorDto>>.Ok([]);
            var filteredExams = exams.Where(e => e.CreatedByUserId == createdByUserId).ToList();
            var examDtos = _mapper.Map<IEnumerable<ExamForExaminatorDto>>(filteredExams);
            return ServiceOperationResult<IEnumerable<ExamForExaminatorDto>>.Ok(examDtos);
        }

        public async Task<ServiceOperationResult<IEnumerable<ExamForStudentDto>>> GetAllByParticipantUserIdAsync(Guid participantUserId)
        {
            var result = await _examRepository.GetAllAsync();
            if (!result.Success)
                return ServiceOperationResult<IEnumerable<ExamForStudentDto>>.Fail(result.ErrorMessage!, ServiceOperationErrorType.Internal);

            var exams = result.Data!;

            var filteredExams = exams
                .Where(e => e.ExamUsers.Any(eu => eu.UserId == participantUserId))
                .ToList();

            var examDtos = _mapper.Map<List<ExamForStudentDto>>(filteredExams);

            for (int i = 0; i < examDtos.Count; i++)
            {
                var exam = filteredExams[i];
                var examUser = exam.ExamUsers.FirstOrDefault(eu => eu.UserId == participantUserId);
                examDtos[i].ExamUser = _mapper.Map<ExamUserDto>(examUser);
            }

            return ServiceOperationResult<IEnumerable<ExamForStudentDto>>.Ok(examDtos);
        }

        public async Task<ServiceOperationResult<ExamForExaminatorDto>> GetByIdAsync(Guid id)
        {
            var result = await _examRepository.GetByIdAsync(id);
            if (!result.Success)
                return ServiceOperationResult<ExamForExaminatorDto>.Fail(result.ErrorMessage!, ServiceOperationErrorType.NotFound);

            var examDto = _mapper.Map<ExamForExaminatorDto>(result.Data);
            return ServiceOperationResult<ExamForExaminatorDto>.Ok(examDto);
        }
        public async Task<ServiceOperationResult<ExamUserDto>> GetExamUserByIdAsync(Guid examUserId)
        {
            var result = await _examRepository.GetExamUserByIdAsync(examUserId);
            if (!result.Success)
                return ServiceOperationResult<ExamUserDto>.Fail(result.ErrorMessage!, ServiceOperationErrorType.NotFound);
            var examUserDto = _mapper.Map<ExamUserDto>(result.Data);
            return ServiceOperationResult<ExamUserDto>.Ok(examUserDto);
        }
        public async Task<ServiceOperationResult> UpdateAsync(Guid examId, ExamUpdateDto updateDto)
        {
            var existingExamResult = await _examRepository.GetByIdAsync(examId);
            if (!existingExamResult.Success || existingExamResult.Data == null)
                return ServiceOperationResult.Fail("Exam not found.", ServiceOperationErrorType.NotFound);

            var exam = existingExamResult.Data;


            if (!ExamValidator.IsModifyAllowed(exam))
                return ServiceOperationResult.Fail("Exam is in progress. Cannot update.", ServiceOperationErrorType.Forbidden);

            exam.Name = updateDto.Name;
            exam.StartDate = updateDto.StartDate;
            exam.EndDate = updateDto.EndDate;

            var updateResult = await _examRepository.UpdateAsync(exam);
            return updateResult.Success
                ? ServiceOperationResult.Ok()
                : ServiceOperationResult.Fail("Failed to update exam.", ServiceOperationErrorType.Internal);
        }

        public async Task<ServiceOperationResult<bool>> IsParticipantAsync(Guid examId, Guid userId)
        {
            var examResult = await _examRepository.GetByIdAsync(examId);
            if (!examResult.Success || examResult.Data == null)
                return ServiceOperationResult<bool>.Fail("Exam not found.", ServiceOperationErrorType.NotFound);
            var exam = examResult.Data;
            var isParticipant = exam.ExamUsers.Any(eu => eu.UserId == userId);
            return ServiceOperationResult<bool>.Ok(isParticipant);
        }

        public async Task<ServiceOperationResult> AddParticipantAsync(Guid examId, Guid userId)
        {
            var existingExamResult = await _examRepository.GetByIdAsync(examId);
            if (!existingExamResult.Success || existingExamResult.Data == null)
                return ServiceOperationResult.Fail("Exam not found.", ServiceOperationErrorType.NotFound);
            var existingUserResult = await _userRepository.GetByIdAsync(userId);
            if (!existingUserResult.Success || existingUserResult.Data == null)
                return ServiceOperationResult.Fail("User not found.", ServiceOperationErrorType.NotFound);
            var exam = existingExamResult.Data;


            var user = existingUserResult.Data;
            exam.ExamUsers.Add(new ExamUser
            {
                ExamId = examId,
                UserId = userId
               ,
                User = user,
                Exam = exam,
                CompleteStatus = false,
                IsBlocked = false,
                IsChecked = false,
                Grade = 0
            });
            var result = await _examRepository.UpdateAsync(exam);
            return result.Success
                ? ServiceOperationResult.Ok()
                : ServiceOperationResult.Fail("Failed to add participant.", ServiceOperationErrorType.Internal);
        }

        public async Task<ServiceOperationResult> RemoveParticipantAsync(Guid examId, Guid userId)
        {
            var existingExamResult = await _examRepository.GetByIdAsync(examId);
            if (!existingExamResult.Success || existingExamResult.Data == null)
                return ServiceOperationResult.Fail("Exam not found.", ServiceOperationErrorType.NotFound);
            var existingUserResult = await _userRepository.GetByIdAsync(userId);
            if (!existingUserResult.Success || existingUserResult.Data == null)
                return ServiceOperationResult.Fail("User not found.", ServiceOperationErrorType.NotFound);
            var exam = existingExamResult.Data;

            if (!ExamValidator.IsModifyAllowed(exam))
                return ServiceOperationResult.Fail("Exam is in progress. Cannot remove participant.", ServiceOperationErrorType.Forbidden);

            var user = existingUserResult.Data;
            var examUser = exam.ExamUsers.FirstOrDefault(eu => eu.UserId == userId);
            if (examUser != null)
            {
                exam.ExamUsers.Remove(examUser);
                var result = await _examRepository.UpdateAsync(exam);
                return result.Success
                    ? ServiceOperationResult.Ok()
                    : ServiceOperationResult.Fail("Failed to remove participant.", ServiceOperationErrorType.Internal);
            }
            return ServiceOperationResult.Fail("Participant not found in the exam.", ServiceOperationErrorType.NotFound);
        }

        public async Task<ServiceOperationResult> AddParticipantByEmailAsync(Guid examId, string email)
        {
            var existingExamResult = await _examRepository.GetByIdAsync(examId);
            if (!existingExamResult.Success || existingExamResult.Data == null)
                return ServiceOperationResult.Fail("Exam not found.", ServiceOperationErrorType.NotFound);
            var existingUserResult = await _userRepository.GetByEmailAsync(email);
            if (!existingUserResult.Success || existingUserResult.Data == null)
                return ServiceOperationResult.Fail("User not found.", ServiceOperationErrorType.NotFound);
            var exam = existingExamResult.Data;

            var user = existingUserResult.Data;
            exam.ExamUsers.Add(new ExamUser
            {
                ExamId = examId,
                UserId = user.UserId
               ,
                User = user,
                Exam = exam,
                CompleteStatus = false,
                IsBlocked = false,
                IsChecked = false,
                Grade = 0
            });
            var result = await _examRepository.UpdateAsync(exam);
            return result.Success
                ? ServiceOperationResult.Ok()
                : ServiceOperationResult.Fail("Failed to add participant.", ServiceOperationErrorType.Internal);
        }

        public async Task<ServiceOperationResult> JoinExam(JoinExamDto joinExamDto)
        {
            var examsResult = await _examRepository.GetAllAsync();
            if (!examsResult.Success || examsResult.Data == null)
                return ServiceOperationResult.Fail("No exams found.", ServiceOperationErrorType.NotFound);
            var exam = examsResult.Data.FirstOrDefault(e => e.JoinCode == joinExamDto.JoinCode);
            if (exam == null)
                return ServiceOperationResult.Fail("Wrong join code.", ServiceOperationErrorType.BadRequest);

            if (!ExamValidator.IsModifyAllowed(exam))
                return ServiceOperationResult.Fail("Exam is in progress. Cannot join.", ServiceOperationErrorType.Forbidden);

            var userResult = await _userRepository.GetByIdAsync(joinExamDto.UserId);
            if (!userResult.Success || userResult.Data == null)
                return ServiceOperationResult.Fail("User not found.", ServiceOperationErrorType.NotFound);
            if (exam.ExamUsers.Any(eu => eu.UserId == joinExamDto.UserId))
                return ServiceOperationResult.Fail("User already joined the exam.", ServiceOperationErrorType.BadRequest);
            var user = userResult.Data;
            exam.ExamUsers.Add(new ExamUser
            {
                ExamId = exam.ExamId,
                UserId = user.UserId
               ,
                User = user,
                Exam = exam,
                CompleteStatus = false,
                IsBlocked = false,
                IsChecked = false,
                Grade = 0
            });
            var result = await _examRepository.UpdateAsync(exam);
            if (!result.Success)
                return ServiceOperationResult.Fail("Failed to join exam.", ServiceOperationErrorType.Internal);
            return ServiceOperationResult.Ok();
        }
        public async Task<ServiceOperationResult> FinishExamAsync(Guid examId, Guid userId)
        {
            var existingExamResult = await _examRepository.GetByIdAsync(examId);
            if (!existingExamResult.Success || existingExamResult.Data == null)
                return ServiceOperationResult.Fail("Exam not found.", ServiceOperationErrorType.NotFound);
            var exam = existingExamResult.Data;
            var examUser = exam.ExamUsers.FirstOrDefault(eu => eu.UserId == userId);
            if (examUser == null)
                return ServiceOperationResult.Fail("User not found in the exam.", ServiceOperationErrorType.NotFound);
            examUser.CompleteStatus = true;

            foreach (var question in exam.Questions.Where(q => q.Type == QuestionType.MultiChoice).ToList())
            {
                double answerGrade = 0;

                var userAnswers = question.Answers.Where(answer => answer.UserId == userId).ToList();
                var correctAnswersCount = userAnswers.Select(a => a.QuestionOption).Count(qo => qo!.IsCorrect);
                var InCorrectAnswersCount = userAnswers.Select(a => a.QuestionOption).Count(qo => !qo!.IsCorrect);
                var questionOptionCount = question.QuestionOptions.Count();

                var totalCorrectOptions = question.QuestionOptions.Count(o => o.IsCorrect);
                if (totalCorrectOptions == 0) continue;

                answerGrade =
                    (double)question.MaxPoints * correctAnswersCount / totalCorrectOptions -
                    (double)question.MaxPoints * InCorrectAnswersCount / totalCorrectOptions;

                if (answerGrade < 0) answerGrade = 0;

                examUser.Grade += (int)Math.Round(answerGrade, 0);

                foreach (var answer in userAnswers)
                {
                    answer.IsGraded = true;
                }
            }

            var containsOpenAnswers = exam.Questions
                .Where(q => q.Type == QuestionType.Text)
                .Any();
            if (!containsOpenAnswers)
                examUser.IsChecked = true;

            var result = await _examRepository.UpdateAsync(exam);
            return result.Success
                ? ServiceOperationResult.Ok()
                : ServiceOperationResult.Fail("Failed to finish exam.", ServiceOperationErrorType.Internal);
        }
        public async Task<ServiceOperationResult> BlockExamUserByIdAsync(Guid examUserId)
        {
            var existingExamUserResult = await _examRepository.GetExamUserByIdAsync(examUserId);
            if (!existingExamUserResult.Success || existingExamUserResult.Data == null)
                return ServiceOperationResult.Fail("Exam user not found.", ServiceOperationErrorType.NotFound);

            var examUser = existingExamUserResult.Data;
            examUser.IsBlocked = true;
            examUser.Grade = 0;

            var existingExamResult = await _examRepository.GetByIdAsync(examUser.ExamId);
            if (!existingExamResult.Success || existingExamResult.Data == null)
                return ServiceOperationResult.Fail("Exam not found.", ServiceOperationErrorType.NotFound);

            var exam = existingExamResult.Data;

            var examUserInExam = exam.ExamUsers.FirstOrDefault(eu => eu.ExamUserId == examUserId);
            if (examUserInExam == null)
                return ServiceOperationResult.Fail("Exam user not found in the exam.", ServiceOperationErrorType.NotFound);

            examUserInExam.IsBlocked = true;
            examUserInExam.Grade = 0;

            var result = await _examRepository.UpdateAsync(exam);
            return result.Success
                ? ServiceOperationResult.Ok()
                : ServiceOperationResult.Fail("Failed to block exam user.", ServiceOperationErrorType.Internal);
        }
        public async Task<ServiceOperationResult<bool>> IsUserBlockedInExamAsync(Guid examId, Guid userId)
        {
            var existingExamResult = await _examRepository.GetByIdAsync(examId);
            if (!existingExamResult.Success || existingExamResult.Data == null)
                return ServiceOperationResult<bool>.Fail("Exam not found.", ServiceOperationErrorType.NotFound);
            var exam = existingExamResult.Data;
            var examUser = exam.ExamUsers.FirstOrDefault(eu => eu.UserId == userId);
            if (examUser == null)
                return ServiceOperationResult<bool>.Fail("User not found in the exam.", ServiceOperationErrorType.NotFound);
            return ServiceOperationResult<bool>.Ok(examUser.IsBlocked);
        }
        public async Task<ServiceOperationResult<bool>> IsExamInProgressAsync(Guid examId)
        {
            var existingExamResult = await _examRepository.GetByIdAsync(examId);
            if (!existingExamResult.Success || existingExamResult.Data == null)
                return ServiceOperationResult<bool>.Fail("Exam not found.", ServiceOperationErrorType.NotFound);
            var exam = existingExamResult.Data;
            var isInProgress = exam.Status == ExamStatus.Started;
            return ServiceOperationResult<bool>.Ok(isInProgress);
        }
        public async Task<ServiceOperationResult<IEnumerable<ExamUserDto>>> GetExpiredNotFinishedExamUsersAsync(DateTime now)
        {
            var result = await _examRepository.GetExpiredNotFinishedExamUsersAsync(now);
            if (!result.Success)
                return ServiceOperationResult<IEnumerable<ExamUserDto>>.Fail(result.ErrorMessage!, ServiceOperationErrorType.Internal);
            var examUserDtos = _mapper.Map<IEnumerable<ExamUserDto>>(result.Data);
            return ServiceOperationResult<IEnumerable<ExamUserDto>>.Ok(examUserDtos);
        }
    }
}
