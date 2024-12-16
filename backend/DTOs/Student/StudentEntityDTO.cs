using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Student
{
    public class StudentEntityDTO
    {
        public Guid PersonId { get; set; }
        public Guid StudentId { get; set; }
    }
}