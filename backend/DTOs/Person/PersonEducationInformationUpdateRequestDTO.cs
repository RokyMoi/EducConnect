using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Person
{
    public class PersonEducationInformationUpdateRequestDTO
    {


        [Required]
        public Guid PersonEducationInformationId { get; set; }

        public string? InstitutionName { get; set; } = null;
        public string? InstitutionOfficialWebsite { get; set; } = null;
        public string? InstitutionAddress { get; set; } = null;
        public string? EducationLevel { get; set; } = null;
        public string? FieldOfStudy { get; set; } = null;
        public string? MinorFieldOfStudy { get; set; } = null;
        public DateOnly? StartDate { get; set; } = null;
        public DateOnly? EndDate { get; set; } = null;
        public bool? IsCompleted { get; set; } = null;
        public string? FinalGrade { get; set; } = null;
        public string? Description { get; set; } = null;
    }
}