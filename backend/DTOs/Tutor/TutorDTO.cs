using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Tutor
{
    public class TutorDTO
    {
        public Guid TutorId { get; set; }
        public Guid PersonId { get; set; }
    }
}