using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace backend.Entities.Reference.Language
{
    [Table("Language", Schema = "Reference")]
    public class Language
    {
        public Guid LanguageId { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }

        public bool IsRightToLeft { get; set; }

        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        public long? UpdatedAt { get; set; } = null;


    }
}