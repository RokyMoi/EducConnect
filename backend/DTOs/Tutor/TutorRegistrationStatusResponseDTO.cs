using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Tutor
{
    public class TutorRegistrationStatusResponseDTO
    {
        public Guid TutorId { get; set; }
        public int TutorRegistrationStatusId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool? IsSkippable { get; set; }
    }
}