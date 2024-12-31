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
        public Guid PersonCareerInformationId { get; set; }

        public string? CompanyName { get; set; } = string.Empty;
        public bool updateCompanyName { get; set; } = false;
        public string? CompanyWebsite { get; set; } = string.Empty;
        public bool updateCompanyWebsite { get; set; } = false;
        public string? JobTitle { get; set; } = string.Empty;
        public bool updateJobTitle { get; set; } = false;
        public string? Position { get; set; } = string.Empty;
        public bool updatePosition { get; set; } = false;
        public string? CityOfEmployment { get; set; } = string.Empty;
        public bool updateCityOfEmployment { get; set; } = false;
        public string? CountryOfEmployment { get; set; } = string.Empty;
        public bool updateCountryOfEmployment { get; set; } = false;
        public int? EmploymentTypeId { get; set; }
        public bool updateEmploymentTypeId { get; set; } = false;
        public DateOnly? StartDate { get; set; }
        public bool updateStartDate { get; set; } = false;

        public DateOnly? EndDate { get; set; }
        public bool updateEndDate { get; set; } = false;
        public string? JobDescription { get; set; } = string.Empty;
        public bool updateJobDescription { get; set; } = false;
        public string? Responsibilities { get; set; } = string.Empty;
        public bool updateResponsibilities { get; set; } = false;
        public string? Achievements { get; set; } = string.Empty;
        public bool updateAchievements { get; set; } = false;
        public Guid? IndustryClassificationId { get; set; }
        public bool updateIndustryClassificationId { get; set; } = false;

        public string? SkillsUsed { get; set; } = string.Empty;
        public bool updateSkillsUsed { get; set; } = false;

        public int? WorkTypeId { get; set; } = null;
        public bool updateWorkTypeId { get; set; } = false;


        public string? AdditionalInformation { get; set; } = string.Empty;
        public bool updateAdditionalInformation { get; set; } = false;




    }
}