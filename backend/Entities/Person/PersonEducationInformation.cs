using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Entities.Person;
using Microsoft.Identity.Client;

namespace backend.Entities.Person
{
    [Table("PersonEducationInformation", Schema = "Person")]
    public class PersonEducationInformation
    {
        [Key]
        public Guid PersonEducationInformationId { get; set; }
        public Guid PersonId { get; set; }
        [ForeignKey(nameof(PersonId))]
        public EduConnect.Entities.Person.Person Person { get; set; }
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