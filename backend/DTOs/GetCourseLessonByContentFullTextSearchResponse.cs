using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Enums;

namespace EduConnect.DTOs
{
    public class GetCourseLessonByContentFullTextSearchResponse
    {
        public Guid CourseLessonId { get; set; }
        public Guid CourseLessonContentId { get; set; }
        public string Title { get; set; }
        public string Topic { get; set; }
        public string ShortSummary { get; set; }
        public PublishedStatus PublishedStatus { get; set; }

        public int? LessonSequenceOrder { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime? StatusChangedAt { get; set; }

        public string Content { get; set; }



    }
}