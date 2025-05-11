using backend.Middleware;
using EduConnect.Data;
using EduConnect.Entities.Promotion;
using EduConnect.Enums;
using EduConnect.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromotionsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<PromotionsController> _logger;

        public PromotionsController(DataContext context, IWebHostEnvironment environment, ILogger<PromotionsController> logger)
        {
            _context = context;
            _environment = environment;
            _logger = logger;
        }

        // GET: api/Promotions
        [HttpGet]
        [Route("/AllPromotions")]
        public async Task<ActionResult<IEnumerable<CoursePromotionDto>>> GetAllPromotions()
        {
            try
            {
                var promotions = await _context.CoursePromotion
                    .Include(p => p.Course)
                    .ThenInclude(pt => pt.Tutor)
                    .ThenInclude(pd => pd.Person)
                    .ThenInclude(pdd => pdd.PersonDetails)
                    .Include(p => p.Duration)
                    .Include(p => p.Images)
                    .OrderByDescending(p => p.CreatedAt)
                    .ToListAsync();

                return Ok(promotions.Select(p => new CoursePromotionDto
                {
                    tutorName = p.Course.Tutor.Person.PersonDetails.FirstName + " " + p.Course.Tutor.Person.PersonDetails.LastName,
                    PromotionId = p.PromotionId,
                    CourseId = p.CourseId,
                    CourseName = p.Course?.Title ?? "Unknown Course",
                    Title = p.Title,
                    Description = p.Description,
                    Status = p.Status,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    StartDate = p.Duration?.StartDate,
                    EndDate = p.Duration?.EndDate,
                    MainImage = p.Images.FirstOrDefault(i => i.IsMainImage)?.ImageId,
                    Images = p.Images
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all promotions");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [CheckPersonLoginSignup]
        public async Task<ActionResult<IEnumerable<CoursePromotionDto>>> GetPromotions()
        {
            try
            {
                var access = new Caller(this.HttpContext);
                var email = access.Email;

                // First check if the email exists
                var personEmail = await _context.PersonEmail
                    .FirstOrDefaultAsync(x => x.Email == email);

                if (personEmail == null)
                    return Unauthorized("Email not found.");

                // Check if the person is a tutor
                var tutor = await _context.Tutor
                    .FirstOrDefaultAsync(x => x.PersonId == personEmail.PersonId);

                if (tutor == null)
                    return Unauthorized("User is not a tutor.");

                // Get only promotions belonging to this tutor's courses
                var promotions = await _context.CoursePromotion
                    .Include(p => p.Course)
                    .Include(p => p.Duration)
                    .Include(p => p.Images)
                    .Where(p => p.Course.TutorId == tutor.TutorId)
                    .OrderByDescending(p => p.CreatedAt)
                    .ToListAsync();

                // Map to DTOs separately to properly handle the image ID
                var promotionDtos = promotions.Select(p => new CoursePromotionDto
                {
                    PromotionId = p.PromotionId,
                    CourseId = p.CourseId,
                    CourseName = p.Course?.Title ?? "Unknown Course",
                    Title = p.Title,
                    Description = p.Description,
                    Status = p.Status,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    StartDate = p.Duration?.StartDate,
                    EndDate = p.Duration?.EndDate,
                    MainImage = p.Images.FirstOrDefault(i => i.IsMainImage)?.ImageId,
                    Images = p.Images
                }).ToList();

                return Ok(promotionDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting promotions for tutor");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Promotions/5
        [HttpGet("{id}")]
        [CheckPersonLoginSignup]
        public async Task<ActionResult<CoursePromotionDetailDto>> GetPromotion(Guid id)
        {
            try
            {
                var promotion = await _context.CoursePromotion
                    .Include(p => p.Course)
                    .Include(p => p.Duration)
                    .Include(p => p.Images)
                    .FirstOrDefaultAsync(p => p.PromotionId == id);

                if (promotion == null)
                {
                    return NotFound("Promotion not found");
                }

                return Ok(new CoursePromotionDetailDto
                {
                    PromotionId = promotion.PromotionId,
                    CourseId = promotion.CourseId,
                    CourseName = promotion.Course?.Title ?? "Unknown Course",
                    Title = promotion.Title,
                    Description = promotion.Description,
                    Status = promotion.Status,
                    CreatedAt = promotion.CreatedAt,
                    UpdatedAt = promotion.UpdatedAt,
                    StartDate = promotion.Duration?.StartDate,
                    EndDate = promotion.Duration?.EndDate,
                    Images = promotion.Images.Select(i => new PromotionImageDto
                    {
                        ImageId = i.ImageId,
                        DisplayOrder = i.DisplayOrder,
                        IsMainImage = i.IsMainImage,
                        FileName = i.FileName
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting promotion with ID {PromotionId}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Promotions
        [HttpPost]
        [CheckPersonLoginSignup]
        public async Task<ActionResult<CoursePromotion>> CreatePromotion([FromForm] CreatePromotionDto promotionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    // Check if course exists
                    var course = await _context.Course.FindAsync(promotionDto.CourseId);
                    if (course == null)
                    {
                        return BadRequest("Invalid course ID.");
                    }

                    // Create promotion
                    var promotion = new CoursePromotion
                    {
                        CourseId = promotionDto.CourseId,
                        Title = promotionDto.Title,
                        Description = promotionDto.Description,
                        Status = PromotionStatus.Draft,
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                    };

                    // Create duration
                    var duration = new PromotionDuration
                    {
                        PromotionId = promotion.PromotionId,
                        StartDate = promotionDto.StartDate,
                        EndDate = promotionDto.EndDate
                    };

                    promotion.Duration = duration;

                    // Process images
                    if (promotionDto.Images != null && promotionDto.Images.Count > 0)
                    {
                        int order = 0;
                        foreach (var image in promotionDto.Images)
                        {
                            if (image.Length > 0)
                            {
                                using var memoryStream = new MemoryStream();
                                await image.CopyToAsync(memoryStream);

                                promotion.Images.Add(new PromotionImages
                                {
                                    PromotionId = promotion.PromotionId,
                                    FileName = image.FileName,
                                    ContentType = image.ContentType,
                                    ImageData = memoryStream.ToArray(),
                                    DisplayOrder = order,
                                    IsMainImage = order == 0, // First image is main by default
                                    CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                                });
                                order++;
                            }
                        }
                    }

                    _context.CoursePromotion.Add(promotion);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return CreatedAtAction(nameof(GetPromotion), new { id = promotion.PromotionId },
                        new { promotion.PromotionId, Message = "Promotion created successfully" });
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating promotion");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Promotions/5
        [HttpPut("{id}")]
        [CheckPersonLoginSignup]
        public async Task<IActionResult> UpdatePromotion(Guid id, [FromForm] UpdatePromotionDto promotionDto)
        {
            if (id != promotionDto.PromotionId)
            {
                return BadRequest("ID mismatch");
            }

            try
            {
                // First check if promotion exists without tracking
                var promotionExists = await _context.CoursePromotion
                    .AsNoTracking()
                    .AnyAsync(p => p.PromotionId == id);

                if (!promotionExists)
                {
                    return NotFound("Promotion not found");
                }

                // Determine if we're just adding images without changing other data
                bool isImageOnlyUpdate =
                    (promotionDto.NewImages != null && promotionDto.NewImages.Count > 0) &&
                    !promotionDto.MainImageId.HasValue &&
                    (promotionDto.RemoveImageIds == null || promotionDto.RemoveImageIds.Count == 0);

                if (isImageOnlyUpdate)
                {
                    // Special handling for just adding images - less likely to cause concurrency issues
                    return await AddImagesToPromotion(id, promotionDto);
                }
                else
                {
                    // Full update with transaction
                    using var transaction = await _context.Database.BeginTransactionAsync();

                    try
                    {
                        // Fetch fresh entity with explicit loading of related entities to minimize tracking issues
                        var promotion = await _context.CoursePromotion
                            .Include(p => p.Duration)
                            .Include(p => p.Images)
                            .FirstOrDefaultAsync(p => p.PromotionId == id);

                        if (promotion == null)
                        {
                            return NotFound("Promotion not found");
                        }

                        // Update promotion details
                        promotion.Title = promotionDto.Title;
                        promotion.Description = promotionDto.Description;
                        promotion.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                        if (promotionDto.Status.HasValue)
                        {
                            promotion.Status = promotionDto.Status.Value;
                        }

                        // Update duration if provided
                        if (promotionDto.StartDate.HasValue && promotionDto.EndDate.HasValue)
                        {
                            if (promotion.Duration == null)
                            {
                                promotion.Duration = new PromotionDuration
                                {
                                    PromotionId = promotion.PromotionId,
                                    StartDate = promotionDto.StartDate.Value,
                                    EndDate = promotionDto.EndDate.Value
                                };
                            }
                            else
                            {
                                promotion.Duration.StartDate = promotionDto.StartDate.Value;
                                promotion.Duration.EndDate = promotionDto.EndDate.Value;
                            }
                        }

                        // Process new images if provided
                        if (promotionDto.NewImages != null && promotionDto.NewImages.Count > 0)
                        {
                            int order = promotion.Images.Count > 0 ? promotion.Images.Max(i => i.DisplayOrder) + 1 : 0;

                            foreach (var image in promotionDto.NewImages)
                            {
                                if (image.Length > 0)
                                {
                                    using var memoryStream = new MemoryStream();
                                    await image.CopyToAsync(memoryStream);

                                    var newImage = new PromotionImages
                                    {
                                        PromotionId = promotion.PromotionId,
                                        FileName = image.FileName,
                                        ContentType = image.ContentType,
                                        ImageData = memoryStream.ToArray(),
                                        DisplayOrder = order,
                                        IsMainImage = promotion.Images.Count == 0, // First image is main by default
                                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                                    };
                                    promotion.Images.Add(newImage);
                                    order++;
                                }
                            }
                        }

                        // Update main image if specified
                        if (promotionDto.MainImageId.HasValue)
                        {
                            var images = promotion.Images.ToList();
                            foreach (var image in images)
                            {
                                image.IsMainImage = image.ImageId == promotionDto.MainImageId.Value;
                            }
                        }

                        // Remove images if specified
                        if (promotionDto.RemoveImageIds != null && promotionDto.RemoveImageIds.Count > 0)
                        {
                            var imagesToRemove = promotion.Images.Where(i => promotionDto.RemoveImageIds.Contains(i.ImageId)).ToList();
                            foreach (var image in imagesToRemove)
                            {
                                promotion.Images.Remove(image);
                                _context.PromotionImages.Remove(image);
                            }
                        }

                        // Apply the changes and save
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        return NoContent();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        await transaction.RollbackAsync();
                        _logger.LogWarning(ex, "Concurrency conflict updating promotion {PromotionId}", id);

                        // Check if the entity still exists
                        var stillExists = await _context.CoursePromotion.AnyAsync(p => p.PromotionId == id);
                        if (!stillExists)
                        {
                            return NotFound("Promotion has been deleted by another user");
                        }

                        // Return a specific response for concurrency conflicts
                        return StatusCode(409, "The promotion was modified by another user. Please refresh and try again.");
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating promotion with ID {PromotionId}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Helper method to just add images to a promotion - reduces concurrency issues
        private async Task<IActionResult> AddImagesToPromotion(Guid id, UpdatePromotionDto promotionDto)
        {
            try
            {
                // Use a separate context to avoid tracking issues
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    // Just check that the promotion exists - don't load full entity with all relations
                    var promotion = await _context.CoursePromotion.FindAsync(id);
                    if (promotion == null)
                    {
                        return NotFound("Promotion not found");
                    }

                    // Get the current highest display order (if any images exist)
                    int currentMaxOrder = await _context.PromotionImages
                        .Where(i => i.PromotionId == id)
                        .Select(i => i.DisplayOrder)
                        .DefaultIfEmpty(-1)
                        .MaxAsync();

                    int order = currentMaxOrder + 1;

                    // Process new images one by one
                    if (promotionDto.NewImages != null)
                    {
                        foreach (var image in promotionDto.NewImages)
                        {
                            if (image.Length > 0)
                            {
                                using var memoryStream = new MemoryStream();
                                await image.CopyToAsync(memoryStream);

                                var newImage = new PromotionImages
                                {
                                    PromotionId = id,
                                    FileName = image.FileName,
                                    ContentType = image.ContentType,
                                    ImageData = memoryStream.ToArray(),
                                    DisplayOrder = order,
                                    IsMainImage = false, // Don't make it main by default when adding to existing
                                    CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                                };

                                _context.PromotionImages.Add(newImage);
                                order++;
                            }
                        }
                    }

                    // Update the promotion's UpdatedAt timestamp
                    promotion.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return NoContent();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogWarning(ex, "Concurrency conflict adding images to promotion {PromotionId}", id);
                    return StatusCode(409, "Could not add images. The promotion was modified by another user. Please refresh and try again.");
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding images to promotion {PromotionId}", id);
                return StatusCode(500, $"Internal server error while adding images: {ex.Message}");
            }
        }

        // DELETE: api/Promotions/5
        [HttpDelete("{id}")]
        [CheckPersonLoginSignup]
        public async Task<IActionResult> DeletePromotion(Guid id)
        {
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    var promotion = await _context.CoursePromotion
                        .Include(p => p.Duration)
                        .Include(p => p.Images)
                        .FirstOrDefaultAsync(p => p.PromotionId == id);

                    if (promotion == null)
                    {
                        return NotFound("Promotion not found");
                    }

                    // Remove related duration and images
                    if (promotion.Duration != null)
                    {
                        _context.PromotionDuration.Remove(promotion.Duration);
                    }

                    foreach (var image in promotion.Images.ToList())
                    {
                        _context.PromotionImages.Remove(image);
                    }

                    _context.CoursePromotion.Remove(promotion);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return NoContent();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogWarning(ex, "Concurrency conflict deleting promotion {PromotionId}", id);

                    // Check if the entity still exists
                    var stillExists = await _context.CoursePromotion.AnyAsync(p => p.PromotionId == id);
                    if (!stillExists)
                    {
                        // It's already gone, so that's what the client wanted
                        return NoContent();
                    }

                    return StatusCode(409, "The promotion was modified by another user. Please refresh and try again.");
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting promotion with ID {PromotionId}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Promotions/Image/5
        [HttpGet("Image/{id}")]
        public async Task<ActionResult> GetImage(Guid id)
        {
            try
            {
                var image = await _context.PromotionImages.FindAsync(id);

                if (image == null)
                {
                    return NotFound("Image not found");
                }

                return File(image.ImageData, image.ContentType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving image with ID {ImageId}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Promotions/SetMainImage/5
        [HttpPut("SetMainImage/{promotionId}/{imageId}")]
        [CheckPersonLoginSignup]
        public async Task<IActionResult> SetMainImage(Guid promotionId, Guid imageId)
        {
            try
            {
                // First verify image exists using direct query to minimize tracking issues
                bool imageExists = await _context.PromotionImages
                    .AsNoTracking()
                    .AnyAsync(i => i.ImageId == imageId && i.PromotionId == promotionId);

                if (!imageExists)
                {
                    return NotFound("Image not found in this promotion");
                }

                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    // Direct SQL approach to avoid concurrency issues with entity tracking
                    // First, set all images in this promotion to not be the main image
                    var resetCommand = "UPDATE PromotionImages SET IsMainImage = 0 WHERE PromotionId = {0}";
                    await _context.Database.ExecuteSqlRawAsync(resetCommand, promotionId);

                    // Then set the specific image as the main image
                    var setCommand = "UPDATE PromotionImages SET IsMainImage = 1 WHERE ImageId = {0}";
                    await _context.Database.ExecuteSqlRawAsync(setCommand, imageId);

                    // Update the promotion's UpdatedAt timestamp using direct SQL as well
                    var updatePromotionCommand = "UPDATE CoursePromotion SET UpdatedAt = {0} WHERE PromotionId = {1}";
                    long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    await _context.Database.ExecuteSqlRawAsync(updatePromotionCommand, timestamp, promotionId);

                    await transaction.CommitAsync();
                    return NoContent();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Database error setting main image for promotion {PromotionId}", promotionId);
                    return StatusCode(500, "Error setting main image. Please try again.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting main image for promotion {PromotionId}", promotionId);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Promotions/UpdateStatus/5
        [HttpPut("UpdateStatus/{id}")]
        [CheckPersonLoginSignup]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusDto statusDto)
        {
            try
            {
                // First check if promotion exists
                var promotionExists = await _context.CoursePromotion
                    .AsNoTracking()
                    .AnyAsync(p => p.PromotionId == id);

                if (!promotionExists)
                {
                    return NotFound("Promotion not found");
                }

                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    // Use direct SQL update to avoid concurrency issues with tracked entities
                    var updateCommand = "UPDATE CoursePromotion SET Status = {0}, UpdatedAt = {1} WHERE PromotionId = {2}";
                    long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                    int rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                        updateCommand,
                        (int)statusDto.Status,
                        timestamp,
                        id);

                    if (rowsAffected == 0)
                    {
                        await transaction.RollbackAsync();
                        return NotFound("Promotion not found or was deleted");
                    }

                    await transaction.CommitAsync();
                    return NoContent();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Database error updating status for promotion {PromotionId}", id);
                    return StatusCode(500, "Error updating status. Please try again.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status for promotion {PromotionId}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private bool PromotionExists(Guid id)
        {
            return _context.CoursePromotion.Any(e => e.PromotionId == id);
        }
    }
}