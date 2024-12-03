using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Person
{
    public class PersonEducationInformationResponseDTO
    {
        public string? InstitutionName { get; set; } = null;
        public string? InstitutionOfficialWebsite { get; set; } = null;
        public string? InstitutionAddress { get; set; } = null;
        public string EducationLevel { get; set; }
        public string FieldOfStudy { get; set; }
        public string? MinorFieldOfStudy { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

        public bool IsCompleted { get; set; }
        public string? FinalGrade { get; set; }

        public string? Description { get; set; }
    }
}