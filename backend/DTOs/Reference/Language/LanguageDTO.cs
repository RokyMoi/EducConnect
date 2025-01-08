using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Reference.Language
{
    public class LanguageDTO
    {
        public Guid LanguageId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsRightToLeft { get; set; }


    }
}