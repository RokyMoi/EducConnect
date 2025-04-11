using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class PasswordHashResult
    {
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
    }
}