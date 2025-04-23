using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class GetAllActiveCollaboratorsByDocumentId
    {
        public Guid PersonId { get; set; }
        public string IdentificationData { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; }
    }
}