using AutoMapper;
using ExamSystem.Application.DTOs;
using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Application.Utils.Validators;
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

        public async Task<RepositoryOperationResult<ExamForExaminatorDto>> CreateAsync(ExamCreateDto examCreateDto)
        {
            var existingUserResult = await _userRepository.GetByIdAsync(examCreateDto.CreatedByUserId);
            if (!existingUserResult.Success || existingUserResult.Data == null)
                return RepositoryOperationResult<ExamForExaminatorDto>.Fail("User who creates exam not found.");
            if (string.IsNullOrWhiteSpace(examCreateDto.Name))
                return RepositoryOperationResult<ExamForExaminatorDto>.Fail("Exam name is required.");

            var exam = _mapper.Map<Exam>(examCreateDto);
            exam.ExamId = Guid.NewGuid();
            exam.UserCreatedBy = existingUserResult.Data;
            exam.CreatedDate = DateTime.UtcNow;
            exam.JoinCode = Guid.NewGuid().ToString();

            var dateValidationResult = ExamValidator.ValidateDates(exam);
            if (!dateValidationResult.Success)
                return RepositoryOperationResult<ExamForExaminatorDto>.Fail(dateValidationResult.ErrorMessage!);

            var result = await _examRepository.AddAsync(exam);
            if (!result.Success)
                return RepositoryOperationResult<ExamForExaminatorDto>.Fail(result.ErrorMessage!);
            var examResult = await _examRepository.GetByIdAsync(exam.ExamId);
            if (!examResult.Success || examResult.Data == null)
                return RepositoryOperationResult<ExamForExaminatorDto>.Fail("Created exam not found");
            var examDto = _mapper.Map<ExamForExaminatorDto>(examResult.Data);
            return RepositoryOperationResult<ExamForExaminatorDto>.Ok(examDto);
        }


        public async Task<RepositoryOperationResult> DeleteAsync(Guid id)
        {
            var existingExamResult = await _examRepository.GetByIdAsync(id);

            if (!existingExamResult.Success || existingExamResult.Data == null)
                return RepositoryOperationResult.Fail("Exam not found.");

            var modifyAllowedResult = ExamValidator.IsModifyAllowed(existingExamResult.Data);
            if (!modifyAllowedResult.Success)
                return RepositoryOperationResult.Fail(modifyAllowedResult.ErrorMessage!);

            var result = await _examRepository.DeleteAsync(id);
            return result.Success
                ? RepositoryOperationResult.Ok()
                : RepositoryOperationResult.Fail("Failed to delete exam.");
        }

        public async Task<RepositoryOperationResult<IEnumerable<ExamForExaminatorDto>>> GetAllAsync()
        {
            var result = await _examRepository.GetAllAsync();
            if (!result.Success)
                return RepositoryOperationResult<IEnumerable<ExamForExaminatorDto>>.Fail(result.ErrorMessage!);
            if (!result.Data!.Any())
                return RepositoryOperationResult<IEnumerable<ExamForExaminatorDto>>.Fail("No exams found.");

            var examDtos = _mapper.Map<IEnumerable<ExamForExaminatorDto>>(result.Data);
            return RepositoryOperationResult<IEnumerable<ExamForExaminatorDto>>.Ok(examDtos);
        }

        public async Task<RepositoryOperationResult<IEnumerable<ExamForExaminatorDto>>> GetAllByCreatedUserIdAsync(Guid createdByUserId)
        {
            var result = await _examRepository.GetAllAsync();
            if (!result.Success)
                return RepositoryOperationResult<IEnumerable<ExamForExaminatorDto>>.Fail(result.ErrorMessage!);
            var exams = result.Data!;
            var filteredExams = exams.Where(e => e.CreatedByUserId == createdByUserId).ToList();
            if (!filteredExams.Any())
                return RepositoryOperationResult<IEnumerable<ExamForExaminatorDto>>.Fail("No exams found for this user.");
            var examDtos = _mapper.Map<IEnumerable<ExamForExaminatorDto>>(filteredExams);
            return RepositoryOperationResult<IEnumerable<ExamForExaminatorDto>>.Ok(examDtos);
        }

        public async Task<RepositoryOperationResult<IEnumerable<ExamForStudentDto>>> GetAllByParticipantUserIdAsync(Guid participantUserId)
        {
            var result = await _examRepository.GetAllAsync();
            if (!result.Success)
                return RepositoryOperationResult<IEnumerable<ExamForStudentDto>>.Fail(result.ErrorMessage!);

            var exams = result.Data!;

            var filteredExams = exams
                .Where(e => e.ExamUsers.Any(eu => eu.UserId == participantUserId))
                .ToList();

            if (!filteredExams.Any())
                return RepositoryOperationResult<IEnumerable<ExamForStudentDto>>.Fail("No exams found for this user.");

            var examDtos = _mapper.Map<List<ExamForStudentDto>>(filteredExams);

            for (int i = 0; i < examDtos.Count; i++)
            {
                var exam = filteredExams[i];
                var examUser = exam.ExamUsers.FirstOrDefault(eu => eu.UserId == participantUserId);
                examDtos[i].ExamUser = _mapper.Map<ExamUserDto>(examUser);
            }

            return RepositoryOperationResult<IEnumerable<ExamForStudentDto>>.Ok(examDtos);
        }

        public async Task<RepositoryOperationResult<ExamForExaminatorDto>> GetByIdAsync(Guid id)
        {
            var result = await _examRepository.GetByIdAsync(id);
            if (!result.Success)
                return RepositoryOperationResult<ExamForExaminatorDto>.Fail(result.ErrorMessage!);

            var examDto = _mapper.Map<ExamForExaminatorDto>(result.Data);
            return RepositoryOperationResult<ExamForExaminatorDto>.Ok(examDto);
        }
        public async Task<RepositoryOperationResult<ExamUserDto>> GetExamUserByIdAsync(Guid examUserId)
        {
            var result = await _examRepository.GetExamUserByIdAsync(examUserId);
            if (!result.Success)
                return RepositoryOperationResult<ExamUserDto>.Fail(result.ErrorMessage!);
            var examUserDto = _mapper.Map<ExamUserDto>(result.Data);
            return RepositoryOperationResult<ExamUserDto>.Ok(examUserDto);
        }
        public async Task<RepositoryOperationResult> UpdateAsync(Guid examId, ExamUpdateDto updateDto)
        {
            var existingExamResult = await _examRepository.GetByIdAsync(examId);
            if (!existingExamResult.Success || existingExamResult.Data == null)
                return RepositoryOperationResult.Fail("Exam not found.");

            var exam = existingExamResult.Data;

            var isAllowedResult = ExamValidator.IsModifyAllowed(exam);

            if (!isAllowedResult.Success)
                return RepositoryOperationResult.Fail(isAllowedResult.ErrorMessage!);

            exam.Name = updateDto.Name;
            exam.StartDate = updateDto.StartDate;
            exam.EndDate = updateDto.EndDate;

            var dateValidationResult = ExamValidator.ValidateDates(exam);
            if (!dateValidationResult.Success)
                return RepositoryOperationResult.Fail(dateValidationResult.ErrorMessage!);

            var updateResult = await _examRepository.UpdateAsync(exam);
            return updateResult.Success
                ? RepositoryOperationResult.Ok()
                : RepositoryOperationResult.Fail("Failed to update exam.");
        }

        public async Task<RepositoryOperationResult<bool>> IsParticipantAsync(Guid examId, Guid userId)
        {
            var examResult = await _examRepository.GetByIdAsync(examId);
            if (!examResult.Success || examResult.Data == null)
                return RepositoryOperationResult<bool>.Fail("Exam not found.");
            var exam = examResult.Data;
            var isParticipant = exam.ExamUsers.Any(eu => eu.UserId == userId);
            return RepositoryOperationResult<bool>.Ok(isParticipant);
        }

        public async Task<RepositoryOperationResult> AddParticipantAsync(Guid examId, Guid userId)
        {
            var existingExamResult = await _examRepository.GetByIdAsync(examId);
            if (!existingExamResult.Success || existingExamResult.Data == null)
                return RepositoryOperationResult.Fail("Exam not found.");
            var existingUserResult = await _userRepository.GetByIdAsync(userId);
            if (!existingUserResult.Success || existingUserResult.Data == null)
                return RepositoryOperationResult.Fail("User not found.");
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
                ? RepositoryOperationResult.Ok()
                : RepositoryOperationResult.Fail("Failed to add participant.");
        }

        public async Task<RepositoryOperationResult> RemoveParticipantAsync(Guid examId, Guid userId)
        {
            var existingExamResult = await _examRepository.GetByIdAsync(examId);
            if (!existingExamResult.Success || existingExamResult.Data == null)
                return RepositoryOperationResult.Fail("Exam not found.");
            var existingUserResult = await _userRepository.GetByIdAsync(userId);
            if (!existingUserResult.Success || existingUserResult.Data == null)
                return RepositoryOperationResult.Fail("User not found.");
            var exam = existingExamResult.Data;

            var modifyAllowedResult = ExamValidator.IsModifyAllowed(exam);
            if (!modifyAllowedResult.Success)
                return RepositoryOperationResult.Fail(modifyAllowedResult.ErrorMessage!);

            var user = existingUserResult.Data;
            var examUser = exam.ExamUsers.FirstOrDefault(eu => eu.UserId == userId);
            if (examUser != null)
            {
                exam.ExamUsers.Remove(examUser);
                var result = await _examRepository.UpdateAsync(exam);
                return result.Success
                    ? RepositoryOperationResult.Ok()
                    : RepositoryOperationResult.Fail("Failed to remove participant.");
            }
            return RepositoryOperationResult.Fail("Participant not found in the exam.");
        }

        public async Task<RepositoryOperationResult> AddParticipantByEmailAsync(Guid examId, string email)
        {
            var existingExamResult = await _examRepository.GetByIdAsync(examId);
            if (!existingExamResult.Success || existingExamResult.Data == null)
                return RepositoryOperationResult.Fail("Exam not found.");
            var existingUserResult = await _userRepository.GetByEmailAsync(email);
            if (!existingUserResult.Success || existingUserResult.Data == null)
                return RepositoryOperationResult.Fail("User not found.");
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
                ? RepositoryOperationResult.Ok()
                : RepositoryOperationResult.Fail("Failed to add participant.");
        }

        public async Task<RepositoryOperationResult> JoinExam(JoinExamDto joinExamDto)
        {
            var examsResult = await _examRepository.GetAllAsync();
            if (!examsResult.Success || examsResult.Data == null)
                return RepositoryOperationResult.Fail("No exams found.");
            var exam = examsResult.Data.FirstOrDefault(e => e.JoinCode == joinExamDto.JoinCode);
            if (exam == null)
                return RepositoryOperationResult.Fail("Wrong join code.");

            var modifyAllowedResult = ExamValidator.IsModifyAllowed(exam);
            if (!modifyAllowedResult.Success)
                return RepositoryOperationResult.Fail(modifyAllowedResult.ErrorMessage!);

            var userResult = await _userRepository.GetByIdAsync(joinExamDto.UserId);
            if (!userResult.Success || userResult.Data == null)
                return RepositoryOperationResult.Fail("User not found.");
            if (exam.ExamUsers.Any(eu => eu.UserId == joinExamDto.UserId))
                return RepositoryOperationResult.Fail("User already joined the exam.");
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
                return RepositoryOperationResult.Fail("Failed to join exam.");
            return RepositoryOperationResult.Ok();
        }
        public async Task<RepositoryOperationResult> FinishExamAsync(Guid examId, Guid userId)
        {
            var existingExamResult = await _examRepository.GetByIdAsync(examId);
            if (!existingExamResult.Success || existingExamResult.Data == null)
                return RepositoryOperationResult.Fail("Exam not found.");
            var exam = existingExamResult.Data;
            var examUser = exam.ExamUsers.FirstOrDefault(eu => eu.UserId == userId);
            if (examUser == null)
                return RepositoryOperationResult.Fail("User not found in the exam.");
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
                ? RepositoryOperationResult.Ok()
                : RepositoryOperationResult.Fail("Failed to finish exam.");
        }
        public async Task<RepositoryOperationResult> BlockExamUserByIdAsync(Guid examUserId)
        {
            var existingExamUserResult = await _examRepository.GetExamUserByIdAsync(examUserId);
            if (!existingExamUserResult.Success || existingExamUserResult.Data == null)
                return RepositoryOperationResult.Fail("Exam user not found.");

            var examUser = existingExamUserResult.Data;
            examUser.IsBlocked = true;
            examUser.Grade = 0;

            var existingExamResult = await _examRepository.GetByIdAsync(examUser.ExamId);
            if (!existingExamResult.Success || existingExamResult.Data == null)
                return RepositoryOperationResult.Fail("Exam not found.");

            var exam = existingExamResult.Data;

            var examUserInExam = exam.ExamUsers.FirstOrDefault(eu => eu.ExamUserId == examUserId);
            if (examUserInExam == null)
                return RepositoryOperationResult.Fail("Exam user not found in the exam.");

            examUserInExam.IsBlocked = true;
            examUserInExam.Grade = 0;

            var result = await _examRepository.UpdateAsync(exam);
            return result.Success
                ? RepositoryOperationResult.Ok()
                : RepositoryOperationResult.Fail("Failed to block exam user.");
        }
        public async Task<RepositoryOperationResult<bool>> IsUserBlockedInExamAsync(Guid examId, Guid userId)
        {
            var existingExamResult = await _examRepository.GetByIdAsync(examId);
            if (!existingExamResult.Success || existingExamResult.Data == null)
                return RepositoryOperationResult<bool>.Fail("Exam not found.");
            var exam = existingExamResult.Data;
            var examUser = exam.ExamUsers.FirstOrDefault(eu => eu.UserId == userId);
            if (examUser == null)
                return RepositoryOperationResult<bool>.Fail("User not found in the exam.");
            return RepositoryOperationResult<bool>.Ok(examUser.IsBlocked);
        }
        public async Task<RepositoryOperationResult<bool>> IsExamInProgressAsync(Guid examId)
        {
            var existingExamResult = await _examRepository.GetByIdAsync(examId);
            if (!existingExamResult.Success || existingExamResult.Data == null)
                return RepositoryOperationResult<bool>.Fail("Exam not found.");
            var exam = existingExamResult.Data;
            var isInProgress = exam.Status == ExamStatus.Started;
            return RepositoryOperationResult<bool>.Ok(isInProgress);
        }
        public async Task<RepositoryOperationResult<IEnumerable<ExamUserDto>>> GetExpiredNotFinishedExamUsersAsync(DateTime now)
        {
            var result = await _examRepository.GetExpiredNotFinishedExamUsersAsync(now);
            if (!result.Success)
                return RepositoryOperationResult<IEnumerable<ExamUserDto>>.Fail(result.ErrorMessage!);
            var examUserDtos = _mapper.Map<IEnumerable<ExamUserDto>>(result.Data);
            return RepositoryOperationResult<IEnumerable<ExamUserDto>>.Ok(examUserDtos);
        }
    }
}
