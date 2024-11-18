using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Entities.Course
{
    public class Course
    {
        //        CourseId – (uuid), PRIMARY KEY

        //TutorId – (uuid), FOREIGN KEY TO „Tutor“.“TutorId“ 

        //CourseName – (string)

        //ShortDescription – (string), (max: 1052) 

        //DetailedDescription – (string), (max: 4kb)  

        //CourseCategory – (uuid), FOREIGN KEY, **Implementirati kasnije kroz tabelu kategorizacije stručnih oblasti

        //CourseField – (uuid), FOREIGN KEY, **Implementirati kasnije kroz tabelu klasifikacije stručnih interesovanja

        //IsUniqueInstance – (boolean)

        //PlannedNumberOfClasses – (int)

        //CreatedAt – (bigint) – UNIX millis

        //ModifiedAt – (bigint) – UNIX millis – NULLABLE
        [Key]
        public required Guid CourseId { get; set; }
        public required Guid TutorId { get; set; }
        [ForeignKey(nameof(TutorId))]
        public Tutor.Tutor Tutor { get; set; }
        public required string CourseName { get; set; }
        public required string ShortDescription { get; set; }  
        public required string DetailedDescription { get; set; }
        public string? CourseCategory {  get; set; }
        public string? CourseField { get; set; }
        public required bool UniqueInstance { get; set; }
        public required int PlannedNumberOfClasses  { get; set; }
        public long CreatedAt {  get; set; }
        public long? ModifiedAt { get; set; }




    }
}
