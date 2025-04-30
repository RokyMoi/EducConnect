using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class GetAllInvitationsSentByPersonIdResponse
    {
        public Guid CollaborationDocumentInvitationId { get; set; }
        public Guid DocumentId { get; set; }

        public string Title { get; set; }

        public DateTime DocumentCreatedAt { get; set; }

        public string InvitationSentToPersonIdentificationData { get; set; } = string.Empty;

        public bool? InvitationStatus { get; set; } = null;

        public DateTime? InvitationStatusChangedAt { get; set; } = null;
        public DateTime InvitationSentAt { get; set; }
    }
}