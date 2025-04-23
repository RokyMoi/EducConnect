using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class SearchUsersToInviteResponse
    {
        public Guid PersonId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string Username { get; set; }


    }
}