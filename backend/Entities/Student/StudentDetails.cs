using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Entities.Student
{
    public class StudentDetails
    {
        [Key]
        public required Guid StudentDetailsId { get; set; }
        public required Guid StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }
        public string? Biography { get; set; }
        public  string? CurrentAcademicInstitution { get; set; }
        public  string? CurrentEducationLevel { get; set; }
        public  string? MainAreaOfSpecialisation { get; set; }
        public required long CreatedAt { get; set; }
        public long? ModifiedAt { get; set; }

    }
}
