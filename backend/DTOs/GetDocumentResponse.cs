using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class GetDocumentResponse
    {
        public Guid DocumentId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public long Version { get; set; }
    }
}