using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class CourseManagementDashboardResponse
    {
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public string DifficultyLevel { get; set; }
        public string Category { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsThumbnailAdded { get; set; }
        public DateTime? ThumbnailAddedOn { get; set; }
        public bool IsUsingAzureStorage { get; set; }

        public int TotalNumberOfTeachingResources { get; set; } = 0;

        public int NumberOfURLs { get; set; } = 0;

        public int NumberOfFiles { get; set; } = 0;

        public long TotalSizeOfFilesInBytes { get; set; } = 0;

        public int NumberOfLessons { get; set; } = 0;
        public int NumberOfDraftLessons { get; set; } = 0;
        public int NumberOfPublishedLessons { get; set; } = 0;
        public int NumberOfArchivedLessons { get; set; } = 0;

        public List<GetCourseTeachingResourceResponse> TwoLatestAddedTeachingResources { get; set; } = new List<GetCourseTeachingResourceResponse>();

        public List<GetCourseLessonByIdResponse> TwoLatestAddedLessons { get; set; } = new List<GetCourseLessonByIdResponse>();

        public int NumberOfPromotionImages { get; set; } = 0;
        public DateTime? LatestPromotionImageUploadedAt { get; set; }



    }
}