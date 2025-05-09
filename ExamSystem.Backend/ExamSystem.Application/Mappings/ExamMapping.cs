using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ExamSystem.Application.DTOs;
using ExamSystem.Core.Entities;

namespace ExamSystem.Application.Mappings
{
    public class ExamMapping : Profile
    {
        public ExamMapping()
        {
            CreateMap<Exam, ExamDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.UserCreatedBy))
                .ForMember(dest => dest.QuestionCount, opt => opt.MapFrom(src => src.Questions.Count))
                .ForMember(dest => dest.ParticipantCount, opt => opt.MapFrom(src => src.ExamUsers.Count));

            CreateMap<Exam, ExamForExaminatorDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.UserCreatedBy))
                .ForMember(dest => dest.QuestionCount, opt => opt.MapFrom(src => src.Questions.Count))
                .ForMember(dest => dest.ParticipantCount, opt => opt.MapFrom(src => src.ExamUsers.Count))
                .ForMember(dest => dest.JoinCode, opt => opt.MapFrom(src => src.JoinCode))
                .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.ExamUsers.Select(eu => new ExamUserDto
                {
                    UserId = eu.UserId,
                    FirstName = eu.User.FirstName,
                    LastName = eu.User.LastName,
                    Email = eu.User.Email,
                    CompleteStatus = eu.CompleteStatus,
                    Grade = eu.Grade,
                    IsBlocked = eu.IsBlocked,
                    Violations = eu.Violations.Select(v => new ViolationDto
                    {
                        ExamUserId = v.ExamUserId,
                        ViolationId = v.ViolationId,
                        Description = v.Description,
                        ViolationType = v.ViolationType.ToString()
                    }).ToList()
                })));

            CreateMap<Exam, ExamForStudentDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.UserCreatedBy))
                .ForMember(dest => dest.QuestionCount, opt => opt.MapFrom(src => src.Questions.Count))
                .ForMember(dest => dest.ParticipantCount, opt => opt.MapFrom(src => src.ExamUsers.Count));


            CreateMap<ExamCreateDto, Exam>();
            CreateMap<ExamUpdateDto, Exam>();

            CreateMap<ExamUser, ExamUserDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Violations, opt => opt.MapFrom(src => src.Violations.Select(v => new ViolationDto
                {
                    ExamUserId = v.ExamUserId,
                    ViolationId = v.ViolationId,
                    Description = v.Description,
                    ViolationType = v.ViolationType.ToString()
                })));
        }
    }
}
