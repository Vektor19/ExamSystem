using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ExamSystem.Application.DTOs.Violation;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Enums;

namespace ExamSystem.Application.Mappings
{
    public class ViolationMapping : Profile
    {
        public ViolationMapping()
        {
            CreateMap<Violation, ViolationDto>()
                .ForMember(dest => dest.ViolationType, opt => opt.MapFrom(src => src.ViolationType.ToString()));
            CreateMap<CreateViolationDto, Violation>()
                .ForMember(dest => dest.ViolationType, opt => opt.MapFrom(src => Enum.Parse<ViolationType>(src.ViolationType)));
        }
    }
}
