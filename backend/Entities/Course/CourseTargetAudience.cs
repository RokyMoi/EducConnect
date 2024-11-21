using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Entities.Course
{

    [Table("CourseTargetAudience", Schema = "Course")]
    public class CourseTargetAudience
    {
        [Key]
        public required Guid CourseTargetAudienceId { get; set; }
        public required Guid CoursePromotionalCampaignId { get; set; }
        [ForeignKey(nameof(CoursePromotionalCampaignId))]
        public CoursePromotionalCampaign CoursePromotionalCampaign { get; set; }
        public required Guid CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }
        public bool? TargetAudinceGender { get; set; }
        public int? TargetAudienceMinimumAge { get; set; }
        public int? TargetAudienceMaximumAge {  get; set; }

        //public Guid TargetAudiencePrimaryInterestId { get; set; }

        //public Guid TargetAudienceSecondaryInterest {get;set;}
        //public Guid TargetAudienceMinEducationLevel {get;set}
       // public Guid TargetAudienceMaxEducationLevel { get; set; }






    }
}
