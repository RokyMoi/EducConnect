using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Tutor
{
    public class TutorResendVerificationCodeRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}