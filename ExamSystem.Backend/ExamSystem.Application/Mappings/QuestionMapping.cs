using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ExamSystem.Application.DTOs.Question;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Enums;

namespace ExamSystem.Application.Mappings
{
    public class QuestionMapping : Profile
    {
        public QuestionMapping()
        {
            CreateMap<Question, QuestionDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.QuestionOptions));

            CreateMap<QuestionCreateDto, Question>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<QuestionType>(src.Type)))
                .ForMember(dest => dest.QuestionOptions, opt => opt.MapFrom(src => src.Options));
            CreateMap<QuestionUpdateDto, Question>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<QuestionType>(src.Type)))
                .ForMember(dest => dest.QuestionOptions, opt => opt.MapFrom(src => src.Options));

        }
    }
}
