using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using backend.Interfaces.Reference;
using backend.Interfaces.Tutor;
using EduConnect.Constants;
using EduConnect.Data;
using EduConnect.DTOs;
using EduConnect.DTOs.Course;
using EduConnect.Entities;
using EduConnect.Entities.Course;
using EduConnect.Enums;
using EduConnect.Interfaces.Course;
using EduConnect.Middleware;
using EduConnect.Services;
using EduConnect.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Stripe.Forwarding;

namespace EduConnect.Controllers.Course
{
    [ApiController]
    [Route("tutor/course")]
    [AuthenticationGuard(isTutor: true, isAdmin: false, isStudent: false)]
    public class CourseTutorController(
        DataContext _dataContext,
        ICourseRepository _courseRepository,
        IReferenceRepository _referenceRepository,
        ITutorRepository _tutorRepository,
        AzureBlobStorageService _azureBlobStorageService
    ) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest request)
        {
            //Check if any of the courses contain the same title
            var courseTitleTaken = await _courseRepository.CourseExistsByTitle(request.Title);

            if (courseTitleTaken)
            {
                return Conflict(
                    ApiResponse<object>.GetApiResponse(
                       "You  cannot create a course with the same title as an existing course",
                       null
                    )
                );
            }

            //Check if the CourseCategoryId exists
            var courseCategoryExists = await _courseRepository.CourseCategoryExistsById(request.CourseCategoryId);

            if (!courseCategoryExists)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "The course category does not exist",
                        null
                    )
                );

            }

            //Check if the LearningDifficultyLevelId exists
            var learningDifficultyLevelExists = await _referenceRepository.LearningDifficultyLevelExistsById(request.LearningDifficultyLevelId);

            if (!learningDifficultyLevelExists)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "The learning difficulty level not found",
                        null
                    )
                );
            }


            //Check if the minimum number of students and maximum number of students (if provided respectively) are greater than or equal to zero, and the minimum number of students is less than maximum number of students
            if (request.MinNumberOfStudents.HasValue && request.MaxNumberOfStudents.HasValue && request.MinNumberOfStudents >= request.MaxNumberOfStudents)
            {
                return BadRequest(
                    ApiResponse<object>.GetApiResponse(
                        "The minimum number of students must be less than the maximum number of students",
                        null
                    )
                );

            }

            //Get the TutorId from the PersonId
            var personId = Guid.Parse(HttpContext.Items["PersonId"].ToString());


            Console.WriteLine("Get Tutor by this PersonId: " + personId);
            var tutor = await _tutorRepository.GetTutorByPersonId(personId);

            Console.WriteLine("Is tutor null? " + (tutor == null));

            var course = new Entities.Course.Course
            {
                CourseId = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                CourseCategoryId = request.CourseCategoryId,
                TutorId = tutor.TutorId,
                LearningDifficultyLevelId = request.LearningDifficultyLevelId,
                MinNumberOfStudents = request.MinNumberOfStudents,
                MaxNumberOfStudents = request.MaxNumberOfStudents,
                Price = request.Price,
                PublishedStatus = Enums.PublishedStatus.Draft,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UpdatedAt = null

            };

            Console.WriteLine(course);

            var courseCreateResult = await _courseRepository.CreateCourse(course);

            if (!courseCreateResult)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while creating the course, please try again",
                        null
                    )
                );
            }



            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "Course created successfully",
                    null
                )
            );

        }

        [HttpGet("check-title")]
        public async Task<IActionResult> CheckCourseTitle([FromQuery] string title)
        {
            var courseTitleTaken = await _courseRepository.CourseExistsByTitle(title);

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    courseTitleTaken ? "Course title already taken" : "",
                    courseTitleTaken
                )
            );



        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCourses()
        {
            var personId = Guid.Parse(HttpContext.Items["PersonId"].ToString());

            var tutor = await _tutorRepository.GetTutorByPersonId(personId);

            if (tutor == null)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while getting the courses, please refer to your administrator, regarding your role",
                        null
                    )
                );
            }


            var courses = await _courseRepository.GetAllCoursesByTutorId(tutor.TutorId);

            if (courses == null || courses.Count == 0)
            {
                return NoContent();
            }

            var response = new List<GetAllCoursesResponse>();

            foreach (var x in courses)
            {

                string? thumbnailUrl = null;
                bool hasThumbnail = x.CourseThumbnail != null;

                if (hasThumbnail)
                {
                    thumbnailUrl = !string.IsNullOrEmpty(x.CourseThumbnail.ThumbnailUrl)
                        ? x.CourseThumbnail.ThumbnailUrl
                        : $"{Request.Scheme}://{Request.Host}/public/course/thumbnail/get?courseId={x.CourseId}";
                }

                response.Add(new GetAllCoursesResponse
                {
                    CourseId = x.CourseId,
                    Title = x.Title,
                    Description = x.Description,
                    CourseCategoryId = x.CourseCategoryId,
                    CourseCategoryName = x.CourseCategory.Name,
                    LearningDifficultyLevelId = x.LearningDifficultyLevelId,
                    LearningDifficultyLevelName = x.LearningDifficultyLevel.Name,
                    MinNumberOfStudents = x.MinNumberOfStudents,
                    MaxNumberOfStudents = x.MaxNumberOfStudents,
                    Price = x.Price,
                    PublishedStatus = x.PublishedStatus,
                    CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(x.CreatedAt).UtcDateTime,
                    HasThumbnail = x.CourseThumbnail != null,
                    ThumbnailUrl = thumbnailUrl,
                    NumberOfLessons = await _courseRepository.GetLessonCountByCourseId(x.CourseId),
                });
            }
            ;
            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "Courses retrieved successfully",
                    response
                )
            );
        }








        [HttpGet("info")]
        public async Task<IActionResult> GetCourseManagementDashboardInfo(
            [FromQuery] Guid courseId
        )
        {
            var personId = Guid.Parse(HttpContext.Items["PersonId"].ToString());

            var tutor = await _tutorRepository.GetTutorByPersonId(personId);

            if (tutor == null)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while getting the courses, please refer to your administrator, regarding your role",
                        null
                    )
                );
            }

            var course = await _courseRepository.GetCourseById(courseId);

            if (course == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Course not found",
                        null
                    )
                );

            }

            var thumbnail = await _courseRepository.GetCourseThumbnailByCourseId(courseId);

            Console.WriteLine("Does course have a thumbnail? " + (thumbnail != null));

            //Get Course Teaching Resources information
            /*
            INFORMATION:
            - Total number of teaching resources
            - Number of URL
            - Number of files and their total size
            - Two latest added teaching resources
            */

            var teachingResourcesInformation = await _courseRepository.GetCourseTeachingResourcesInformationByCourseId(courseId);


            var lessonCountResponse = await _courseRepository.GetCourseLessonsCountFilteredByPublishedStatus(courseId);

            var latestLessons = await _courseRepository.GetLatestCourseLessons(courseId);

            var promotionImagesMetadata = await _courseRepository.GetCoursePromotionImagesMetadataForCourseManagementDashboard(courseId);
            var response = new CourseManagementDashboardResponse
            {
                CourseId = course.CourseId,
                Title = course.Title,
                DifficultyLevel = course.LearningDifficultyLevel.Name,
                Category = course.CourseCategory.Name,
                CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(course.CreatedAt).UtcDateTime,
                UpdatedAt = course.UpdatedAt != null ? DateTimeOffset.FromUnixTimeMilliseconds((long)course.UpdatedAt).UtcDateTime : null,
                IsThumbnailAdded = thumbnail != null,
                ThumbnailAddedOn = thumbnail != null ? DateTimeOffset.FromUnixTimeMilliseconds((long)thumbnail.CreatedAt).UtcDateTime : null,
                IsUsingAzureStorage = thumbnail != null && !string.IsNullOrEmpty(thumbnail.ThumbnailUrl),
                TotalNumberOfTeachingResources = teachingResourcesInformation == null ? 0 : teachingResourcesInformation.TotalNumberOfTeachingResources,
                NumberOfURLs = teachingResourcesInformation == null ? 0 : teachingResourcesInformation.NumberOfURLs,
                NumberOfFiles = teachingResourcesInformation == null ? 0 : teachingResourcesInformation.NumberOfFiles,
                TotalSizeOfFilesInBytes = teachingResourcesInformation == null ? 0 : teachingResourcesInformation.TotalSizeOfFilesInBytes,
                TwoLatestAddedTeachingResources = teachingResourcesInformation == null ? [] : teachingResourcesInformation.TwoLatestAddedTeachingResources,
                NumberOfLessons = lessonCountResponse != null ? lessonCountResponse.TotalNumberOfLessons : 0,
                NumberOfPublishedLessons = lessonCountResponse != null ? lessonCountResponse.NumberOfPublishedLessons : 0,
                NumberOfDraftLessons = lessonCountResponse != null ? lessonCountResponse.NumberOfDraftLessons : 0,
                NumberOfArchivedLessons = lessonCountResponse != null ? lessonCountResponse.NumberOfArchivedLessons : 0,
                TwoLatestAddedLessons = latestLessons ?? [],
                NumberOfPromotionImages = promotionImagesMetadata.Item1,
                LatestPromotionImageUploadedAt = promotionImagesMetadata.Item2.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds((long)promotionImagesMetadata.Item2).UtcDateTime : null,



            };

            PrintObjectUtility.PrintObjectProperties(response);

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "Course retrieved successfully",
                    response
                )
            );

        }

        [HttpGet("basics")]
        public async Task<IActionResult> GetCourseBasics([FromQuery] Guid courseId)
        {
            var personId = Guid.Parse(HttpContext.Items["PersonId"].ToString());

            var tutor = await _tutorRepository.GetTutorByPersonId(personId);

            if (tutor == null)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while getting the courses, please refer to your administrator, regarding your role",
                        null
                    )
                );
            }

            var course = await _courseRepository.GetCourseById(courseId);

            if (course == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Course not found",
                        null
                    )
                );
            }

            var response = new GetAllCoursesResponse
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Description = course.Description,
                CourseCategoryId = course.CourseCategoryId,
                CourseCategoryName = course.CourseCategory.Name,
                LearningDifficultyLevelId = course.LearningDifficultyLevelId,
                LearningDifficultyLevelName = course.LearningDifficultyLevel.Name,
                MinNumberOfStudents = course.MinNumberOfStudents,
                MaxNumberOfStudents = course.MaxNumberOfStudents,
                Price = course.Price,
                PublishedStatus = course.PublishedStatus,
                CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(course.CreatedAt).UtcDateTime,

            };

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "Course retrieved successfully",
                    response
                )
            );
        }

        [HttpPatch("update/basics")]
        public async Task<IActionResult> UpdateCourseBasics(
            [FromBody] UpdateCourseBasicsRequest request
        )
        {
            var personId = Guid.Parse(HttpContext.Items["PersonId"].ToString());

            var tutor = await _tutorRepository.GetTutorByPersonId(personId);

            if (tutor == null)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while getting the courses, please refer to your administrator, regarding your role",
                        null
                    )
                );
            }

            //Check if the course exists and if the tutor is the owner of the course
            var course = await _courseRepository.GetCourseById(request.CourseId);

            if (course == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Course not found",
                        null
                    )
                );
            }

            //Check if the course title is taken by another course
            var courseTitleExists = await _courseRepository.CourseExistsByTitleExceptTheGivenCourseById(request.CourseId, request.Title);

            if (courseTitleExists)
            {
                return Conflict(
                    ApiResponse<object>.GetApiResponse(
                        "Course title already exists",
                        null
                    )
                );
            }


            if (course.TutorId != tutor.TutorId)
            {
                return Conflict(
                    ApiResponse<object>.GetApiResponse(
                        "You are not authorized to update this course",
                        null
                    )
                );
            }

            //Check if the course category exists
            var courseCategory = await _referenceRepository.CourseCategoryExistsById(request.CourseCategoryId);

            if (!courseCategory)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Course category not found",
                        null
                    )
                );
            }


            //Check if the learning difficulty level exists
            var learningDifficultyLevel = await _referenceRepository.LearningDifficultyLevelExistsById(request.LearningDifficultyLevelId);

            if (!learningDifficultyLevel)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Learning difficulty level not found",
                        null
                    )
                );
            }

            //Update the course 
            course.Title = request.Title;
            course.Description = request.Description;
            course.CourseCategoryId = request.CourseCategoryId;
            course.LearningDifficultyLevelId = request.LearningDifficultyLevelId;
            course.MinNumberOfStudents = request.MinNumberOfStudents;
            course.MaxNumberOfStudents = request.MaxNumberOfStudents;
            course.Price = request.Price;
            course.PublishedStatus = request.PublishedStatus;
            course.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var updateResult = await _courseRepository.UpdateCourseBasics(course);

            if (!updateResult)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while updating the course, please try again later",
                        null
                    )
                );
            }

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "Course updated successfully",
                    null
                )
            );


        }

        [HttpGet("check-title-except")]
        public async Task<IActionResult> CheckCourseTitleExistEmitGivenCourse(
            [FromQuery] CheckCourseTitleExistEmitGivenCourseRequest request
        )
        {
            var courseTitleExists = await _courseRepository.CourseExistsByTitleExceptTheGivenCourseById(request.CourseId, request.Title);

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    $"Course title {request.Title} {(courseTitleExists ? "already exists" : "does not exist")}",
                    courseTitleExists
                )
            );
        }

        [HttpPost("thumbnail/upload")]
        public async Task<IActionResult> UploadCourseThumbnail(UploadCourseThumbnailRequest request)
        {
            const int maxFileSize = 5 * 1024 * 1024; // 5MB
            //Check file size 
            if (request.ThumbnailData.Length > maxFileSize)
            {
                return BadRequest(
                    ApiResponse<object>.GetApiResponse(
                        "Thumbnail maximum file size is 5MB, this file has a size of " + request.ThumbnailData.Length + " bytes",
                        null
                    )
                );
            }

            //Check if the course exists
            var course = await _courseRepository.GetCourseById(request.CourseId);
            if (course == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Course not found",
                        null
                    )
                );
            }

            //Check if file type is image
            if (!request.ThumbnailData.ContentType.StartsWith("image/"))
            {
                return BadRequest(
                    ApiResponse<object>.GetApiResponse(
                        "Thumbnail must be an image",
                        null
                    )
                );
            }

            //Check if the course already has a thumbnail (is it create or update operation)
            var courseThumbnail = await _courseRepository.GetCourseThumbnailByCourseId(request.CourseId);
            bool isUpdate = courseThumbnail != null;
            bool isChangingStorageType = false;

            bool currentlyUsingAzureStorage = false;

            if (isUpdate)
            {
                currentlyUsingAzureStorage = !string.IsNullOrEmpty(courseThumbnail.ThumbnailUrl) && courseThumbnail.ThumbnailImageFile == null;
                isChangingStorageType = currentlyUsingAzureStorage != request.UseAzureStorage;
            }


            string? uploadImageUrl = null;
            byte[]? thumbnailImageFile = null;



            if (request.UseAzureStorage)
            {
                uploadImageUrl = await _azureBlobStorageService.UploadCourseThumbnailAsync(request.ThumbnailData, request.CourseId.ToString());
            }

            if (!request.UseAzureStorage)
            {
                using var memoryStream = new MemoryStream();
                await request.ThumbnailData.CopyToAsync(memoryStream);
                thumbnailImageFile = memoryStream.ToArray();
            }

            if (isChangingStorageType)
            {
                if (currentlyUsingAzureStorage && !request.UseAzureStorage)
                {
                    await _azureBlobStorageService.DeleteCourseThumbnailAsync(request.CourseId);
                }

            }

            if (!isUpdate)
            {
                courseThumbnail = new CourseThumbnail
                {
                    CourseThumbnailId = Guid.NewGuid(),
                    CourseId = request.CourseId,
                    ThumbnailUrl = request.UseAzureStorage ? uploadImageUrl : null,
                    ThumbnailImageFile = request.UseAzureStorage ? null : thumbnailImageFile,
                    ContentType = request.ThumbnailData.ContentType,
                    CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    UpdatedAt = null
                };

                var createResult = await _courseRepository.CreateCourseThumbnail(courseThumbnail);

                if (!createResult)
                {
                    return StatusCode(
                        500,
                        ApiResponse<object>.GetApiResponse("Failed to create a thumbnail for this course, please try again later", null)
                    );
                }
            }

            if (isUpdate)
            {
                courseThumbnail.ThumbnailUrl = request.UseAzureStorage ? uploadImageUrl : null;
                courseThumbnail.ThumbnailImageFile = request.UseAzureStorage ? null : thumbnailImageFile;
                courseThumbnail.ContentType = request.ThumbnailData.ContentType;
                courseThumbnail.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                var updateResult = await _courseRepository.UpdateCourseThumbnail(courseThumbnail);

                if (!updateResult)
                {
                    return StatusCode(
                        500,
                        ApiResponse<object>.GetApiResponse("Failed to update the thumbnail for this course, please try again later", null)
                    );
                }
            }

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "Thumbnail uploaded successfully",
                    null
                )
            );
        }

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


        [HttpDelete("thumbnail/delete")]
        public async Task<IActionResult> DeleteCourseThumbnail([FromQuery] Guid courseId)
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

            var tutor = await _tutorRepository.GetTutorByPersonId(Guid.Parse(HttpContext.Items["PersonId"].ToString()));


            if (tutor == null)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while deleting the thumbnail, please refer to your administrator, regarding your role",
                        null
                    )
                );
            }

            if (tutor.TutorId != courseThumbnail.Course.TutorId)
            {
                return StatusCode(
                    403,
                    ApiResponse<object>.GetApiResponse(
                        "You are not authorized to delete this thumbnail",
                        null
                    )
                );
            }
            if (!string.IsNullOrEmpty(courseThumbnail.ThumbnailUrl))
            {


                var blobDeleteResult = await _azureBlobStorageService.DeleteCourseThumbnailAsync(courseId);

                if (!blobDeleteResult)
                {
                    return StatusCode(
                        500,
                        ApiResponse<object>.GetApiResponse(
                            "An error occurred while deleting the thumbnail on remote server, please try again later",
                            null
                        )
                    );
                }

                return Ok(ApiResponse<object>.GetApiResponse("Course thumbnail deleted successfully from remote server", null));
            }
            var deleteResult = await _courseRepository.DeleteCourseThumbnail(courseId);

            if (!deleteResult)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while deleting the thumbnail, please try again later",
                        null
                    )
                );
            }

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "Course thumbnail deleted successfully from local server",
                    null
                )
            );


        }

        [HttpPost("teaching-resource/upload")]
        public async Task<IActionResult> UploadCourseTeachingResource(UploadCourseTeachingResourceRequest request)
        {

            //Check is it update or create operation (determined by the CourseTeachingResourceId field and the CourseId field from the request, if the CourseTeachingResourceId field is null and CourseId is populated by Guid value, then it is a create operation, and if it is vice versa, then it is an update operation)

            //Check if the teaching resource exists (update operation)
            if (request.CourseTeachingResourceId.HasValue && !await _courseRepository.CourseTeachingResourceExists(request.CourseTeachingResourceId.Value))
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse("Course teaching resource not found", null)
                    );
            }

            //Check if the course exists (create operation)
            if (request.CourseId.HasValue && !await _courseRepository.CourseExistsById(request.CourseId.Value))
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse("Course not found", null)
                    );
            }

            //Check if both the ResourceUrl and ResourceFile fields are provided (only one should be provided)

            if (!string.IsNullOrEmpty(request.ResourceUrl) && request.ResourceFile != null)
            {
                return BadRequest(
                    ApiResponse<object>.GetApiResponse("A single upload request can only contain either a file or an url for upload", null)
                );
            }

            //Check if both of the ResourceUrl and ResourceFile fields are empty
            if (string.IsNullOrEmpty(request.ResourceUrl) && request.ResourceFile == null)
            {
                return BadRequest(
                    ApiResponse<object>.GetApiResponse("Resource to upload must be provided", null)
                );
            }



            //Check if the ResourceFile is provided, validate file type and size
            if (request.ResourceFile != null)
            {
                long fileSize = request.ResourceFile.Length;
                string fileType = request.ResourceFile.ContentType;

                var fileCategoryMap = new Dictionary<string[], long>
                {
                    { AllowedFileTypes.Documents, MaxFileTypesSizes.MaxDocumentSizeInBytes},
                    { AllowedFileTypes.Archives, MaxFileTypesSizes.MaxArchiveSizeInBytes},
                    { AllowedFileTypes.Audio, MaxFileTypesSizes.MaxAudioSizeInBytes},
                    { AllowedFileTypes.Images, MaxFileTypesSizes.MaxImageSizeInBytes},
                    { AllowedFileTypes.Videos, MaxFileTypesSizes.MaxVideoSizeInBytes}
                };

                bool isValidFile = false;

                foreach (var entry in fileCategoryMap)
                {
                    if (entry.Key.Contains(fileType))
                    {
                        if (fileSize > entry.Value)
                        {
                            return BadRequest(ApiResponse<object>.GetApiResponse(
                                $"File size exceeds the maximum allowed limit of {entry.Value / (1024 * 1024)}MB for this file type", null));
                        }
                        isValidFile = true;
                        break; // No need to check other categories once a match is found
                    }
                }

                if (!isValidFile)
                {
                    return BadRequest(ApiResponse<object>.GetApiResponse("Invalid file type", null));
                }

            }

            if (request.CourseTeachingResourceId.HasValue)
            {
                var existingResource = await _courseRepository.GetCourseTeachingResourceById(request.CourseTeachingResourceId.Value);

                if (existingResource == null)
                {
                    return NotFound(ApiResponse<object>.GetApiResponse("Resource not found", null));
                }


                existingResource.Title = request.Title;
                existingResource.Description = request.Description;
                existingResource.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                if (!string.IsNullOrEmpty(request.ResourceUrl))
                {
                    existingResource.ResourceUrl = request.ResourceUrl;
                    existingResource.FileData = null;
                    existingResource.FileName = null;
                    existingResource.ContentType = null;
                    existingResource.FileSize = null;
                }
                else
                {
                    using var memoryStream = new MemoryStream();
                    await request.ResourceFile.CopyToAsync(memoryStream);
                    existingResource.FileData = memoryStream.ToArray();
                    existingResource.FileName = request.ResourceFile.FileName;
                    existingResource.ContentType = request.ResourceFile.ContentType;
                    existingResource.FileSize = request.ResourceFile.Length;
                    existingResource.ResourceUrl = null;
                }

                var updateResult = await _courseRepository.UpdateCourseTeachingResource(existingResource);


                if (!updateResult)
                {
                    return StatusCode(500, ApiResponse<object>.GetApiResponse("Failed to update resource, please try again later", null));
                }

                return Ok(
                    ApiResponse<object>.GetApiResponse("Resource updated successfully", null)
                );
            }

            byte[] tempFileData = null;
            if (request.ResourceFile != null)
            {
                using var memoryStream = new MemoryStream();
                await request.ResourceFile.CopyToAsync(memoryStream);
                tempFileData = memoryStream.ToArray();
            }


            var newResource = new CourseTeachingResource
            {
                CourseId = request.CourseId.Value,
                Title = request.Title,
                Description = request.Description,
                ResourceUrl = request.ResourceUrl,
                FileData = tempFileData,
                FileName = request.ResourceFile?.FileName,
                ContentType = request.ResourceFile?.ContentType,
                FileSize = request.ResourceFile?.Length,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            var createResult = await _courseRepository.CreateCourseTeachingResource(newResource);

            if (!createResult)
            {
                return StatusCode(500, ApiResponse<object>.GetApiResponse("Failed to upload resource, please try again later", null));
            }
            return Ok(
                ApiResponse<object>.GetApiResponse("Resource uploaded successfully", null)
            );


        }

        [HttpGet("teaching-resource/get")]
        public async Task<IActionResult> GetCourseTeachingResource(Guid courseTeachingResourceId)
        {
            var resource = await _courseRepository.GetCourseTeachingResourceById(courseTeachingResourceId);

            if (resource == null)
            {
                return NotFound(ApiResponse<object>.GetApiResponse("Resource not found", null));
            }

            GetCourseTeachingResourceResponse response = new()
            {
                CourseTeachingResourceId = resource.CourseTeachingResourceId,
                Title = resource.Title,
                Description = resource.Description,
                ResourceUrl = resource.ResourceUrl,
            };

            if (resource.FileData != null)
            {
                response.ResourceUrl = null;
                response.FileName = resource.FileName;
                response.ContentType = resource.ContentType;
                response.FileSize = resource.FileSize;

            }



            return Ok(
                ApiResponse<object>.GetApiResponse("Resource retrieved successfully", response)
            );
        }

        [HttpGet("teaching-resource/all")]
        public async Task<IActionResult> GetAllCourseTeachingResourcesByCourseId([FromQuery] Guid courseId)
        {

            var courseExists = await _courseRepository.CourseExistsById(courseId);

            if (!courseExists)
            {
                return NotFound(ApiResponse<object>.GetApiResponse("Course not found", null));
            }


            var resources = await _courseRepository.GetAllCourseTeachingResourcesWithoutFileDataByCourseId(courseId);

            if (resources == null || resources.Count == 0)
            {
                return NotFound(ApiResponse<object>.GetApiResponse("No resources found for the course", null));
            }

            return Ok(
                ApiResponse<object>.GetApiResponse("Resources retrieved successfully", resources)
            );


        }

        [HttpGet("teaching-resource/download")]
        public async Task<IActionResult> DownloadCourseTeachingResource(Guid courseTeachingResourceId)
        {
            var resourceFile = await _courseRepository.GetCourseLessonResourceById(courseTeachingResourceId);

            if (resourceFile == null)
            {
                return NotFound(ApiResponse<object>.GetApiResponse("Resource not found", null));
            }

            if (resourceFile.FileData == null)
            {
                return Conflict(ApiResponse<object>.GetApiResponse("Only files hosted on EduConnect server's can be downloaded", null));
            }

            return File(resourceFile.FileData, resourceFile.ContentType, resourceFile.FileName);
        }

        [HttpDelete("teaching-resource/delete")]
        public async Task<IActionResult> DeleteCourseTeachingResource(Guid courseTeachingResourceId)
        {
            var resource = await _courseRepository.GetCourseTeachingResourceByIdWithoutFileData(courseTeachingResourceId);

            if (resource == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Resource not found",
                        null
                    )
                );

            }

            //Check if the resource if owned by the course tutor
            var tutor = await _tutorRepository.GetTutorByPersonId(Guid.Parse(HttpContext.Items["PersonId"].ToString()));


            if (tutor == null)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while deleting the resource, please refer to your administrator, regarding your role",
                        null
                    )
                );
            }

            if (tutor.TutorId != resource.Course.TutorId)
            {
                return StatusCode(
                    403,
                    ApiResponse<object>.GetApiResponse(
                        "You are not authorized to delete this resource",
                        null
                    )
                );
            }


            bool deleteResult = await _courseRepository.DeleteCourseTeachingResourceById(courseTeachingResourceId);


            if (!deleteResult)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while deleting the resource, please try again",
                        null
                    )
                );
            }

            return Ok(
                ApiResponse<object>.GetApiResponse("Resource deleted successfully", null)
            );




        }

        [HttpPost("lesson/create")]
        public async Task<IActionResult> CreateCourseLesson(CreateCourseLessonRequest request)
        {

            //Check if the course exists
            var course = await _courseRepository.GetCourseById(request.CourseId);
            if (course == null)
            {
                return NotFound(ApiResponse<object>.GetApiResponse("Course not found", null));
            }

            //Check if the resource if owned by the course tutor
            var tutor = await _tutorRepository.GetTutorByPersonId(Guid.Parse(HttpContext.Items["PersonId"].ToString()));


            if (tutor == null)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while deleting the resource, please refer to your administrator, regarding your role",
                        null
                    )
                );
            }

            if (tutor == null || course.TutorId != tutor.TutorId)
            {
                return StatusCode(
                    403,
                    ApiResponse<object>.GetApiResponse(
                        "You are not authorized to add lesson to this course",
                        null
                    )
                );
            }

            if (request.CourseLessonId.HasValue && request.CourseLessonId.Value != Guid.Empty)
            {
                var lesson = await _courseRepository.GetCourseLessonById(request.CourseLessonId.Value);

                if (lesson != null)
                {
                    var publishedCourseLessonCount = await _courseRepository.GetPublishedCourseLessonCountByCourseId(lesson.CourseId);
                    var setLessonSequenceOrder = request.LessonSequenceOrder.Value;

                    var publishedLessons = await _courseRepository.GetAllCourseLessons(lesson.CourseId);
                    publishedLessons = [.. publishedLessons.Where(l => l.PublishedStatus == PublishedStatus.Published)];

                    //Check if the user wants to update the lesson sequence order position, and if the provided new position is larger than the current max position, if so, then set the lessons position to the max position, and decrement the position of all lessons between the current lesson's position and the max position
                    if (request.LessonSequenceOrder.HasValue && request.LessonSequenceOrder.Value > lesson.LessonSequenceOrder)
                    {
                        Console.WriteLine("User tried adding the lesson on a position larger than the current max position, moving the lesson to the end of the list");
                        var maxValue = publishedLessons.Max(l => l.LessonSequenceOrder);
                        setLessonSequenceOrder = (int)maxValue;

                        var currentLessonPosition = lesson.LessonSequenceOrder;

                        foreach (var lessonItem in _dataContext.CourseLesson.Where(x => x.CourseId == lesson.CourseId && x.LessonSequenceOrder > currentLessonPosition))
                        {
                            Console.WriteLine($"Lesson {lessonItem.Title} - {lessonItem.LessonSequenceOrder} -> {lessonItem.LessonSequenceOrder - 1}");
                            lessonItem.LessonSequenceOrder--;

                        }
                        await _dataContext.SaveChangesAsync();
                    }
                    //Check if the user wants to update the lesson sequence order position by moving it to a smaller or larger value position, and if so, then update the other lessons positions accordingly
                    else if (request.LessonSequenceOrder.HasValue && request.LessonSequenceOrder.Value != lesson.LessonSequenceOrder && request.LessonSequenceOrder.Value < publishedCourseLessonCount)
                    {
                        Console.WriteLine("Incrementing lessons sequence order");
                        setLessonSequenceOrder = request.LessonSequenceOrder.Value;
                        var currentPosition = lesson.LessonSequenceOrder;

                        Console.WriteLine($"Current position: {currentPosition}");
                        Console.WriteLine($"Set position: {setLessonSequenceOrder}");

                        if (currentPosition < setLessonSequenceOrder)
                        {
                            foreach (var lessonItem in _dataContext.CourseLesson.Where(x => x.LessonSequenceOrder > currentPosition && x.LessonSequenceOrder <= setLessonSequenceOrder))
                            {
                                Console.WriteLine($"Lesson {lessonItem.Title} - {lessonItem.LessonSequenceOrder} -> {lessonItem.LessonSequenceOrder - 1}");
                                lessonItem.LessonSequenceOrder--;
                            }
                        }
                        if (currentPosition > setLessonSequenceOrder)
                        {
                            foreach (var lessonItem in _dataContext.CourseLesson.Where(x => x.LessonSequenceOrder >= setLessonSequenceOrder && x.LessonSequenceOrder < currentPosition))
                            {
                                Console.WriteLine($"Lesson {lessonItem.Title} - {lessonItem.LessonSequenceOrder} -> {lessonItem.LessonSequenceOrder + 1}");
                                lessonItem.LessonSequenceOrder++;
                            }
                        }

                        await _dataContext.SaveChangesAsync();
                    }







                    lesson.Title = request.Title;
                    lesson.ShortSummary = request.ShortSummary;
                    lesson.Description = request.Description;
                    lesson.Topic = request.Topic;
                    lesson.LessonSequenceOrder = setLessonSequenceOrder;
                    lesson.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    if (lesson.CourseLessonContent == null)
                    {
                        lesson.CourseLessonContent = new CourseLessonContent
                        {
                            CourseLessonId = lesson.CourseLessonId,
                            Content = request.Content,
                            UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                        };
                    }
                    if (lesson.CourseLessonContent != null)
                    {

                        lesson.CourseLessonContent.Content = request.Content;
                        lesson.CourseLessonContent.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    }

                    var updateResult = await _courseRepository.UpdateCourseLesson(lesson);

                    if (!updateResult)
                    {
                        return StatusCode(
                            500,
                            ApiResponse<object>.GetApiResponse(
                                "An error occurred while updating the lesson, please try again",
                                null
                            )
                        );

                    }



                    return Ok(
                        ApiResponse<object>.GetApiResponse("Lesson updated successfully", null)
                    );

                }
            }

            var courseLesson = new CourseLesson
            {
                CourseId = request.CourseId,
                Title = request.Title,
                ShortSummary = request.ShortSummary,
                Description = request.Description,
                Topic = request.Topic,
                TutorId = tutor.TutorId,
            };

            var createResult = await _courseRepository.CreateCourseLesson(courseLesson);
            if (!createResult)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while creating the lesson, please try again",
                        null
                    )
                );
            }

            var lessonContent = new CourseLessonContent
            {
                CourseLessonId = courseLesson.CourseLessonId,
                Content = request.Content,
            };

            var createContentResult = await _courseRepository.CreateCourseLessonContent(lessonContent);

            if (!createContentResult)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "Failed to save the lesson content, but the lesson was created successfully",
                        null
                    )
                );
            }

            return Ok(
                ApiResponse<object>.GetApiResponse("Lesson created successfully", null)
            );



        }

        [HttpPatch("lesson/publish")]
        public async Task<IActionResult> PublishCourseLesson([FromQuery] ChangeCourseLessonPublishedStatusRequest request)
        {
            var courseLesson = await _courseRepository.GetCourseLessonById(request.CourseLessonId);
            if (courseLesson == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Lesson not found",
                        null
                    )
                );
            }



            if (courseLesson.PublishedStatus == PublishedStatus.Published && request.LessonSequenceOrder.HasValue)
            {
                return Ok(
                    ApiResponse<object>.GetApiResponse(
                        "Lesson is already published",
                        null
                    )
                );
            }

            var publishedCourseLessonCount = await _courseRepository.GetPublishedCourseLessonCountByCourseId(courseLesson.CourseId);

            var publishedLessons = await _courseRepository.GetAllCourseLessons(courseLesson.CourseId);
            publishedLessons = [.. publishedLessons.Where(l => l.PublishedStatus == PublishedStatus.Published)];


            Console.WriteLine($"Course lesson sequence order has value: {courseLesson.LessonSequenceOrder.HasValue}");
            Console.WriteLine($"Request lesson sequence order has value: {request.LessonSequenceOrder.HasValue}");
            Console.WriteLine($"request.LessonSequenceOrder.Value < publishedCourseLessonCount: {request.LessonSequenceOrder.Value < publishedCourseLessonCount}");
            Console.WriteLine($"Request lesson sequence order: {request.LessonSequenceOrder.Value}");
            Console.WriteLine($"Stored lesson sequence order: {courseLesson.LessonSequenceOrder ?? 0}");
            Console.WriteLine($"Published course lesson count: {publishedCourseLessonCount}");
            int setLessonSequenceOrder = 0;
            //Append to the end of the published lessons sequence list
            if ((!request.LessonSequenceOrder.HasValue || request.LessonSequenceOrder.Value > publishedCourseLessonCount) && publishedCourseLessonCount < 2)
            {
                setLessonSequenceOrder = 1;
            }
            //Insert into a specified position in the list if the position is less than the current max position, and the lesson is not already in the list (archived)
            else if (!courseLesson.LessonSequenceOrder.HasValue && request.LessonSequenceOrder.HasValue && request.LessonSequenceOrder.Value <= publishedCourseLessonCount)
            {

                Console.WriteLine("Inserting the lesson into a specified position in the list, after archiving the lesson");
                var requestedLessonPosition = request.LessonSequenceOrder.Value;
                var lessonsToUpdateCount = await _dataContext.CourseLesson.Where(x => x.CourseId == courseLesson.CourseId && x.LessonSequenceOrder > requestedLessonPosition).CountAsync();

                foreach (var lesson in _dataContext.CourseLesson.Where(x => x.CourseId == courseLesson.CourseId && x.LessonSequenceOrder >= requestedLessonPosition))
                {
                    Console.WriteLine($"Lesson {lesson.Title} - {lesson.LessonSequenceOrder} -> {lesson.LessonSequenceOrder + 1}");
                    lesson.LessonSequenceOrder++;

                }
                setLessonSequenceOrder = request.LessonSequenceOrder.Value;
                Console.WriteLine($"Lessons to move count: {lessonsToUpdateCount}");
                await _dataContext.SaveChangesAsync();
            }
            //Insert at a specific position and update other lessons sequence order accordingly
            else if (courseLesson.LessonSequenceOrder.HasValue && request.LessonSequenceOrder.HasValue && request.LessonSequenceOrder.Value < publishedCourseLessonCount)
            {
                Console.WriteLine("Incrementing lessons sequence order");
                setLessonSequenceOrder = request.LessonSequenceOrder.Value;
                var currentPosition = courseLesson.LessonSequenceOrder;

                Console.WriteLine($"Current position: {currentPosition}");
                Console.WriteLine($"Set position: {setLessonSequenceOrder}");

                if (currentPosition < setLessonSequenceOrder)
                {
                    foreach (var lesson in _dataContext.CourseLesson.Where(x => x.LessonSequenceOrder > currentPosition && x.LessonSequenceOrder <= setLessonSequenceOrder))
                    {
                        Console.WriteLine($"Lesson {lesson.Title} - {lesson.LessonSequenceOrder} -> {lesson.LessonSequenceOrder - 1}");
                        lesson.LessonSequenceOrder--;
                    }
                }
                if (currentPosition > setLessonSequenceOrder)
                {
                    foreach (var lesson in _dataContext.CourseLesson.Where(x => x.LessonSequenceOrder >= setLessonSequenceOrder && x.LessonSequenceOrder < currentPosition))
                    {
                        Console.WriteLine($"Lesson {lesson.Title} - {lesson.LessonSequenceOrder} -> {lesson.LessonSequenceOrder + 1}");
                        lesson.LessonSequenceOrder++;
                    }
                }

                await _dataContext.SaveChangesAsync();




            }
            else if (!courseLesson.LessonSequenceOrder.HasValue && request.LessonSequenceOrder.HasValue && request.LessonSequenceOrder.Value >= publishedCourseLessonCount)
            {
                Console.WriteLine("User tried adding archived lesson to a position larger than the current max position, moving the lesson to the end of the list");
                setLessonSequenceOrder = publishedCourseLessonCount + 1;

            }
            else
            {
                Console.WriteLine("User tried adding the lesson on a position larger than the current max position, moving the lesson to the end of the list");
                var maxValue = publishedLessons.Max(l => l.LessonSequenceOrder);
                setLessonSequenceOrder = (int)maxValue;

                var currentLessonPosition = courseLesson.LessonSequenceOrder;

                foreach (var lesson in _dataContext.CourseLesson.Where(x => x.CourseId == courseLesson.CourseId && x.LessonSequenceOrder > currentLessonPosition))
                {
                    Console.WriteLine($"Lesson {lesson.Title} - {lesson.LessonSequenceOrder} -> {lesson.LessonSequenceOrder - 1}");
                    lesson.LessonSequenceOrder--;

                }
                await _dataContext.SaveChangesAsync();
            }
            _dataContext.CourseLesson.Where(x => x.CourseLessonId == courseLesson.CourseLessonId).First().LessonSequenceOrder = setLessonSequenceOrder;
            courseLesson.PublishedStatus = PublishedStatus.Published;
            await _dataContext.SaveChangesAsync();





            return Ok(ApiResponse<object>.GetApiResponse(
                $"Lesson {courseLesson.Title} published on position {setLessonSequenceOrder} successfully",
                null
            ));






        }

        [HttpPatch("lesson/archive")]
        public async Task<IActionResult> ArchiveCourseLesson([FromQuery] Guid courseLessonId)
        {
            var courseLesson = await _courseRepository.GetCourseLessonById(courseLessonId);
            if (courseLesson == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Lesson not found",
                        null
                    )
                );
            }

            if (courseLesson.PublishedStatus == PublishedStatus.Archived)
            {
                return Conflict(
                    ApiResponse<object>.GetApiResponse(
                        "Lesson already archived",
                        null
                    )
                );
            }

            if (courseLesson.PublishedStatus == PublishedStatus.Draft)
            {
                return Conflict(
                    ApiResponse<object>.GetApiResponse(
                        "Lesson to be archived must be published first",
                        null
                    )
                );
            }

            var course = await _courseRepository.GetCourseById(courseLesson.CourseId);

            //Check if the resource if owned by the course tutor
            var tutor = await _tutorRepository.GetTutorByPersonId(Guid.Parse(HttpContext.Items["PersonId"].ToString()));


            if (tutor == null)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while archiving the lesson, please refer to your administrator, regarding your role",
                        null
                    )
                );
            }

            if (tutor == null || course.TutorId != tutor.TutorId)
            {
                return StatusCode(
                    403,
                    ApiResponse<object>.GetApiResponse(
                        "You are not authorized to archive this lesson from this course",
                        null
                    )
                );
            }

            var maxValue = await _dataContext.CourseLesson.Where(x => x.CourseId == course.CourseId).MaxAsync(x => x.LessonSequenceOrder);
            var currentPosition = courseLesson.LessonSequenceOrder;

            if (courseLesson.LessonSequenceOrder.Value < maxValue)
            {
                foreach (var lesson in _dataContext.CourseLesson.Where(x => x.LessonSequenceOrder > currentPosition))
                {
                    Console.WriteLine($"Lesson {lesson.Title} - {lesson.LessonSequenceOrder} -> {lesson.LessonSequenceOrder - 1}");
                    lesson.LessonSequenceOrder--;
                }
            }

            _dataContext.CourseLesson.Where(x => x.CourseLessonId == courseLessonId).FirstOrDefault().PublishedStatus = PublishedStatus.Archived;
            _dataContext.CourseLesson.Where(x => x.CourseLessonId == courseLessonId).FirstOrDefault().LessonSequenceOrder = null;
            await _dataContext.SaveChangesAsync();


            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "Lesson archived successfully",
                    null
                )
            );

        }


        [HttpGet("lesson/all")]
        public async Task<IActionResult> GetAllCourseLessons([FromQuery] Guid courseId)
        {
            var courseExists = await _courseRepository.CourseExistsById(courseId);

            if (!courseExists)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Course not found",
                        null
                    )
                );

            }

            var lessons = await _courseRepository.GetAllCourseLessons(courseId);

            if (lessons.Count == 0)
            {
                return NoContent();
            }

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "Lessons retrieved successfully",
                    lessons
                )
            );




        }

        [HttpGet("lesson")]
        public async Task<IActionResult> GetCourseLessonById([FromQuery] Guid courseLessonId)
        {
            var lesson = await _courseRepository.GetCourseLessonByIdForTutorDashboard(courseLessonId);

            if (lesson == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Lesson not found",
                        null
                    )
                );
            }

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "Lesson retrieved successfully",
                    lesson
                )
            );

        }

        [HttpPost("lesson/resource/upload")]
        public async Task<IActionResult> UploadCourseLessonResource(UploadCourseLessonResourceRequest request)
        {

            //Check if it is a resource update or create operation, determined by the presence or the absence of the resourceId and the courseLessonId (resourceId is null and courseLessonId is not null - create operation, otherwise update operation)

            //Check if the resource exists (update operation)
            if (request.CourseLessonResourceId.HasValue && !await _courseRepository.CourseLessonResourceExists(request.CourseLessonResourceId.Value))
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Resource not found",
                        null
                    )
                );
            }

            Console.WriteLine($"Check for the following lesson ID: {request.CourseLessonId ?? Guid.Empty}");
            Console.WriteLine($"Above lesson with given ID exists: {await _courseRepository.CourseLessonExistsById(request.CourseLessonId.HasValue ? request.CourseLessonId.Value : Guid.Empty)}");
            //Check if the lesson exists (create operation)
            if (request.CourseLessonId.HasValue && !await _courseRepository.CourseLessonExistsById(request.CourseLessonId.Value))
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Lesson not found",
                        null
                    )
                );
            }

            //Check if both the ResourceUrl and ResourceFile fields are provided (only one should be provided)

            if (!string.IsNullOrEmpty(request.ResourceUrl) && request.ResourceFile != null)
            {
                return BadRequest(
                    ApiResponse<object>.GetApiResponse("A single upload request can only contain either a file or an url for upload", null)
                );
            }

            //Check if both of the ResourceUrl and ResourceFile fields are empty
            if (string.IsNullOrEmpty(request.ResourceUrl) && request.ResourceFile == null)
            {
                return BadRequest(
                    ApiResponse<object>.GetApiResponse("Resource to upload must be provided", null)
                );
            }

            //Check if the ResourceFile is provided, validate file type and size
            if (request.ResourceFile != null)
            {
                long fileSize = request.ResourceFile.Length;
                string fileType = request.ResourceFile.ContentType;

                var fileCategoryMap = new Dictionary<string[], long>
                {
                    { AllowedFileTypes.Documents, MaxFileTypesSizes.MaxDocumentSizeInBytes},
                    { AllowedFileTypes.Archives, MaxFileTypesSizes.MaxArchiveSizeInBytes},
                    { AllowedFileTypes.Audio, MaxFileTypesSizes.MaxAudioSizeInBytes},
                    { AllowedFileTypes.Images, MaxFileTypesSizes.MaxImageSizeInBytes},
                    { AllowedFileTypes.Videos, MaxFileTypesSizes.MaxVideoSizeInBytes}
                };

                bool isValidFile = false;

                foreach (var entry in fileCategoryMap)
                {
                    if (entry.Key.Contains(fileType))
                    {
                        if (fileSize > entry.Value)
                        {
                            return BadRequest(ApiResponse<object>.GetApiResponse(
                                $"File size exceeds the maximum allowed limit of {entry.Value / (1024 * 1024)}MB for this file type", null));
                        }
                        isValidFile = true;
                        break; // No need to check other categories once a match is found
                    }
                }

                if (!isValidFile)
                {
                    return BadRequest(ApiResponse<object>.GetApiResponse("Invalid file type", null));
                }

            }

            if (request.CourseLessonResourceId.HasValue)
            {
                CourseLessonResource? existingResource = await _courseRepository.GetCourseLessonResourceById(request.CourseLessonResourceId.Value);

                if (existingResource == null)
                {
                    return NotFound(
                        ApiResponse<object>.GetApiResponse(
                            "Resource not found",
                            null
                        )
                    );
                }

                existingResource.Title = request.Title;
                existingResource.Description = request.Description;
                existingResource.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                if (!string.IsNullOrEmpty(request.ResourceUrl))
                {
                    existingResource.ResourceUrl = request.ResourceUrl;
                    existingResource.FileData = null;
                    existingResource.FileName = null;
                    existingResource.ContentType = null;
                    existingResource.FileSize = null;
                }
                else
                {
                    using var memoryStream = new MemoryStream();
                    await request.ResourceFile.CopyToAsync(memoryStream);
                    existingResource.FileData = memoryStream.ToArray();
                    existingResource.FileName = request.ResourceFile.FileName;
                    existingResource.ContentType = request.ResourceFile.ContentType;
                    existingResource.FileSize = request.ResourceFile.Length;
                    existingResource.ResourceUrl = null;
                }

                var updateResult = await _courseRepository.UpdateCourseLessonResource(existingResource);

                if (!updateResult)
                {
                    return StatusCode(500, ApiResponse<object>.GetApiResponse("Failed to update resource, please try again later", null));
                }

                return Ok(
                    ApiResponse<object>.GetApiResponse("Resource updated successfully", null)
                );
            }

            byte[] tempFileData = null;
            if (request.ResourceFile != null)
            {
                using var memoryStream = new MemoryStream();
                await request.ResourceFile.CopyToAsync(memoryStream);
                tempFileData = memoryStream.ToArray();
            }

            var newResource = new CourseLessonResource
            {
                CourseLessonId = request.CourseLessonId.Value,
                Title = request.Title,
                Description = request.Description,
                ResourceUrl = request.ResourceUrl,
                FileData = tempFileData,
                FileName = request.ResourceFile?.FileName,
                ContentType = request.ResourceFile?.ContentType,
                FileSize = request.ResourceFile?.Length,
            };

            var createResult = await _courseRepository.CreateCourseLessonResource(newResource);

            if (!createResult)
            {
                return StatusCode(500, ApiResponse<object>.GetApiResponse("Failed to create resource, please try again later", null));
            }

            return Ok(
                ApiResponse<object>.GetApiResponse("Resource created successfully", null)
            );
        }


        [HttpGet("lesson/resource/all")]
        public async Task<IActionResult> GetAllCourseLessonResources([FromQuery] Guid courseLessonId)
        {
            if (courseLessonId == Guid.Empty)
            {
                return BadRequest(ApiResponse<object>.GetApiResponse("Invalid course lesson ID", null));
            }

            var resources = await _courseRepository.GetAllCourseLessonResourcesByCourseLessonId(courseLessonId);


            return Ok(
                ApiResponse<object>.GetApiResponse("Resources retrieved successfully", resources)
            );

        }

        [HttpGet("lesson/resource")]
        public async Task<IActionResult> GetCourseLessonResourceById([FromQuery] Guid courseLessonResourceId)
        {
            var resource = await _courseRepository.GetCourseLessonResourceByIdWithoutFileData(courseLessonResourceId);

            if (resource == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Resource not found",
                        null
                    )
                );
            }

            return Ok(
                ApiResponse<GetCourseLessonResourceWithoutFileDataByIdResponse>.GetApiResponse("Resource retrieved successfully", resource)
            );
        }

        [HttpGet("lesson/resource/download")]
        public async Task<IActionResult> DownloadCourseLessonResource([FromQuery] Guid courseLessonResourceId)
        {
            var resourceFile = await _courseRepository.GetCourseLessonResourceById(courseLessonResourceId);

            if (resourceFile == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Resource not found",
                        null
                    )
                );
            }

            if (resourceFile.FileData == null)
            {
                return Conflict(ApiResponse<object>.GetApiResponse("Only files hosted on EduConnect server's can be downloaded", null));
            }

            return File(resourceFile.FileData, resourceFile.ContentType, resourceFile.FileName);
        }

        [HttpDelete("lesson/resource")]
        public async Task<IActionResult> DeleteCourseLessonResource([FromQuery] Guid courseLessonResourceId)
        {
            var resource = await _courseRepository.GetCourseLessonResourceByIdWithoutFileData(courseLessonResourceId);

            if (resource == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Resource not found",
                        null
                    )
                );
            }

            var tutor = await _tutorRepository.GetTutorByPersonId(Guid.Parse(HttpContext.Items["PersonId"].ToString()));


            if (tutor == null)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while deleting the thumbnail, please refer to your administrator, regarding your role",
                        null
                    )
                );
            }

            var resourceOwner = await _courseRepository.GetCourseLessonTutorByCourseLessonId(resource.CourseLessonId);

            if (resourceOwner == null)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while deleting the resource, please refer to your administrator, regarding your role",
                        null
                    )
                );
            }

            if (tutor.TutorId != resourceOwner)
            {
                return StatusCode(
                    403,
                    ApiResponse<object>.GetApiResponse(
                        "You are not authorized to delete this thumbnail",
                        null
                    )
                );
            }

            var deleteResult = await _courseRepository.DeleteCourseLessonResourceById(courseLessonResourceId);

            if (!deleteResult)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while deleting the resource, please try again later",
                        null
                    )
                );
            }

            return Ok(
                ApiResponse<object>.GetApiResponse("Resource deleted successfully", null)
            );

        }

        [HttpPost("promotion/image/upload")]
        public async Task<IActionResult> UploadPromotionImage(UploadPromotionImageRequest request)
        {
            if (request.Image == null || request.Image.Length == 0)
            {
                return BadRequest(ApiResponse<object>.GetApiResponse("Invalid image file", null));
            }

            const int maxFileSize = 5 * 1024 * 1024; // 5MB
            //Check file size 
            if (request.Image.Length > maxFileSize)
            {
                return BadRequest(
                    ApiResponse<object>.GetApiResponse(
                        "Promotion image maximum file size is 5MB, this file has a size of " + request.Image.Length + " bytes",
                        null
                    )
                );
            }

            //Check if create or update operation
            //Create - request.CourseId is not null and request.PromotionId is null
            //Update - request.PromotionId is not null
            if (request.CourseId == null && request.CoursePromotionImageId == null)
            {
                return BadRequest(
                    ApiResponse<object>.GetApiResponse(
                        "Invalid request, either CourseId or CoursePromotionImageId must be provided",
                        null
                    )
                );
            }
            if ((request.CourseId.HasValue && request.CourseId.Value == Guid.Empty) || (request.CoursePromotionImageId.HasValue && request.CoursePromotionImageId.Value == Guid.Empty))
            {
                return BadRequest(
                    ApiResponse<object>.GetApiResponse(
                        "Invalid request, both CourseId and CoursePromotionImageId cannot be empty",
                        null
                    )
                );
            }

            var personId = Guid.Parse(HttpContext.Items["PersonId"].ToString());

            var tutor = await _tutorRepository.GetTutorByPersonId(personId);

            if (tutor == null)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while getting the courses, please refer to your administrator, regarding your role",
                        null
                    )
                );
            }

            //Since the request CoursePromotionImageId is not null, promotion image is being updated
            if (request.CoursePromotionImageId.HasValue)
            {
                var existingPromotionImage = await _courseRepository.GetCoursePromotionImageById(request.CoursePromotionImageId.Value);

                if (existingPromotionImage == null)
                {
                    return NotFound(
                        ApiResponse<object>.GetApiResponse(
                            "Promotion image not found",
                            null
                        )
                    );
                }


                if (existingPromotionImage.Course.TutorId != tutor.TutorId)
                {
                    return Conflict(
                        ApiResponse<object>.GetApiResponse(
                            "You are not authorized to update this course",
                            null
                        )
                    );
                }

                using var memoryStream1 = new MemoryStream();
                await request.Image.CopyToAsync(memoryStream1);
                byte[] promotionImageFile1 = memoryStream1.ToArray();

                existingPromotionImage.ContentType = request.Image.ContentType;
                existingPromotionImage.ImageFile = promotionImageFile1;
                existingPromotionImage.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                var updateResult = await _courseRepository.UpdateCoursePromotionImage(existingPromotionImage);
                if (!updateResult)
                {
                    return StatusCode(
                        500,
                        ApiResponse<object>.GetApiResponse(
                            "An error occurred while updating the promotion image, please try again later",
                            null
                        )
                    );
                }

                return Ok(
                    ApiResponse<object>.GetApiResponse(
                        "Promotion image updated successfully",
                        null
                    )
                );


            }

            //Check if the course exists
            var course = await _courseRepository.GetCourseById(request.CourseId.Value);
            if (course == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Course not found",
                        null
                    )
                );
            }



            if (course.TutorId != tutor.TutorId)
            {
                return Conflict(
                    ApiResponse<object>.GetApiResponse(
                        "You are not authorized to update this course",
                        null
                    )
                );
            }


            //Check if file type is image
            if (!request.Image.ContentType.StartsWith("image/"))
            {
                return BadRequest(
                    ApiResponse<object>.GetApiResponse(
                        "Promotion image must be an image",
                        null
                    )
                );
            }

            //Check the number of promotion images for the course (default maximum is 5)
            var promotionImagesCount = await _courseRepository.GetPromotionImageCountByCourseId(course.CourseId);

            if (promotionImagesCount >= 5)
            {
                return BadRequest(
                    ApiResponse<object>.GetApiResponse(
                        "You have reached the maximum number of promotion images for this course",
                        null
                    )
                );
            }

            using var memoryStream = new MemoryStream();
            await request.Image.CopyToAsync(memoryStream);
            byte[] promotionImageFile = memoryStream.ToArray();
            var coursePromotionImage = new CoursePromotionImage
            {
                CoursePromotionImageId = Guid.NewGuid(),
                CourseId = request.CourseId.Value,
                ContentType = request.Image.ContentType,
                ImageFile = promotionImageFile,


            };
            var createResult = await _courseRepository.CreateCoursePromotionImage(coursePromotionImage);

            if (!createResult)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while creating the uploading the image, please try again later",
                        null
                    )
                );
            }
            return Ok(
                ApiResponse<Guid>.GetApiResponse("Promotion image uploaded successfully", coursePromotionImage.CoursePromotionImageId)
            );




        }

        [HttpDelete("promotion/image/delete/{coursePromotionImageId}")]
        public async Task<IActionResult> DeletePromotionImage(Guid coursePromotionImageId)
        {

            var imageExists = await _courseRepository.CheckCoursePromotionImageExists(coursePromotionImageId);

            if (!imageExists)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Promotion image not found",
                        null
                    )
                );
            }

            var personId = Guid.Parse(HttpContext.Items["PersonId"].ToString());

            var tutor = await _tutorRepository.GetTutorByPersonId(personId);

            if (tutor == null)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while getting the courses, please refer to your administrator, regarding your role",
                        null
                    )
                );
            }

            var isTutorOwner = await _courseRepository.IsTutorCoursePromotionImageOwner(coursePromotionImageId, tutor.TutorId);

            if (!isTutorOwner)
            {
                return StatusCode(
                    403,
                    ApiResponse<object>.GetApiResponse(
                        "You are not authorized to delete this promotion image",
                        null
                    )
                );
            }

            var deleteResult = await _courseRepository.DeleteCoursePromotionImageById(coursePromotionImageId);

            if (!deleteResult)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while deleting the promotion image, please try again later",
                        null
                    )
                );
            }

            return Ok(
                ApiResponse<object>.GetApiResponse("Promotion image deleted successfully", null)
            );

        }


        [HttpGet("promotion/image/all")]
        public async Task<IActionResult> GetPromotionImages([FromQuery] Guid courseId)
        {
            var courseExists = await _courseRepository.CourseExistsById(courseId);

            if (!courseExists)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Course not found",
                        null
                    )
                );
            }

            var coursePromotionImagesMetadata = await _courseRepository.GetCoursePromotionImagesMetadataByCourseId(courseId);

            return Ok(
                ApiResponse<List<GetCoursePromotionImagesMetadataResponse>>.GetApiResponse("Promotion images retrieved successfully", coursePromotionImagesMetadata)
            );



        }

        [HttpGet("promotion/image/{coursePromotionImageId}")]
        public async Task<IActionResult> GetCoursePromotionImageById(Guid coursePromotionImageId)
        {
            var image = await _courseRepository.GetCoursePromotionImageMetadataById(coursePromotionImageId);

            if (image == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Promotion image not found",
                        null
                    )
                );
            }

            return Ok(
                ApiResponse<GetCoursePromotionImageMetadataByIdResponse?>.GetApiResponse("Promotion image retrieved successfully", image)
            );
        }

        [HttpGet("analytics/history")]
        public async Task<IActionResult> GetCourseAnalyticsHistory([FromQuery] Guid courseId)
        {
            var courseExists = await _courseRepository.CourseExistsById(courseId);

            if (!courseExists)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Course not found",
                        null
                    )
                );
            }

            var analyticsHistory = await _courseRepository.GetCourseAnalyticsHistory(courseId);
            (int usersCameFromFeedCount, int usersCameFromSearchCount) = await _courseRepository.GetCourseUsersCameFromCounts(courseId);
            return Ok(
                ApiResponse<GetCourseAnalyticsHistoryControllerResponse>.GetApiResponse("Analytics history retrieved successfully", new GetCourseAnalyticsHistoryControllerResponse
                {
                    CourseAnalyticsHistory = analyticsHistory,
                    UsersCameFromFeedCount = usersCameFromFeedCount,
                    UsersCameFromSearchCount = usersCameFromSearchCount
                })
            );
        }
    }



}
