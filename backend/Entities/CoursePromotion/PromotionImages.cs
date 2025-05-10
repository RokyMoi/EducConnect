using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Entities.Promotion {
 
    public class PromotionImages
    {
        [Key]
        public Guid ImageId { get; set; } = Guid.NewGuid();

        [ForeignKey("PromotionId")]
        public Guid PromotionId { get; set; }

        [ForeignKey(nameof(PromotionId))]
        public CoursePromotion? Promotion { get; set; } = null;

        [Required]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public string ContentType { get; set; } = string.Empty;

        [Required]
        public byte[] ImageData { get; set; } = Array.Empty<byte>();

        // Order number for displaying images
        public int DisplayOrder { get; set; } = 0;

        // Flag to indicate if this is the main image
        public bool IsMainImage { get; set; } = false;

        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}