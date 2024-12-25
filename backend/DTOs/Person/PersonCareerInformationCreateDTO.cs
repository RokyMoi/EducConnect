using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Person
{
    public class PersonCareerInformationCreateDTO
    {


        public Guid PersonId { get; set; }
        public string CompanyName { get; set; }
        public string? CompanyWebsite { get; set; }
        public string JobTitle { get; set; }
        public string? Position { get; set; }
        public string CityOfEmployment { get; set; }
        public string CountryOfEmployment { get; set; }
        public int EmploymentTypeId { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly? EndDate { get; set; }
        public string? JobDescription { get; set; }
        public string? Responsibilities { get; set; }
        public string? Achievements { get; set; }
        public Guid IndustryClassificationId { get; set; }
        public string SkillsUsed { get; set; }
        public int? WorkTypeId { get; set; }
        public string? AdditionalInformation { get; set; }

    }
}