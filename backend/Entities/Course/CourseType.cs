using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Entities.Course
{
    [Table("CourseType", Schema = "Reference")]
    public class CourseType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseTypeId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}