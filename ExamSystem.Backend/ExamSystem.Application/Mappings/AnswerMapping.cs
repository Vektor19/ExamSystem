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
            CreateMap<Answer, AnswerDto>().ReverseMap();
        }
    }
}
