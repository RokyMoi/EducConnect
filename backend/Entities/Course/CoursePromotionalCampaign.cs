using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Entities.Course
{
    [Table("CoursePromotionalCampaign", Schema = "Course")]
    public class CoursePromotionalCampaign
    {
        //        CoursePromotionalCampaignId - (uuid) PRIMARY KEY, 

        //CourseId – (uuid) FOREIGN KEY TO „Course“.“Course“ 

        //CampaignName – (string)

        //CampaignStartDateTime – (bigint), UNIX millis

        //CampaignEndDateTime – (bigint), UNIX millis, NULLABLE

        //IsActive – (boolean)

        //CreatedAt – (bigint) – UNIX millis

        //ModifiedAt – (bigint) – UNIX millis – NULLABLE
        [Key]
        public required Guid CoursePromotionalCampaignId { get; set; }
        public required Guid CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }
        public required string CampaignName { get; set; }
        public required long CampaignStartDateTime { get; set; }
        public required long CampaignEndDateTime { get; set; }
        public required bool IsActive { get; set; }
        public required long CreatedAt { get; set; }
        public long? ModifiedAt { get; set; }



    }
}
