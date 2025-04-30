using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class SearchUsersToInviteRequest
    {
        public Guid DocumentId { get; set; }
        public string? SearchQuery { get; set; }
    }
}