using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Person
{
    public class PersonCareerInformationUpdateRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public Guid PersonCareerInformationId { get; set; }

        public string? CompanyName { get; set; }

        [Required]
        public bool UpdateCompanyWebsite { get; set; }
        public string? CompanyWebsite { get; set; }

        public string? JobTitle { get; set; }

        [Required]
        public bool UpdatePosition { get; set; }

        public string? Position { get; set; }

        public string? CityOfEmployment { get; set; }

        public string? CountryOfEmployment { get; set; }

        public int? EmploymentType { get; set; }

        public DateOnly? StartDate { get; set; }

        [Required]
        public bool UpdateEndDate { get; set; }

        public DateOnly? EndDate { get; set; }

        [Required]
        public bool UpdateJobDescription { get; set; }

        public string? JobDescription { get; set; }

        [Required]
        public bool UpdateResponsibilities { get; set; }

        public string? Responsibilities { get; set; }

        [Required]
        public bool UpdateAchievements { get; set; }

        public string? Achievements { get; set; }

        public string? Industry { get; set; }

        public string? SkillsUsed { get; set; }

        [Required]
        public bool UpdateWorkType { get; set; }

        public int? WorkType { get; set; }

        [Required]
        public bool UpdateAdditionalInformation { get; set; }

        public string? AdditionalInformation { get; set; }




    }
}