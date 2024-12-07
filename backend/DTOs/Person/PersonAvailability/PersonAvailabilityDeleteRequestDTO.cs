using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Person.PersonAvailability
{
    public class PersonAvailabilityDeleteRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public Guid PersonAvailabilityId { get; set; }

    }
}