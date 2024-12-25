using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Reference.EmploymentType
{
    public class EmploymentTypeResponseDTO
    {

        public int EmploymentTypeId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = string.Empty;
    }
}