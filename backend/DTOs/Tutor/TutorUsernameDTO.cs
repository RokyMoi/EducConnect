using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Tutor
{
    public class TutorUsernameDTO
    {
        public Guid PersonId { get; set; }
        public Guid TutorId { get; set; }
        public Guid PersonDetailsId { get; set; }

        public string Username { get; set; }
    }
}