using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace backend.DTOs.Person
{
    public class PersonCareerInformationControllerSaveRequestDTO
    {


        [Required]
        public string CompanyName { get; set; }

        public string? CompanyWebsite { get; set; }
        [Required]
        public string JobTitle { get; set; }

        public string? Position { get; set; }

        [Required]
        public string CityOfEmployment { get; set; }

        public string CountryOfEmployment { get; set; }
        [Required]
        public int EmploymentTypeId { get; set; }

        [Required]
        public DateOnly StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public string? JobDescription { get; set; } = string.Empty;
        public string? Responsibilities { get; set; } = string.Empty;
        public string? Achievements { get; set; } = string.Empty;
        [Required]
        public required Guid IndustryClassificationId { get; set; }
        [Required]
        public required string SkillsUsed { get; set; }

        public int? WorkType { get; set; } = null;

        public string? AdditionalInformation { get; set; } = string.Empty;
    }
}