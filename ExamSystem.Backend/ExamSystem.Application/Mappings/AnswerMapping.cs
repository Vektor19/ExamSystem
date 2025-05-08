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
    public class AnswerMapping : Profile
    {
        public AnswerMapping()
        {
            CreateMap<Answer, OptionAnswerDto>().ReverseMap();
            CreateMap<OpenAnswerDto, Answer>()
                .ForMember(dest => dest.AnswerText, opt => opt.MapFrom(src => src.AnswerText))
                .ForMember(dest => dest.QuestionOptionId, opt => opt.Ignore());

            CreateMap<OptionAnswerDto, Answer>()
                .ForMember(dest => dest.QuestionOptionId, opt => opt.MapFrom(src => src.QuestionOptionId))
                .ForMember(dest => dest.AnswerText, opt => opt.Ignore());

            CreateMap<CreateOpenAnswerDto, Answer>()
                .ForMember(dest => dest.AnswerText, opt => opt.MapFrom(src => src.AnswerText))
                .ForMember(dest => dest.QuestionOptionId, opt => opt.Ignore());
            CreateMap<CreateOptionAnswerDto, Answer>()
                .ForMember(dest => dest.QuestionOptionId, opt => opt.MapFrom(src => src.QuestionOptionId))
                .ForMember(dest => dest.AnswerText, opt => opt.Ignore());
        }
    }
}
