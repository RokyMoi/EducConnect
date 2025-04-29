using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class UpdateDocumentContentRequest
    {
        public Guid DocumentId { get; set; }
        public long ClientVersion { get; set; }
        public List<DocumentDelta> Deltas { get; set; } = [];
    }
}