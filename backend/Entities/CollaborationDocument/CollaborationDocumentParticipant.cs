using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Entities.CollaborationDocument
{
    [Table("CollaborationDocumentParticipant", Schema = "Document")]
    public class CollaborationDocumentParticipant
    {
        [Key]
        public Guid CollaborationDocumentParticipantId { get; set; } = Guid.NewGuid();
        public Guid DocumentId { get; set; }

        [ForeignKey(nameof(DocumentId))]
        public Document? Document { get; set; } = null;

        public Guid? ParticipantPersonId { get; set; }

        [ForeignKey(nameof(ParticipantPersonId))]
        public Person.Person? ParticipantPerson { get; set; } = null;

        public Guid CollaborationDocumentInvitationId { get; set; }
        [ForeignKey(nameof(CollaborationDocumentInvitationId))]
        public CollaborationDocumentInvitation? CollaborationDocumentInvitation { get; set; } = null;

        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        public long? UpdatedAt { get; set; } = null;


    }
}