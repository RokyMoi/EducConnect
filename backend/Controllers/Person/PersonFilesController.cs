using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EduConnect.Data;
using EduConnect.DTOs;
using EduConnect.Entities;
using EduConnect.Entities.Course;
using EduConnect.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduConnect.Helpers;

namespace EduConnect.Controllers.Person
{
    [ApiController]
    [Route("person/files")]
    [AuthenticationGuard(isTutor: true, isAdmin: true, isStudent: true)]
    public class PersonFilesController(
        ILogger<PersonFilesController> logger,
        DataContext dataContext,
        IHttpContextAccessor httpContextAccessor
    ) : ControllerBase
    {
        private readonly ILogger<PersonFilesController> _logger = logger;
        private readonly DataContext _dataContext = dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        [HttpGet("all")]
        public async Task<IActionResult> GetAllFilesUploadedByPerson()
        {
            var personId = _httpContextAccessor.HttpContext.Items["PersonId"].ToString();
            var role = _httpContextAccessor.HttpContext.Items["Role"].ToString();

            _logger.LogInformation("Fetching all files uploaded by person with ID: {PersonId} and role: {Role}", personId, role);

            //Fetch all files from CourseTeachingResource table
            var teachingResources = await _dataContext.CourseTeachingResource
            .Include(x => x.Course.Tutor)
            .Where(x => x.Course.Tutor.PersonId == Guid.Parse(personId) && x.FileData != null)
            .Select(x =>

                new GetAllFilesUploadedByPersonResponse
                {
                    Id = x.CourseTeachingResourceId,
                    Source = $"/tutor/course/teaching-resources/details/{x.CourseId}/{x.CourseTeachingResourceId}",
                    Title = x.Title,
                    Description = x.Description,
                    FileName = x.FileName,
                    ContentType = x.ContentType,
                    FileSize = x.FileSize,
                    DownloadUrl = $"/tutor/course/teaching-resource/download?courseTeachingResourceId={x.CourseTeachingResourceId}",
                    FileSourceType = Enums.FileSourceType.CourseTeachingResource,
                    CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(x.CreatedAt).DateTime,
                    UpdatedAt = x.UpdatedAt.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(x.UpdatedAt.Value).DateTime : null,
                }
            )
            .ToListAsync();

            var lessonResources = await _dataContext.CourseLessonResource
            .Include(x => x.CourseLesson.Course.Tutor)
            .Where(x => x.CourseLesson.Course.Tutor.PersonId == Guid.Parse(personId) && x.FileData != null)
            .Select(
                x => new GetAllFilesUploadedByPersonResponse
                {
                    Id = x.CourseLessonResourceId,
                    Source = "/tutor/course/lessons/resources/details/" + x.CourseLesson.CourseId + "/" + x.CourseLessonId + "/" + x.CourseLessonResourceId,
                    Title = x.Title,
                    Description = x.Description,
                    FileName = x.FileName,
                    ContentType = x.ContentType,
                    FileSize = x.FileSize,
                    DownloadUrl = $"tutor/course/lessons/resources/details/{x.CourseLesson.CourseId}/{x.CourseLessonId}/{x.CourseLessonResourceId}",
                    FileSourceType = Enums.FileSourceType.CourseLessonResource,
                    CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(x.CreatedAt).DateTime,
                    UpdatedAt = x.UpdatedAt.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(x.UpdatedAt.Value).DateTime : null,


                }
            )
            .ToListAsync();

            var thumbnails = await _dataContext.CourseThumbnail
            .Include(x => x.Course)
            .Include(x => x.Course.Tutor)
            .Where(x => x.Course.Tutor.PersonId == Guid.Parse(personId) && x.ThumbnailImageFile != null)
            .Select(
                x => new GetAllFilesUploadedByPersonResponse
                {
                    Id = x.CourseThumbnailId,
                    Source = $"/tutor/course/thumbnail/{x.CourseThumbnailId}",
                    Title = $"{x.Course.Title} thumbnail",
                    Description = $"This is the thumbnail for the course {x.Course.Title}",
                    FileName = x.Course.Title.ToLower().Replace(" ", "-") + "-thumbnail." + FileExtensionHelper.GetFileExtension(x.ContentType),
                    ContentType = x.ContentType,
                    FileSize = x.ThumbnailImageFile.Length,
                    FileSourceType = Enums.FileSourceType.CourseThumbnail,
                    CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(x.CreatedAt).DateTime,
                    UpdatedAt = x.UpdatedAt.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(x.UpdatedAt.Value).DateTime : null,


                }
            ).ToListAsync();

            var promotionImages = await _dataContext.CoursePromotionImage
            .Include(x => x.Course)
            .Include(x => x.Course.Tutor)
            .Where(
                x => x.Course.Tutor.PersonId == Guid.Parse(personId)
            )
            .Select(
                x => new GetAllFilesUploadedByPersonResponse
                {
                    Id = x.CoursePromotionImageId,
                    Source = $"/tutor/course/promotion/details/{x.CourseId}/{x.CoursePromotionImageId}",
                    Title = $"{x.Course.Title} promotion image",
                    Description = $"This is the promotion image for the course {x.Course.Title}",
                    FileName = $"{x.CoursePromotionImageId.ToString().Replace("-", "")}_promotion_image.{FileExtensionHelper.GetFileExtension(x.ContentType)}",
                    ContentType = x.ContentType,
                    FileSize = x.ImageFile.Length,
                    FileSourceType = Enums.FileSourceType.CoursePromotionImage,
                    CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(x.CreatedAt).DateTime,
                    UpdatedAt = x.UpdatedAt.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(x.UpdatedAt.Value).DateTime : null,
                }
            ).ToListAsync();

            var userFiles = teachingResources;
            userFiles.AddRange(lessonResources);
            userFiles.AddRange(thumbnails);
            userFiles.AddRange(promotionImages);


            return Ok(
                ApiResponse<List<GetAllFilesUploadedByPersonResponse>>.GetApiResponse(
                    "Successfully fetched all files uploaded by person",
                    userFiles)
            );
        }
    }
}