using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Person
{
    public class PersonEducationInformationSaveRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string? InstitutionName { get; set; } = null;
        public string? InstitutionOfficialWebsite { get; set; } = null;
        public string? InstitutionAddress { get; set; } = null;
        [Required]
        public string EducationLevel { get; set; }
        [Required]
        public string FieldOfStudy { get; set; }
        public string? MinorFieldOfStudy { get; set; } = null;
        public DateOnly? StartDate { get; set; } = null;
        public DateOnly? EndDate { get; set; } = null;
        [Required]
        public bool IsCompleted { get; set; }
        public string? FinalGrade { get; set; }

        public string? Description { get; set; }

    }
}