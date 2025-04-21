using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Entities.CollaborationDocument
{
    [Table("Document", Schema = "Document")]
    public class Document
    {
        public Guid DocumentId { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Content { get; set; } = string.Empty;

        public Guid CreatedByPersonId { get; set; }

        [ForeignKey(nameof(CreatedByPersonId))]
        public Person.Person? Person { get; set; } = null;
        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        public long? UpdatedAt { get; set; } = null;

        public List<CollaborationDocumentActiveUser> CollaborationDocumentActiveUsers { get; set; } = new List<CollaborationDocumentActiveUser>();

        public List<CollaborationDocumentInvitation> CollaborationDocumentInvitations { get; set; } = new List<CollaborationDocumentInvitation>();

        public List<CollaborationDocumentParticipant> CollaborationDocumentParticipants { get; set; } = new List<CollaborationDocumentParticipant>();

    }
}