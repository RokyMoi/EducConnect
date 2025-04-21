using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class GetAllDocumentsByParticipantPersonIdResponse
    {
        public Guid DocumentId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByIdentificationData { get; set; } = string.Empty;

        public int NumberOfParticipants { get; set; }

        public DateTime JoinedAt { get; set; }



    }
}