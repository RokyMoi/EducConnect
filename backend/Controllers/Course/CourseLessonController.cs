using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using backend.Interfaces.Course;
using backend.Interfaces.Person;
using backend.Interfaces.Reference;
using backend.Interfaces.Tutor;
using backend.Middleware;
using EduConnect.DTOs.Course.CourseLesson;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace backend.Controllers.Course
{
    [ApiController]
    [Route("course/lesson")]
    public class CourseLessonController(IPersonRepository _personRepository, ITutorRepository _tutorRepository, ICourseRepository _courseRepository, IReferenceRepository _referenceRepository) : ControllerBase
    {
        [HttpPost("create")]
        [CheckPersonLoginSignup]
        public async Task<IActionResult> CreateCourseLesson(CourseLessonSaveRequestDTO saveRequestDTO)
        {

            //Check if the CourseId from the saveRequest is equal to Guid.Empty
            if (saveRequestDTO.CourseId == Guid.Empty)
            {
                return BadRequest(
                    new
                    {
                        success = "error",
                        message = "CourseId is required.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            Console.WriteLine("HttpContext email: " + HttpContext.Items["Email"].ToString());

            //Check if the email in the context dictionary is null
            if (string.IsNullOrEmpty(HttpContext.Items["Email"].ToString()))
            {
                return StatusCode(
                    500,
                    new
                    {
                        success = "error",
                        message = "Something went wrong, please try again later.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            string email = HttpContext.Items["Email"].ToString();

            Guid personId = Guid.Parse(HttpContext.Items["PersonId"].ToString());
            //Check if the PersonId from dictionary is null and if it is, call to the database to get the PersonId
            if (string.IsNullOrEmpty(HttpContext.Items["PersonId"].ToString()))
            {
                var personEmail = await _personRepository.GetPersonEmailByEmail(email);
                personId = personEmail.PersonId;
            }


            //Check if the PersonId is Tutor and if it is, check the TutorRegistrationStatus

            var tutor = await _tutorRepository.GetTutorRegistrationStatusByPersonId(personId);
            Console.WriteLine("Tutor Id: " + tutor.PersonId);

            if (tutor == null)
            {
                return Unauthorized(new
                {

                    success = "false",
                    message = "You must be a tutor to create a course.",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }


            //If tutor is not null, check the TutorRegistrationStatus is below 10 (Completed Registration)
            if (tutor != null && tutor.TutorRegistrationStatusId < 10)
            {
                return UnprocessableEntity(
                    new
                    {

                        success = "false",
                        message = "You must complete your registration first, to be able to create a course.",
                        data = new
                        {
                            CurrentTutorRegistrationStatus = new
                            {
                                TutorRegistrationStatusId = tutor.TutorRegistrationStatusId,

                            }
                        },
                        timestamp = DateTime.Now

                    }
                );
            }

            //Check if the Course exists
            var course = await _courseRepository.GetCourseById(saveRequestDTO.CourseId);

            if (course == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "Course not found.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the Course is owned by the Tutor
            if (tutor.TutorId != course.TutorId)
            {
                return StatusCode(
                    403,
                    new
                    {
                        success = "false",
                        message = "You cannot add a lesson to a course that is not yours.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the lesson title is already in the course 
            var lessonTitle = await _courseRepository.CheckIfLessonTitleExistsByCourseIdAndLessonTitle(saveRequestDTO.CourseId, saveRequestDTO.LessonTitle);

            if (lessonTitle)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Lesson with the same title already exists in the course.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Reorder the lessons in the course to make room for the new lesson
            //Get the highest lesson sequence order in the course
            var highestLessonSequenceOrder = await _courseRepository.GetHighestLessonSequenceOrderByCourseId(saveRequestDTO.CourseId);

            //If no records in the CourseLesson table exist, set the lesson sequence order to 0, because the lesson to be create is the first lesson in sequence
            if (highestLessonSequenceOrder == null)
            {
                saveRequestDTO.LessonSequenceOrder = 1;
            }

            //If there was a record in the CourseLesson table, reorder the lessons in the course to make room for the new lesson
            if (highestLessonSequenceOrder != null)
            {
                // If there are existing lessons
                int desiredOrder = saveRequestDTO.LessonSequenceOrder;

                // If desired order is greater than highest + 1, append to end
                if (desiredOrder > highestLessonSequenceOrder + 1)
                {
                    saveRequestDTO.LessonSequenceOrder = (int)highestLessonSequenceOrder + 1;
                }

                // If desired order is within valid range, shift existing lessons
                else if (desiredOrder >= 1 && desiredOrder <= highestLessonSequenceOrder)
                {
                    // Increment sequence order for all lessons >= desired position
                    await _courseRepository.IncrementLessonSequenceOrders(
                        saveRequestDTO.CourseId,
                        desiredOrder,
                        (int)highestLessonSequenceOrder
                    );
                    saveRequestDTO.LessonSequenceOrder = desiredOrder;
                }
                // If desired order is less than 1, place at beginning
                else
                {
                    await _courseRepository.IncrementAllLessonSequenceOrders(saveRequestDTO.CourseId);
                    saveRequestDTO.LessonSequenceOrder = 1;
                }






            }

            //Check if the LessonCompletionTimeInMinutes is less than 5 minute
            if (saveRequestDTO.LessonCompletionTimeInMinutes < 5)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Lesson completion time must be at least 5 minutes.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }



            // Create new lesson with final sequence order
            var newLesson = new CourseLessonDTO
            {
                CourseId = saveRequestDTO.CourseId,
                CourseLessonId = new Guid(),
                LessonTitle = saveRequestDTO.LessonTitle,
                LessonDescription = saveRequestDTO.LessonDescription,
                LessonSequenceOrder = saveRequestDTO.LessonSequenceOrder,
                LessonPrerequisites = saveRequestDTO.LessonPrerequisites,
                LessonObjective = saveRequestDTO.LessonObjective,
                LessonCompletionTimeInMinutes = saveRequestDTO.LessonCompletionTimeInMinutes,
                LessonTag = saveRequestDTO.LessonTag,
            };

            // Save the new lesson
            var saveResult = await _courseRepository.CreateCourseLesson(newLesson);
            if (saveResult == null)
            {
                return StatusCode(
                    500,
                    new
                    {
                        success = "false",
                        message = "Failed to create lesson.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }


            return Ok(
                new
                {
                    success = "true",
                    message = "Lesson created successfully.",
                    data = new
                    {
                        courseLesson = new
                        {
                            courseLessonId = saveResult.CourseLessonId,
                            courseId = saveResult.CourseId,
                            lessonTitle = saveResult.LessonTitle,
                            lessonDescription = saveResult.LessonDescription,
                            lessonSequenceOrder = saveResult.LessonSequenceOrder,
                            lessonPrerequisites = saveResult.LessonPrerequisites,
                            lessonObjective = saveResult.LessonObjective,
                            lessonCompletionTimeInMinutes = saveResult.LessonCompletionTimeInMinutes,
                            lessonTag = saveResult.LessonTag
                        }
                    },
                    timestamp = DateTime.Now
                }
            );
        }

        [HttpPost("content")]
        [CheckPersonLoginSignup]
        public async Task<IActionResult> CreateCourseContent(CreateCourseLessonContentSaveRequestDTO saveRequestDTO)
        {

            //Check if the CourseLessonId is not a Guid.Empty
            if (saveRequestDTO.CourseLessonId == Guid.Empty)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "CourseLessonId is required",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            Console.WriteLine("HttpContext email: " + HttpContext.Items["Email"].ToString());

            //Check if the email in the context dictionary is null
            if (string.IsNullOrEmpty(HttpContext.Items["Email"].ToString()))
            {
                return StatusCode(
                    500,
                    new
                    {
                        success = "error",
                        message = "Something went wrong, please try again later.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            string email = HttpContext.Items["Email"].ToString();

            Guid personId = Guid.Parse(HttpContext.Items["PersonId"].ToString());
            //Check if the PersonId from dictionary is null and if it is, call to the database to get the PersonId
            if (string.IsNullOrEmpty(HttpContext.Items["PersonId"].ToString()))
            {
                var personEmail = await _personRepository.GetPersonEmailByEmail(email);
                personId = personEmail.PersonId;
            }


            //Check if the PersonId is Tutor and if it is, check the TutorRegistrationStatus

            var tutor = await _tutorRepository.GetTutorRegistrationStatusByPersonId(personId);
            Console.WriteLine("Tutor Id: " + tutor.PersonId);

            if (tutor == null)
            {
                return Unauthorized(new
                {

                    success = "false",
                    message = "You must be a tutor to create a course.",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }


            //If tutor is not null, check the TutorRegistrationStatus is below 10 (Completed Registration)
            if (tutor != null && tutor.TutorRegistrationStatusId < 10)
            {
                return UnprocessableEntity(
                    new
                    {

                        success = "false",
                        message = "You must complete your registration first, to be able to create a course.",
                        data = new
                        {
                            CurrentTutorRegistrationStatus = new
                            {
                                TutorRegistrationStatusId = tutor.TutorRegistrationStatusId,

                            }
                        },
                        timestamp = DateTime.Now

                    }
                );
            }

            //Check if the CourseLessonId exists
            var courseLesson = await _courseRepository.GetCourseLessonWithCourseByCourseLessonId(saveRequestDTO.CourseLessonId);

            if (courseLesson == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "Course lesson not found",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the logged in tutor (tutor.TutorId) is the same as the CourseLesson.Course.TutorId

            Console.WriteLine("CourseLesson.Course.TutorId: " + courseLesson.Course.TutorId);
            if (courseLesson.Course.TutorId != tutor.TutorId)
            {
                return StatusCode(
                    403,
                    new
                    {
                        success = "false",
                        message = "You are not authorized to create content for this course lesson.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the CourseLesson already has an associated CourseLessonContent
            var courseLessonContent = await _courseRepository.GetCourseLessonContentByCourseLessonId(saveRequestDTO.CourseLessonId);
            if (courseLessonContent != null)
            {
                return Conflict(
                    new
                    {
                        success = "false",
                        message = "Only one content instance per course lesson is allowed.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Save the CourseLessonContent
            var courseLessonContentToSave = new CourseLessonContentCreateDTO
            {
                CourseLessonId = saveRequestDTO.CourseLessonId,
                Title = saveRequestDTO.Title,
                Description = saveRequestDTO.Description,
                ContentType = saveRequestDTO.ContentType,
                ContentData = saveRequestDTO.ContentData,
            };

            var saveResult = await _courseRepository.CreateCourseCourseLessonContent(courseLessonContentToSave);

            if (saveResult == null)
            {
                return StatusCode(500,
                    new
                    {
                        success = "false",
                        message = "An error occurred while saving the course lesson content.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }


            return Ok(
                new
                {
                    success = "true",
                    message = "Course lesson content created successfully.",
                    data = new
                    {
                        CourseLessonContentId = saveResult.CourseLessonContentId,
                        CourseLessonId = saveResult.CourseLessonId,
                        Title = saveResult.Title,
                        Description = saveResult.Description,
                        ContentType = saveResult.ContentType,
                        ContentData = saveResult.ContentData,
                    },
                    timestamp = DateTime.Now
                }
            );
        }

        [HttpPost("supplementary-material")]
        [CheckPersonLoginSignup]
        public async Task<IActionResult> UploadCourseLessonSupplementaryMaterial(CourseLessonSupplementaryMaterialSaveRequestDTO saveRequestDTO)
        {
            //Check if the the CourseLessonId is not a Guid.Empty
            if (saveRequestDTO.CourseLessonId == Guid.Empty)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Course lesson ID is required.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the DateTimePointOfFileCreation is in the future
            if (saveRequestDTO.DateTimePointOfFileCreation > DateTimeOffset.Now.ToUnixTimeMilliseconds())
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "DateTimePointOfFileCreation cannot be in the future",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the FileToUpload is null or empty
            if (saveRequestDTO.FileToUpload == null || saveRequestDTO.FileToUpload.Length == 0)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "FileToUpload is required",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the FileToUpload file type is allowed

            //Define allowed file types for documents, images, videos, and archives
            var allowedDocumentFileTypes = new string[] {
"application/pdf",
                "application/msword",
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                "application/vnd.ms-powerpoint",
                "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                "application/vnd.ms-excel",
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "text/plain",
                "application/rtf",
            };

            var allowedImageFileTypes = new string[] {
"image/png",
                "image/jpeg",
                "image/jpg",
                "image/gif",
            };

            var allowedVideoFileTypes = new string[] {
                "video/mp4",
                "video/mpeg",
                "video/quicktime",
                "video/x-msvideo",
                "video/x-ms-wmv",
                "video/x-matroska",
            };

            var allowedArchiveFileTypes = new string[] {
                "application/zip",
                "application/x-rar-compressed",
                "application/x-rar",
            };

            //Define max file size in bytes per file type
            //Max image size in bytes is 5MB
            const long MaxImageSizeInBytes = 5 * 1024 * 1024;

            //Max document size in bytes is 10MB
            const long MaxDocumentSizeInBytes = 10 * 1024 * 1024;

            //Max video size in bytes is 100MB
            const long MaxVideoSizeInBytes = 100 * 1024 * 1024;

            //Max archive size in bytes is 50MB
            const long MaxArchiveSizeInBytes = 50 * 1024 * 1024;


            // Check the file type and size
            var fileType = saveRequestDTO.FileToUpload.ContentType;
            var fileSize = saveRequestDTO.FileToUpload.Length;

            // Check if file type is valid
            if (!(allowedDocumentFileTypes.Contains(fileType) ||
                  allowedImageFileTypes.Contains(fileType) ||
                  allowedVideoFileTypes.Contains(fileType) ||
                  allowedArchiveFileTypes.Contains(fileType)))
            {

                var fileExtension = string.Empty;
                if (saveRequestDTO.FileToUpload.ContentDisposition != null)
                {
                    var contentDisposition = ContentDispositionHeaderValue.Parse(saveRequestDTO.FileToUpload.ContentDisposition);
                    fileExtension = Path.GetExtension(
                        contentDisposition.FileName.ToString()
                    ).Trim('"');
                    Console.WriteLine("File extension: " + fileExtension);
                }

                fileExtension = fileExtension == string.Empty ? fileType : fileExtension;

                return BadRequest(
                    new
                    {
                        success = "false",
                        message = $"File type {fileExtension} is not allowed. Please upload a valid file.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check file size
            //Check if the file is type image and check if it is less than 5MB
            if (allowedImageFileTypes.Contains(fileType) && fileSize > MaxImageSizeInBytes)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = $"The image is too large. Please upload an image less than 5MB.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }


            //Check if the file is type document and check if it is less than 10MB
            if (allowedDocumentFileTypes.Contains(fileType) && fileSize > MaxDocumentSizeInBytes)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = $"The document is too large. Please upload a document less than 10MB.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the file is type video and check if it is less than 100MB
            if (allowedVideoFileTypes.Contains(fileType) && fileSize > MaxVideoSizeInBytes)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = $"The video is too large. Please upload a video less than 100MB in size.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the file is type archive and check if it is less than 50MB
            if (allowedArchiveFileTypes.Contains(fileType) && fileSize > MaxArchiveSizeInBytes)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = $"The archived folder is too large. Please upload a archived folder smaller than 50MB in size.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            string email = HttpContext.Items["Email"].ToString();

            Guid personId = Guid.Parse(HttpContext.Items["PersonId"].ToString());
            //Check if the PersonId from dictionary is null and if it is, call to the database to get the PersonId
            if (string.IsNullOrEmpty(HttpContext.Items["PersonId"].ToString()))
            {
                var personEmail = await _personRepository.GetPersonEmailByEmail(email);
                personId = personEmail.PersonId;
            }


            //Check if the PersonId is Tutor and if it is, check the TutorRegistrationStatus

            var tutor = await _tutorRepository.GetTutorRegistrationStatusByPersonId(personId);
            Console.WriteLine("Tutor Id: " + tutor.PersonId);

            if (tutor == null)
            {
                return Unauthorized(new
                {

                    success = "false",
                    message = "You must be a tutor to edit a course.",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }


            //If tutor is not null, check the TutorRegistrationStatus is below 10 (Completed Registration)
            if (tutor != null && tutor.TutorRegistrationStatusId < 10)
            {
                return UnprocessableEntity(
                    new
                    {

                        success = "false",
                        message = "You must complete your registration first, to be able to create a course.",
                        data = new
                        {
                            CurrentTutorRegistrationStatus = new
                            {
                                TutorRegistrationStatusId = tutor.TutorRegistrationStatusId,

                            }
                        },
                        timestamp = DateTime.Now

                    }
                );
            }


            //Check if the CourseLessonId exists
            var courseLesson = await _courseRepository.GetCourseLessonWithCourseByCourseLessonId(saveRequestDTO.CourseLessonId);
            if (courseLesson == null)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "Course lesson not found.",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            //Check if the Tutor is the owner of the Course to which the CourseLesson belongs
            if (courseLesson.Course.TutorId != tutor.TutorId)
            {
                return StatusCode(
                    403,
                    new
                    {
                        success = "false",
                        message = "You cannot add supplementary material to this course lesson.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Define the maximum size of the course lesson supplementary materials per course
            const long maxCourseLessonSupplementaryMaterialSize = 200 * 1024 * 1024; //200MB

            //Check if the total size of the course lesson supplementary materials together with the file to be uploaded is more than 200MB
            var courseLessonSupplementaryMaterialsSize = await _courseRepository.GetTotalFileSizeOfCourseLessonSupplementaryMaterialsByCourseLessonId(saveRequestDTO.CourseLessonId);

            if (courseLessonSupplementaryMaterialsSize + fileSize > maxCourseLessonSupplementaryMaterialSize)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "The total size of the other supplementary materials of the course lesson and the file to be uploaded cannot be more than 200MB.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the total number of the course lesson supplementary materials is more than 50
            var courseLessonSupplementaryMaterialsCount = await _courseRepository.GetCountOfCourseLessonSupplementaryMaterialsByCourseLessonId(saveRequestDTO.CourseLessonId);

            if (courseLessonSupplementaryMaterialsCount + 1 > 50)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "The number of supplementary materials of the course lesson cannot be more than 50.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Upload the file to the server 
            //Create new CourseLessonSupplementaryMaterialCreateDTO 
            var courseLessonSupplementaryMaterialToSave = new CourseLessonSupplementaryMaterialCreateDTO { };


            //Declare the Guid Id variable to store the Id of the newly created CourseLessonSupplementaryMaterial 
            var createdCourseLessonSupplementaryMaterialId = Guid.Empty;

            //Extract the file into a byte array
            using (var memoryStream = new MemoryStream())
            {

                await saveRequestDTO.FileToUpload.CopyToAsync(memoryStream);
                var fileData = memoryStream.ToArray();

                courseLessonSupplementaryMaterialToSave = new CourseLessonSupplementaryMaterialCreateDTO
                {
                    CourseLessonId = saveRequestDTO.CourseLessonId,
                    ContentType = saveRequestDTO.FileToUpload.ContentType,
                    ContentSize = saveRequestDTO.FileToUpload.Length,
                    FileName = saveRequestDTO.FileToUpload.FileName,
                    Data = fileData,
                    DateTimePointOfFileCreation = saveRequestDTO.DateTimePointOfFileCreation,
                };



                try
                {
                    var saveResult = await _courseRepository.StoreFileToCourseLessonSupplementaryMaterial(courseLessonSupplementaryMaterialToSave);
                    createdCourseLessonSupplementaryMaterialId = saveResult.CourseLessonSupplementaryMaterialId;
                }
                catch (System.Exception ex)
                {

                    Console.WriteLine("Error while saving the file to the server.");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    return StatusCode(
                        500,
                        new
                        {
                            success = "false",
                            message = "Error while saving the file to the server.",
                            data = new { },
                            timestamp = DateTime.Now
                        }
                    );
                }



            }

            return Ok(
                new
                {
                    success = "true",
                    message = "The file was successfully uploaded.",
                    data = new
                    {

                        CourseLessonSupplementaryMaterialId = createdCourseLessonSupplementaryMaterialId,
                        CourseLessonId = saveRequestDTO.CourseLessonId,
                        FileName = saveRequestDTO.FileToUpload.FileName,
                        ContentType = saveRequestDTO.FileToUpload.ContentType,
                        ContentSize = saveRequestDTO.FileToUpload.Length,
                        DateTimePointOfFileCreation = saveRequestDTO.DateTimePointOfFileCreation,

                    },
                    timestamp = DateTime.Now
                }
            );
        }




    }
}
