using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Entities.Education
{
    [Table("GeneralExpertiseField", Schema = "Education")]
    public class GeneralExpertiseField
    {
        [Key]
        public Guid ExpertiseGeneralFieldId { get; set; }
        public string ExpertiseGeneralFieldName { get; set; }
        public string ExpertiseGeneralFieldDescription { get; set; }
        public long CreatedAt { get; set; }
        public long? ModifiedAt { get; set; }
    }
}