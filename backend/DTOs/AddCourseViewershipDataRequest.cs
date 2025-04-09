using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Enums;

namespace EduConnect.DTOs
{
    public class AddCourseViewershipDataRequest
    {

        public Guid CourseId { get; set; }
        public UserCourseSourceType UserCameFrom { get; set; } = UserCourseSourceType.Search;
        public DateTime ClickedOn { get; set; } = DateTime.Now;
    }
}