using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class GetDashboardPersonInfoResponse
    {
        public string Email { get; set; }
        public string? Username { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
    }
}