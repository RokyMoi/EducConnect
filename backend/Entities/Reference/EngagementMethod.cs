using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Entities.Reference
{
    [Table("EngagementMethod", Schema = "Reference")]
    public class EngagementMethod
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EngagementMethodId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}