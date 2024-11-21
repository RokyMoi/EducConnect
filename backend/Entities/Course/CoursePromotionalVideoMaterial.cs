using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace EduConnect.Entities.Course
//    CoursePromotionalVideoMaterialId– (uuid)

//CoursePromotionalVideoMaterialId- (uuid) FOREIGN KEY TO „Course“.“CoursePromotionalCampaign“, 

//CourseId – (uuid) FOREIGN KEY TO „Course“.“Course“ 

//Video – (blob)

//VideoName – (string), NULLABLE

//LengthInSeconds – (int)

//HorizontalResolution – (int)

//VerticalResolution – (int)

//CreatedAt – (bigint) – UNIX millis

//ModifiedAt – (bigint) – UNIX millis – NULLABLE 
{
    [Table("CoursePromotionalVideoMaterial", Schema = "Course")]
    public class CoursePromotionalVideoMaterial
    {
        [Key]
        public required Guid CoursePromotionalVideoMaterialId { get; set; }
        public required Guid CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }
        public required byte[] Video { get; set; }
        public required int LenghtInSeconds { get; set; }
        public required int HorizontalResolution { get; set; }
        public required int VerticalResolution { get; set; }
        public required long CreatedAt { get; set; }
        public long? ModifiedAt { get; set; }




    }
}
