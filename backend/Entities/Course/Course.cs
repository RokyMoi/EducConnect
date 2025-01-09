using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Entities.Reference;
using EduConnect.Entities.Reference;

namespace EduConnect.Entities.Course
{
    [Table("Course", Schema = "Course")]
    public class Course
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required Guid CourseId { get; set; }
        public required Guid TutorId { get; set; }

        //Navigation property
        [ForeignKey("TutorId")]
        public Tutor.Tutor? Tutor { get; set; }

        public required string CourseName { get; set; }

        public required string CourseSubject { get; set; }


        public required bool IsDraft { get; set; }

        public required Guid CourseCreationCompletenessStepId { get; set; }

        [ForeignKey("CourseCreationCompletenessStepId")]
        public CourseCreationCompletenessStep? CourseCreationCompletenessStep { get; set; } = null;


        public required long CreatedAt { get; set; } = DateTimeOffset.Now.ToUnixTimeSeconds();

        public long? UpdatedAt { get; set; } = null;




    }
}
