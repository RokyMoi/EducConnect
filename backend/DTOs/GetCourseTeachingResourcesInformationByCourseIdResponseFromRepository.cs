using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    /*
            INFORMATION:
            - Total number of teaching resources
            - Number of URL
            - Number of files and their total size
            - Two latest added teaching resources
            */
    public class GetCourseTeachingResourcesInformationByCourseIdResponseFromRepository
    {
        public int TotalNumberOfTeachingResources { get; set; }

        public int NumberOfURLs { get; set; }

        public int NumberOfFiles { get; set; }

        public long TotalSizeOfFilesInBytes { get; set; }

        public List<GetCourseTeachingResourceResponse> TwoLatestAddedTeachingResources { get; set; }
    }
}