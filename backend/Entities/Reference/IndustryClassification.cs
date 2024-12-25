using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Entities.Reference
{
    [Table("IndustryClassification", Schema = "Reference")]
    public class IndustryClassification
    {
        public Guid IndustryClassificationId { get; set; }

        public string Industry { get; set; }

        public string Sector { get; set; }

        public long CreatedAt { get; set; } = DateTimeOffset.Now.ToUnixTimeMilliseconds();

        public long? UpdatedAt { get; set; } = null;
    }
}