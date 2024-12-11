using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Entities.Reference
{
    [Table("TutorTeachingStyleType", Schema = "Reference")]
    public class TutorTeachingStyleType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TutorTeachingStyleTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}