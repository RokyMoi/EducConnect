using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Enums;

namespace EduConnect.DTOs
{
    public class GetCoursesByQueryResponse
    {
        public Guid CourseId { get; set; }
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Guid CourseCategoryId { get; set; }

        public string CourseCategoryName { get; set; } = string.Empty;

        public int LearningDifficultyLevelId { get; set; }

        public string LearningDifficultyLevelName { get; set; } = string.Empty;

        public decimal Price { get; set; } = 0;

        public int? MinNumberOfStudents { get; set; } = null;

        public int? MaxNumberOfStudents { get; set; } = null;

        public int NumberOfLessons { get; set; } = 0;

        public PublishedStatus PublishedStatus { get; set; } = PublishedStatus.Draft;

        public DateTime CreatedAt { get; set; }

        public bool HasThumbnail { get; set; } = false;

        public string? ThumbnailUrl { get; set; } = null;

        public int NumberOfStudents { get; set; } = 0;

        public string TutorUsername { get; set; } = string.Empty;
        public string TutorEmail { get; set; } = string.Empty;


    }
}