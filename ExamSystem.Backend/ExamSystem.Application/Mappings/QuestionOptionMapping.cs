using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ExamSystem.Application.DTOs;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Enums;

namespace ExamSystem.Application.Mappings
{
    public class QuestionOptionMapping : Profile
    {
        public QuestionOptionMapping()
        {
            CreateMap<QuestionOption, QuestionOptionDto>();
            CreateMap<QuestionOptionCreateDto, QuestionOption>();
        }
    }
}
