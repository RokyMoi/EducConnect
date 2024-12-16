using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Entities.Person
{

    [Table("PersonAvailability", Schema = "Person")]
    public class PersonAvailability
    {
        public Guid PersonAvailabilityId { get; set; }
        public Guid PersonId { get; set; }

        //Navigation property
        [ForeignKey(nameof(PersonId))]
        public EduConnect.Entities.Person.Person Person { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public long CreatedAt { get; set; }
        public long? UpdatedAt { get; set; }
    }
}