using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace EduConnect.Entities.Reference
{
    [Table("CourseCreationCompletenessStep", Schema = "Reference")]
    public class CourseCreationCompletenessStep
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required Guid CourseCreationCompletenessStepId { get; set; }

        public required int StepOrder { get; set; } = 0;

        public required string StepName { get; set; } = "Basic Information";


    }
}