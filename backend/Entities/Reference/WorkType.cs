using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Entities.Reference
{
    [Table("WorkType", Schema = "Reference")]
    public class WorkType
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkTypeId { get; set; }
        [Required]
        public required string Name { get; set; }
        public string? Description { get; set; } = string.Empty;
        [Required]
        public required long CreatedAt { get; set; }
        public long? UpdatedAt { get; set; } = null;
    }
}