using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Reference.IndustryClassification
{
    public class IndustryClassificationResponseDTO
    {
        public Guid IndustryClassificationId { get; set; }
        public string Industry { get; set; }
        public string Sector { get; set; }

    }
}