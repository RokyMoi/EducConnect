using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class GetCourseLessonsCountFilteredByPublishedStatusRepositoryResponse
    {
        public int TotalNumberOfLessons { get; set; }
        public int NumberOfPublishedLessons { get; set; }
        public int NumberOfDraftLessons { get; set; }
        public int NumberOfArchivedLessons { get; set; }
    }
}