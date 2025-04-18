using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class CheckUserRoleRequest
    {
        public required string RequiredRole { get; set; }
    }
}