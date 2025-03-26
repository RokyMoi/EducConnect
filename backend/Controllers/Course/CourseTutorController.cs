using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Interfaces.Reference;
using backend.Interfaces.Tutor;
using EduConnect.Constants;
using EduConnect.DTOs;
using EduConnect.DTOs.Course;
using EduConnect.Entities;
using EduConnect.Entities.Course;
using EduConnect.Interfaces.Course;
using EduConnect.Middleware;
using EduConnect.Services;
using EduConnect.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace EduConnect.Controllers.Course
{
    [ApiController]
    [Route("tutor/course")]
    [AuthenticationGuard(isTutor: true, isAdmin: false, isStudent: false)]
    public class CourseTutorController(
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
                PublishedStatus = false,
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
                    ThumbnailUrl = thumbnailUrl
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
                TwoLatestAddedTeachingResources = teachingResourcesInformation == null ? [] : teachingResourcesInformation.TwoLatestAddedTeachingResources

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
            var resourceFile = await _courseRepository.GetCourseTeachingResourceById(courseTeachingResourceId);

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





    }
}