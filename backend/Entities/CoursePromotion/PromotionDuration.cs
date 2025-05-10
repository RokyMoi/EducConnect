
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Entities.Promotion
{
   
    public class PromotionDuration
    {
        [Key]
        public Guid DurationId { get; set; } = Guid.NewGuid();

        [ForeignKey("PromotionId")]
        public Guid PromotionId { get; set; }

        [ForeignKey(nameof(PromotionId))]
        public CoursePromotion? Promotion { get; set; } = null;

        [Required]
        public long StartDate { get; set; } // Unix timestamp

        [Required]
        public long EndDate { get; set; } // Unix timestamp
    }
}