using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Person
{
    public class PersonEducationInformationDeleteRequestDTO
    {


        [Required]
        public Guid PersonEducationInformationId { get; set; }
    }
}