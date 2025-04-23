using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Entities.CollaborationDocument
{
    [Table("CollaborationDocumentActiveUser", Schema = "Document")]
    public class CollaborationDocumentActiveUser
    {
        [Key]
        public Guid CollaborationDocumentActiveUserId { get; set; } = Guid.NewGuid();
        public Guid DocumentId { get; set; }
        [ForeignKey(nameof(DocumentId))]
        public Document? Document { get; set; } = null;

        public Guid? ActiveUserPersonId { get; set; }

        [ForeignKey(nameof(ActiveUserPersonId))]
        public Person.Person? Person { get; set; } = null;

        /*
        Flag to indicate if the user is currently part or not
        VALUES:
        - true: User is currently collaborating on the document
        - false: User is not currently collaborating on the document
        */
        public bool Status { get; set; } = false;

        public DateTime StatusChangedAt { get; set; } = DateTime.UtcNow;

        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        public long? UpdatedAt { get; set; } = null;
    }
}