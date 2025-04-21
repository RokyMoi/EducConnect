using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class InviteUserToDocumentRequest
    {
        public Guid DocumentId { get; set; }
        public Guid InvitedPersonId { get; set; }

    }
}