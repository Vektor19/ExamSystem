using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.DTOs
{
    public class ExamForExaminatorDto: ExamDto
    {
        public string JoinCode { get; set; } = string.Empty;
        public List<ExamUserDto> Participants { get; set; } = [];
    }
}
