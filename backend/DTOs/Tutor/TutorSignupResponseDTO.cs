using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Tutor
{
    public class TutorSignupResponseDTO
    {
        public Guid TutorId { get; set; }
        public string Email { get; set; }

    }
}