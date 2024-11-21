using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Metadata;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Entities.Course
{
    [Table("CoursePromotionalPictureMaterial", Schema = "Course")]
    public class CoursePromotionalPictureMaterial
    {
        //        CoursePromotionalPictureMaterialId – (uuid)

        //CoursePromotionalCampaignId - (uuid) FOREIGN KEY TO „Course“.“CoursePromotionalCampaign“, 

        //CourseId – (uuid) FOREIGN KEY TO „Course“.“Course“ 

        //PromotionalPicture – (blob)

        //PictureName – (string), NULLABLE

        //HorizontalResolution – (int)

        //VerticalResolution – (int)

        //CreatedAt – (bigint) – UNIX millis

        //ModifiedAt – (bigint) – UNIX millis – NULLABLE
        [Key]
        public required Guid CoursePromotionalPictureMaterialId { get; set; }
        public required Guid CoursePromotionalCampaignId { get; set; }
        [ForeignKey(nameof(CoursePromotionalCampaignId))]
        public CoursePromotionalCampaign CoursePromotionalCampaign { get; set; }
        public required Guid CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public  Course Course { get; set; }
        public required byte[] PromotionalPicture { get; set; }
        public required string? PictureName { get; set; }
        public required int HorizontalResolution {  get; set; }
        public required int VerticalResolution { get; set; }
        public  required long CreatedAt { get; set; }
        public long? ModifiedAt { get; set; }

    }
}
