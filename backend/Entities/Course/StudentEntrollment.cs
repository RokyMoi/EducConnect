using EduConnect.Entities.Student;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Entities.Course
{
    public enum EnrollmentStatus
    {
        Active,
        Completed,
        Dropped
    }
    public class StudentEnrollment
    {
        public Guid StudentEnrollmentId { get; set; }
      
        public Guid StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public Student.Student Student { get; set; }
        public Guid CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]

        public Course Course { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public EnrollmentStatus Status { get; set; }
    }
}
