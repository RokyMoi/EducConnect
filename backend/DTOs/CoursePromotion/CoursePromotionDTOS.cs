using EduConnect.Entities.Promotion;
using EduConnect.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduConnect.Controllers
{
    public class CoursePromotionDto
    {
        public Guid PromotionId { get; set; }
        public Guid CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PromotionStatus Status { get; set; }
        public long CreatedAt { get; set; }
        public long? UpdatedAt { get; set; }
        public long? StartDate { get; set; }
        public long? EndDate { get; set; }
        public Guid? MainImage { get; set; }
         public ICollection<PromotionImages> Images { get; set; }
        public string? tutorName { get; set; }
    } 

    public class CoursePromotionDetailDto : CoursePromotionDto
    {
        public List<PromotionImageDto> Images { get; set; } = new List<PromotionImageDto>();
    }

    public class PromotionImageDto
    {
        public Guid ImageId { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsMainImage { get; set; }
        public string FileName { get; set; } = string.Empty;
    }

    public class CreatePromotionDto
    {
        [Required]
        public Guid CourseId { get; set; }

        [Required]
        [StringLength(500)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public long StartDate { get; set; }

        [Required]
        public long EndDate { get; set; }

        public List<IFormFile>? Images { get; set; }
    }

    public class UpdatePromotionDto
    {
        [Required]
        public Guid PromotionId { get; set; }

        [Required]
        [StringLength(500)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        public PromotionStatus? Status { get; set; }

        public long? StartDate { get; set; }

        public long? EndDate { get; set; }

        public List<IFormFile>? NewImages { get; set; }

        public Guid? MainImageId { get; set; }

        public List<Guid>? RemoveImageIds { get; set; }
    }

    public class UpdateStatusDto
    {
        [Required]
        public PromotionStatus Status { get; set; }
    }
}