using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class GetAllDocumentsByCreatedByPersonIdRepositoryResponse
    {
        public Guid DocumentId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }

        public int NumberOfParticipants { get; set; }
        public int TotalNumberOfInvitedUsers { get; set; }
        public int NumberOfAcceptedInvitations { get; set; }
        public int NumberOfRejectedInvitations { get; set; }
        public int NumberOfPendingInvitations { get; set; }


    }
}