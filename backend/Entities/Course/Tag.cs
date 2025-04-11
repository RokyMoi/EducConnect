using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Entities.Course
{
    [Table("Tag", Schema = "Course")]
    public class Tag
    {
        [Key]
        public Guid TagId { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public Guid CreatedByPersonId { get; set; }

        [ForeignKey(nameof(CreatedByPersonId))]
        public Person.Person? Person { get; set; } = null;

        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        public long? UpdatedAt { get; set; } = null;


    }
}