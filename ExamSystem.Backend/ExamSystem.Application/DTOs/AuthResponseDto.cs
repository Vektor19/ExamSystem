using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.DTOs
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }

}
