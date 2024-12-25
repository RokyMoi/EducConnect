using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Reference.WorkType
{
    public class WorkTypeResponseDTO
    {
        public int WorkTypeId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = string.Empty;
    }
}