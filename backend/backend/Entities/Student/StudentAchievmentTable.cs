using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Entities.Student
{
    public class StudentAchievmentTable
    {
        //        StudentAchievmentId – (uuid) PRIMARY KEY, 

        //StudentId – (uuid) FOREIGN KEY TO „Student“.“Student“ 

        //AchievmentId – (uuid)* Kasnije implementirati kroz tabelu
        //za sistem dodijele uspjeha i medalja*

        //AwardedBy – (boolean) – true – awarder by Tutor, false awarder by system

        //CreatedAt – (bigint) – UNIX millis

        //ModifiedAt – (bigint) – UNIX millis – NULLABLE
        [Key]
        public required Guid StudentAchievmentId { get; set; }
        public required Guid StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }
        public required bool AwardedBy { get; set; }
        public required long CreatedAt { get; set; }
        public long? ModifiedAt { get; set; }



    }
}
