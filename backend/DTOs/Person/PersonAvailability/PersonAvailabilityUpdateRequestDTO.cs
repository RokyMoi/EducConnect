using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Person.PersonAvailability
{
    public class PersonAvailabilityUpdateRequestDTO
    {

        [Required]
        public Guid PersonAvailabilityId { get; set; }

        public int? DayOfWeek { get; set; }

        [RegularExpression(@"(0[0-9]{1}|1[0-9]{1}|2[0-3]{1}){1}:([0-5]{1}[0-9]{1}){1}(\:[0-5]{1}[0-9]{1}){0,1}", ErrorMessage = "Allowed time format is hh:mm:ss or hh:mm")]
        public string? StartTime { get; set; }
        [RegularExpression(@"(0[0-9]{1}|1[0-9]{1}|2[0-3]{1}){1}:([0-5]{1}[0-9]{1}){1}(\:[0-5]{1}[0-9]{1}){0,1}", ErrorMessage = "Allowed time format is hh:mm:ss or hh:mm")]
        public string? EndTime { get; set; }
    }
}