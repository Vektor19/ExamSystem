using AutoMapper;
using ExamSystem.Application.DTOs;
using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Enums;
using ExamSystem.Core.Interfaces.Repositories;

namespace ExamSystem.Application.Services
{
    public class ViolationService : IViolationService
    {
        private readonly IViolationRepository _violationRepository;
        private readonly IMapper _mapper;
        public ViolationService(IMapper mapper, IViolationRepository violationRepository)
        {
            _violationRepository = violationRepository;
            _mapper = mapper;
        }
        public async Task<OperationResult<ViolationDto>> GetByIdAsync(Guid violationId)
        {
            var result = await _violationRepository.GetByIdAsync(violationId);
            if (!result.Success)
                return OperationResult<ViolationDto>.Fail(result.ErrorMessage!);

            var violationDto = _mapper.Map<ViolationDto>(result.Data);
            return OperationResult<ViolationDto>.Ok(violationDto);
        }
        public async Task<OperationResult<IEnumerable<ViolationDto>>> GetAllAsync()
        {
            var result = await _violationRepository.GetAllAsync();
            if (!result.Success)
                return OperationResult<IEnumerable<ViolationDto>>.Fail(result.ErrorMessage!);
            if (!result.Data!.Any())
                return OperationResult<IEnumerable<ViolationDto>>.Fail("No violations found.");

            var violationDtos = _mapper.Map<IEnumerable<ViolationDto>>(result.Data);
            return OperationResult<IEnumerable<ViolationDto>>.Ok(violationDtos);
        }
        public async Task<OperationResult<IEnumerable<ViolationDto>>> GetAllByExamUserIdAsync(Guid examUserId)
        {
            var result = await _violationRepository.GetAllByExamUserIdAsync(examUserId);
            if (!result.Success)
                return OperationResult<IEnumerable<ViolationDto>>.Fail(result.ErrorMessage!);
            if (!result.Data!.Any())
                return OperationResult<IEnumerable<ViolationDto>>.Fail("No violations found for this exam user.");
            var violationDtos = _mapper.Map<IEnumerable<ViolationDto>>(result.Data);
            return OperationResult<IEnumerable<ViolationDto>>.Ok(violationDtos);
        }
        public async Task<OperationResult> DeleteAsync(Guid violationId)
        {
            var result = await _violationRepository.DeleteAsync(violationId);
            return result.Success
                ? OperationResult.Ok()
                : OperationResult.Fail("Failed to delete violation.");
        }
        public async Task<OperationResult<ViolationDto>> CreateAsync(CreateViolationDto createViolationDto)
        {
            var violation = _mapper.Map<Violation>(createViolationDto);
            violation.ViolationId = Guid.NewGuid();

            var result = await _violationRepository.AddAsync(violation);

            if (result.Success)
            {
                var violationDto = _mapper.Map<ViolationDto>(violation);
                return OperationResult<ViolationDto>.Ok(violationDto);
            }
            return OperationResult<ViolationDto>.Fail("Failed to create violation.");
        }
    }
}
