using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Entities.Tutor
{
    [Table("TutorRegistrationStatus", Schema = "Reference")]
    public class TutorRegistrationStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TutorRegistrationStatusId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsSkippable { get; set; } = false;
        public long CreatedAt { get; set; }

        public long? UpdatedAt { get; set; }
    }
}