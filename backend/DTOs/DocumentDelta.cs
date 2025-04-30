using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class DocumentDelta
    {
        public int Position { get; set; }
        public string? Insert { get; set; }
        public int? Delete { get; set; }
    }
}