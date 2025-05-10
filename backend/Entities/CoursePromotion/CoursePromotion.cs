using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using EduConnect.Enums;

namespace EduConnect.Entities.Promotion
{
   
    public class CoursePromotion
    {
        [Key]
        public Guid PromotionId { get; set; } = Guid.NewGuid();

        [ForeignKey("CourseId")]
        public Guid CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        public Course.Course? Course { get; set; } = null;

        [Required]
        [MaxLength(500)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        public PromotionStatus Status { get; set; } = PromotionStatus.Draft;

        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        public long? UpdatedAt { get; set; } = null;

        // Navigation property for the promotion images
        public virtual ICollection<PromotionImages> Images { get; set; } = new List<PromotionImages>();
        public PromotionDuration Duration { get; set; }
    }
}