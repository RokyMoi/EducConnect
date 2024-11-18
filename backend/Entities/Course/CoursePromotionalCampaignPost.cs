using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Entities.Course
{
//    CoursePromotionalCampaignPostId - PRIMARY KEY

//CoursePromotionalCampaignId - (uuid) FOREIGN KEY TO „Course“.“CoursePromotionalCampaign“, 

//CourseId – (uuid) FOREIGN KEY TO „Course“.“Course“ 

//PostTitle – (string)

//PostDescription – (string) NULLABLE(default „Course“.“ ShortDescription“)

//CreatedAt – (bigint) – UNIX millis

//ModifiedAt – (bigint) – UNIX millis – NULLABLE
    public class CoursePromotionalCampaignPost
    {
        [Key]
        public required Guid CoursePromotionalCampaignPostId { get; set; }
        public required Guid CoursePromotionalCampaignId { get; set; }
        [ForeignKey(nameof(CoursePromotionalCampaignId))]
        public CoursePromotionalCampaign CoursePromotionalCampaign{ get; set; }
       
        public required string PostTitle { get; set; }
        public required string PostDescription { get; set; }
        public required long CreatedAt { get; set; }
        public long? ModifiedAt { get; set; }
    }
}
