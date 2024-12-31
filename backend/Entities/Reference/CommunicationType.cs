using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Entities.Reference
{
    [Table("CommunicationType", Schema = "Reference")]
    public class CommunicationType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommunicationTypeId { get; set; }
        public string Name { get; set; }
    }
}