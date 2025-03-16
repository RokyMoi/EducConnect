using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Entities.Reference;
using backend.Entities.Reference.Language;
using backend.Entities.Reference.Learning;
using EduConnect.Entities.Reference;

namespace EduConnect.Entities.Course
{
    [Table("Course", Schema = "Course")]
    public class Course
    {

        public Guid CourseId { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid CourseCategoryId { get; set; }

        [ForeignKey(nameof(CourseCategoryId))]
        public CourseCategory? CourseCategory { get; set; } = null;

        public Guid LanguageId { get; set; }

        [ForeignKey(nameof(LanguageId))]
        public Language? Language { get; set; } = null;

        public Guid TutorId { get; set; }

        [ForeignKey(nameof(TutorId))]
        public Tutor.Tutor? Tutor { get; set; } = null;


        public int LearningDifficultyLevelId { get; set; }

        [ForeignKey(nameof(LearningDifficultyLevelId))]
        public LearningDifficultyLevel? LearningDifficultyLevel { get; set; } = null;

        public long EstimatedDurationMinutes { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } = 0;

        public int? MinNumberOfStudents { get; set; } = null;

        public int? MaxNumberOfStudents { get; set; } = null;

        //Flag value which indicates the status of the course
        //True - published
        //False - draft
        //Null - archived

        public bool? PublishedStatus { get; set; } = false;
        public List<string> Tags { get; set; } = new List<string>();


        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        public long? UpdatedAt { get; set; } = null;





    }
}
