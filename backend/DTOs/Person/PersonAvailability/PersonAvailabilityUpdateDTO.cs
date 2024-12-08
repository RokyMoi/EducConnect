using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Person.PersonAvailability
{
    public class PersonAvailabilityUpdateDTO
    {
        public Guid PersonAvailabilityId { get; set; }
        public int? DayOfWeek { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}