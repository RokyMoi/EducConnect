using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class CreateOrUpdateCourseTagRequest
    {
        public Guid? TagId { get; set; }

        [Length(1, 32, ErrorMessage = "Name must be between 1 and 32 characters")]
        public string Name { get; set; }




    }
}