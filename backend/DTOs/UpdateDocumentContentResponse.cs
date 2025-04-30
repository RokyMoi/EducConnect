using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class UpdateDocumentContentResponse
    {
        public Guid DocumentId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string UpdatedByIdentificationData { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }
        public long Version { get; set; }
    }
}