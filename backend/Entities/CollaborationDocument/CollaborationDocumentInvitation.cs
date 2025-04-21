using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Entities.CollaborationDocument
{
    [Table("CollaborationDocumentInvitation", Schema = "Document")]
    public class CollaborationDocumentInvitation
    {
        [Key]
        public Guid CollaborationDocumentInvitationId { get; set; } = Guid.NewGuid();
        public Guid DocumentId { get; set; }

        [ForeignKey(nameof(DocumentId))]
        public Document? Document { get; set; } = null;

        public Guid? InvitedPersonId { get; set; }

        [ForeignKey(nameof(InvitedPersonId))]
        public Person.Person? InvitedPerson { get; set; } = null;

        public Guid? InvitedByPersonId { get; set; }

        [ForeignKey(nameof(InvitedByPersonId))]
        public Person.Person? InvitedByPerson { get; set; } = null;

        /*Flag to indicate if the invitation is accepted or not
        VALUES:
        - true: Invitation is accepted
        - false: Invitation is rejected
        - null: Invitation is pending
        */

        public bool? Status { get; set; } = null;

        public DateTime? StatusChangedAt { get; set; } = null;

        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        public long? UpdatedAt { get; set; } = null;

    }
}