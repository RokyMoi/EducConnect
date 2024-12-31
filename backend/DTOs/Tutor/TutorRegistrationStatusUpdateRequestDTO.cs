using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Tutor
{
    public class TutorRegistrationStatusUpdateRequestDTO
    {

        [Required]
        public int tutorRegistrationStatusId { get; set; }
    }
}