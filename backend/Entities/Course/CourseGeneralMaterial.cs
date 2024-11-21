using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Entities.Course
{
    [Table("CourseGeneralMaterial", Schema = "Course")]
    public class CourseGeneralMaterial
    {
        //        CourseGeneralMaterialId - (uuid) PRIMARY KEY

        //CourseId – (uuid), FOREIGN KEY TO „Course“.“Course“ 

        //GeneralMaterialTitle – (string) NULLABLE

        //GeneralMaterialDescription – (string) (max: 256) 

        //FileType – (string)
        //        LinkToMaterial – (string)
        //        IsRequired. – (boolean)
        //        IsRecommended – (boolean)

        //CreatedAt – (bigint) – UNIX millis

        //ModifiedAt – (bigint) – UNIX millis – NULLABLE
        [Key]
        public required Guid CourseGeneralMaterialId { get; set; }
        public required Guid CourseId { get; set; }

        public string? GeneralMaterialTitle { get; set; }
        public required string GeneralMaterialDescription { get; set; }
        public required string FileType { get; set; }
        public required string LinkToMaterial { get; set; }
        public required bool IsRequired { get; set; }
        public required bool IsRecommended { get; set; }
        public required long CreatedAt { get; set; }
        public long? ModifiedAt { get; set; }





    }
}
