using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Entities.Course
{
    //CourseGeneralClassesOccurenceInformation(uuid) – PRIMARY KEY

//CourseId – (uuid) – FOREIGN KEY TO „Course“.“Course“ 

//DayOfTheWeekWhenClassOccurs – (integer) – (min: 0, max: 6) 

//ClassStartTime – (string)

//ClassEndTime – (string)

//CreatedAt – (bigint) – UNIX millis

//ModifiedAt – (bigint) – UNIX millis – NULLABLE
    public class CourseClassesWeeklyOccurenceInformation
    {
        [Key]
        public required Guid CourseGeneralClassesOccurenceInformation { get; set; }
        public required Guid CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }
        public required int DayOfTheWeekWhenClassOccurs { get; set; }
        public required string ClassStartTime { get; set; }
        public required string ClassEndTime { get; set; }
        public required long CreatedAt {  get; set; }
        public long? ModifiedAt {  get; set; }


    }
}
