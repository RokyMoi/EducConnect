using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Person
{
    public class PersonCareerInformationDeleteRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public Guid PersonCareerInformationId { get; set; }
    }
}