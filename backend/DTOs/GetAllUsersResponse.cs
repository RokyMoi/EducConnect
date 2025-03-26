using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class GetAllUsersResponse
    {
        public Guid PersonId { get; set; }
        public Guid PersonPublicId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}