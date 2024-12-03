using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Entities.Education
{
    [Table("SpecificExpertiseArea", Schema = "Education")]
    public class SpecificExpertiseArea
    {
        [Key]
        public Guid SpecificExpertiseFieldId { get; set; }
        public Guid GeneralExpertiseFieldId { get; set; }
        //Navigation property
        [ForeignKey(nameof(GeneralExpertiseFieldId))]
        public GeneralExpertiseField GeneralExpertiseField { get; set; }
        public string SpecificExpertiseFieldName { get; set; }
        public string? Description { get; set; } = null;
        public long CreatedAt { get; set; }
        public long? UpdatedAt { get; set; } = null;
    }
}