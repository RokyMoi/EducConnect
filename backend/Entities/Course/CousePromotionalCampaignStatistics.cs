using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Entities.Course
{
//    CousePromotionalCampaignStatisticsId – (uuid) – PRIMARY KEY

//CoursePromotionalCampaignId - (uuid) FOREIGN KEY TO „Course“.“CoursePromotionalCampaign“, 

//CourseId – (uuid) FOREIGN KEY TO „Course“.“Course“ 

//NumberOfIndividualUsersWhoSawThePost – (int)

//NumberOfIndividualUsersWhoClickedOnThePost – (int)

//NumberOfIndividualUsersWhoSubscribedFromThePost – (int)

//PostGeneratedValue – (money)
[Table("CousePromotionalCampaignStatistics", Schema = "Course")]
    public class CousePromotionalCampaignStatistics
    {
        [Key]
        public required Guid CousePromotionalCampaignStatisticsId { get; set; }
        
        public required Guid CoursePromotionalCampaignId { get; set; }
        [ForeignKey(nameof(CoursePromotionalCampaignId))]
        public CoursePromotionalCampaign CoursePromotionalCampaign {  get; set; }
        public required Guid CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }
        public required int NumberOfIndividualUsersWhoSawThePost {  get; set; }
        public required int NumberOfIndividualUsersWhoClickedOnThePost { get; set; }
        public required int NumberOfIndividualUsersWhoSubscribedFromThePost {  get; set; }
        public required decimal PostGeneratedValue {  get; set; }


    }
}
