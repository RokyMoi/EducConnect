using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities.Reference.Language;

namespace backend.Entities.Course
{
    [Table("CourseLanguage", Schema = "Course")]
    public class CourseLanguage
    {
        [Key]
        public Guid CourseId { get; set; }

        //Navigation property
        public EduConnect.Entities.Course.Course? Course { get; set; }

        public Guid LanguageId { get; set; }

        //Navigation property 
        public Language? Language { get; set; }
    }
}