using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Entities.Course;

namespace EduConnect.Entities.Course
{
    [Table("CourseLesson", Schema = "Course")]
    public class CourseLesson
    {
        [Key]
        public Guid CourseLessonId { get; set; } = Guid.NewGuid();

        public Guid CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        public Course? Course { get; set; } = null;

        public string Title { get; set; }

        //This is field that is used to display a short summary of the lesson in the course overview page
        public string ShortSummary { get; set; }

        //This is the actual description of the lesson that is displayed in the lesson page
        public string Description { get; set; }

        public string Topic { get; set; }

        public int? LessonSequenceOrder { get; set; } = null;

        //Status of the lesson, represents a flag that indicates lesson status, with the following values:
        //false - not published
        //true - published
        //null - archived
        public bool? PublishedStatus { get; set; } = false;

        public DateTime? StatusChangedAt { get; set; } = null;

        public Guid TutorId { get; set; }

        [ForeignKey(nameof(TutorId))]
        public EduConnect.Entities.Tutor.Tutor? Tutor { get; set; } = null;


        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        public long? UpdatedAt { get; set; } = null;

        public virtual CourseLessonContent? CourseLessonContent { get; set; } = null;





    }


}