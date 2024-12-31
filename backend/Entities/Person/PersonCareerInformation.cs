using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities.Reference;

namespace backend.Entities.Person
{
    [Table("PersonCareerInformation", Schema = "Person")]
    public class PersonCareerInformation
    {
        [Key]
        [Required]
        public required Guid PersonCareerInformationId { get; set; }
        [Required]
        public required Guid PersonId { get; set; }
        //Navigation property
        [ForeignKey(nameof(PersonId))]
        public EduConnect.Entities.Person.Person Person { get; set; }
        [Required]
        public required string CompanyName { get; set; }
        public string? CompanyWebsite { get; set; } = string.Empty;
        [Required]
        public required string JobTitle { get; set; }
        public string? Position { get; set; } = string.Empty;
        [Required]
        public required string CityOfEmployment { get; set; }
        [Required]
        public required string CountryOfEmployment { get; set; }
        [Required]
        public required int EmploymentTypeId { get; set; }
        //Navigation property
        [ForeignKey(nameof(EmploymentTypeId))]
        public EmploymentType EmploymentType { get; set; }
        [Required]
        public required DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? JobDescription { get; set; } = string.Empty;
        public string? Responsibilities { get; set; } = string.Empty;
        public string? Achievements { get; set; } = string.Empty;
        [Required]
        public required Guid IndustryClassificationId { get; set; }

        //Navigation property
        [ForeignKey(nameof(IndustryClassificationId))]
        public IndustryClassification IndustryClassification { get; set; }
        [Required]
        public required string SkillsUsed { get; set; }

        public int? WorkTypeId { get; set; } = null;

        //Navigation property
        [ForeignKey(nameof(WorkTypeId))]
        public WorkType? WorkType { get; set; }

        public string? AdditionalInformation { get; set; } = string.Empty;

        [Required]
        public required long CreatedAt { get; set; }
        public long? UpdatedAt { get; set; }



    }
}