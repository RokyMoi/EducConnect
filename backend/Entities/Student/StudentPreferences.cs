using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Entities.Student
{
    [Table("StudentPreferences", Schema = "Student")]
    public class StudentPreferences
    {
        //        StudentPreferenceId – (uuid) PRIMARY KEY

        //StudentId – (uuid) FOREIGN KEY TO „Student“.“Student“ 

        //SubjectCategoryId – (uuid)* Kasnije implementirati kroz tabelu za
        //kategorizaciju stručnih oblasti, trenutno postaviti STRING NULLABLE* 

        //SpecificExpertiseAreaId – (uuid)* Kasnije implementirati kroz tabelu za klasifikaciju stručnih usmjerenja, trenutno postaviti STRING NULLABLE* 

        //InterestLevel – (number) (min: 1, max: 5) 

        //IsActive – boolean

        //CreatedAt – (bigint) – UNIX millis

        //ModifiedAt – (bigint) – UNIX millis – NULLABLE
        [Key]
        public required Guid StudentPreferenceId { get; set; }
        
        public required Guid StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }
        public string? SubjectCategoryId { get; set; }
        public string? SpecificExpertiseAreaId { get; set; }
        public required int InterestLevel { get; set; }
        public required bool IsActive { get; set; }
        public required long CreatedAt { get; set; }
        public long? ModifiedAt { get; set; }

    }
}
