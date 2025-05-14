using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.DTOs
{
    public class ExamForStudentDto : ExamDto
    {
        public ExamUserDto? ExamUser { get; set; }
    }
}
