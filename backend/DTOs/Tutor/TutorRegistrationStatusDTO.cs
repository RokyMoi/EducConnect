using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Tutor
{
    public class TutorRegistrationStatusDTO
    {
        public Guid TutorId { get; set; }
        public Guid PersonId { get; set; }
        public int TutorRegistrationStatusId { get; set; }
        public string? TutorRegistrationStatusName { get; set; }
        public string? TutorRegistrationStatusDescription { get; set; }
        public bool? IsSkippable { get; set; }
    }
}