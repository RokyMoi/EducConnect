using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Entities;
using EduConnect.Interfaces.Course;
using EduConnect.Services;
using Microsoft.AspNetCore.Mvc;

namespace EduConnect.Controllers.Course
{
    [ApiController]
    [Route("public/course")]
    public class CoursePublicAccessController(
        ICourseRepository _courseRepository,
        AzureBlobStorageService _azureBlobStorageService
    ) : ControllerBase
    {
        [HttpGet("thumbnail/get")]
        public async Task<IActionResult> GetCourseThumbnail([FromQuery] Guid courseId)
        {
            var courseThumbnail = await _courseRepository.GetCourseThumbnailByCourseId(courseId);

            if (courseThumbnail == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Course thumbnail not found",
                        null
                    )
                );
            }

            if (courseThumbnail.ThumbnailImageFile != null)
            {
                return File(courseThumbnail.ThumbnailImageFile, courseThumbnail.ContentType);
            }


            try
            {
                var (stream, contentType) = await _azureBlobStorageService.DownloadCourseThumbnailAsync(courseId);
                return File(stream, contentType);

            }
            catch (FileNotFoundException fileEx)
            {

                Console.WriteLine("Thumbnail for course " + courseId + " not found");
                Console.WriteLine(fileEx);
                await _courseRepository.DeleteCourseThumbnail(courseId);

                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Course thumbnail not found",
                        null
                    )
                );
            }


        }

    }
}