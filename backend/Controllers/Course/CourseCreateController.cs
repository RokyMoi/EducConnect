using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using backend.DTOs.Course.Basic;
using backend.DTOs.Course.CourseMainMaterial;
using backend.DTOs.Course.Language;
using backend.Interfaces.Course;
using backend.Interfaces.Person;
using backend.Interfaces.Reference;
using backend.Interfaces.Tutor;
using backend.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Course
{
    [ApiController]
    [Route("course/create")]
    public class CourseCreateController(IPersonRepository _personRepository, ITutorRepository _tutorRepository, ICourseRepository _courseRepository, IReferenceRepository _referenceRepository) : ControllerBase
    {

        [HttpPost("basic")]
        [CheckPersonLoginSignup]
        public async Task<IActionResult> CreateCourse(CourseBasicSaveRequestDTO saveRequestDTO)
        {

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

            //Data validation


            //Check if the Price field from the saveRequestDTO is negative numeric value 
            if (saveRequestDTO.Price < 0)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Set price to 0 for a free course, and above 0 for a paid course.",
                    }
                );
            }

            //Check if the LearningSubcategoryId is not Guid.Empty
            if (saveRequestDTO.LearningSubcategoryId == Guid.Empty)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Subcategory is required.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the LearningDifficultyId is not a negative numeric value
            if (saveRequestDTO.LearningDifficultyLevelId < 1)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Difficulty level does not exist.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the CourseTypeId is not a negative numeric value
            if (saveRequestDTO.CourseTypeId < 1)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Course type does not exist.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check the given database against the one in the database

            //Check if there is a course with the same name
            var courseNameDuplicate = await _courseRepository.GetCourseByCourseName(saveRequestDTO.CourseName);

            if (courseNameDuplicate != null)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Course with the same name already exists.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the LearningSubcategoryId exists in the database
            var learningSubcategory = await _referenceRepository.GetLearningSubcategoryByIdAsync(saveRequestDTO.LearningSubcategoryId);

            if (learningSubcategory == null)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Subcategory does not exist.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the LearningDifficultyId exists in the database

            var learningDifficulty = await _referenceRepository.GetLearningDifficultyLevelByIdAsync(saveRequestDTO.LearningDifficultyLevelId);

            if (learningDifficulty == null)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Difficulty level does not exist.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the CourseTypeId exists in the database
            var courseType = await _referenceRepository.GetCourseTypeByIdAsync(saveRequestDTO.CourseTypeId);

            if (courseType == null)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Course type does not exist.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Get the first step of the CourseCreationCompleteness
            var firstStep = await _referenceRepository.GetCourseCreationCompletenessStepDTOByStepOrderAsync(0);

            //Create Course object and save it to the database
            var newCourse = new CourseCreateDTO
            {
                TutorId = tutor.TutorId,
                CourseName = saveRequestDTO.CourseName,
                CourseSubject = saveRequestDTO.CourseSubject,
                IsDraft = saveRequestDTO.IsDraft,
                CourseCreationCompletenessStepId = firstStep.CourseCreationCompletenessStepId,
            };
            var createdCourse = await _courseRepository.CreateCourse(newCourse); ;

            if (createdCourse == null)
            {
                return StatusCode(
                    500,
                    new
                    {
                        success = "false",
                        message = "Failed to create course, please try again later.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Create CourseDetails object and save it to the database

            var newCourseDetails = new CourseDetailsCreateDTO
            {
                CourseId = createdCourse.CourseId,
                CourseDescription = saveRequestDTO.CourseDescription,
                Price = saveRequestDTO.Price,
                LearningSubcategoryId = saveRequestDTO.LearningSubcategoryId,
                LearningDifficultyLevelId = saveRequestDTO.LearningDifficultyLevelId,
                CourseTypeId = saveRequestDTO.CourseTypeId,
            };

            var createdCourseDetails = await _courseRepository.CreateCourseDetails(newCourseDetails);

            if (createdCourseDetails == null)
            {
                return StatusCode(
                    500,
                    new
                    {
                        success = "false",
                        message = "Failed to create course, please try again later.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }



            return Ok(
                new
                {
                    success = "true",
                    message = "Course created successfully.",
                    data = new
                    {
                        course = createdCourse,
                        courseDetails = createdCourseDetails
                    },
                    timestamp = DateTime.Now
                }
            );
        }

        [HttpPost("language")]
        [CheckPersonLoginSignup]
        public async Task<IActionResult> AddLanguageSupportToCourse(CourseLanguageSupportSaveRequestDTO saveRequestDTO)
        {

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
                    message = "You must be a tutor to edit a course.",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            //Check if the courseId from the saveRequestDTO is correct Guid
            if (saveRequestDTO.CourseId == Guid.Empty)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Course identification is required",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );

            }

            //Check if the languageId from the saveRequestDTO is correct Guid
            if (saveRequestDTO.LanguageId == Guid.Empty)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Language identification is required",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );

            }

            //Check if the course exists
            var course = await _courseRepository.GetCourseById(saveRequestDTO.CourseId);

            if (course == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "Course not found",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the logged in Tutor is an owner of the course (Course.TutorId equals tutor.TutorId)
            if (course.TutorId != tutor.TutorId)
            {
                return StatusCode(
                    403,
                    new
                    {
                        success = "false",
                        message = "You must be the owner of the course to edit it.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
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

            //Check if the languageId from the saveRequestDTO exists in the database table Languages
            var language = await _referenceRepository.GetLanguageByIdAsync(saveRequestDTO.LanguageId);

            if (language == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "Language not found",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the language is already supported by the course 
            var courseLanguage = await _courseRepository.GetCourseLanguageByCourseIdAndLanguageId(course.CourseId, saveRequestDTO.LanguageId);
            Console.WriteLine("Course Language: " + courseLanguage);
            if (courseLanguage != null)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "This language is already supported by the course.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Update the course with the new CourseCreationStep (Step 1)
            var courseUpdateStep = await _courseRepository.UpdateCourseCompletenessStepByCourseIdAndStepOrder(course.CourseId, 1);
            Console.WriteLine("Course Completeness Step: " + course.CourseCreationCompletenessStepId);
            if (courseUpdateStep == null)
            {
                return StatusCode(
                        500,
                        new
                        {
                            success = "false",
                            message = "An error occurred while updating the course completeness step.",
                            data = new { },
                            timestamp = DateTime.Now
                        }
                    );
            }

            //Attempt to add the language to the course
            try
            {
                await _courseRepository.CreateCourseLanguage(course.CourseId, saveRequestDTO.LanguageId);
            }
            catch (System.Exception)
            {

                return StatusCode(
                    500,
                    new
                    {
                        success = "false",
                        message = "An error occurred while adding the language to the course.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }
            return Ok(
                new
                {
                    success = "true",
                    message = "Language added to course successfully.",
                    data = new
                    {

                    },
                    timestamp = DateTime.Now
                }
            );

        }

        [HttpPost("main-materials")]
        [CheckPersonLoginSignup]
        public async Task<IActionResult> UploadCourseMainMaterial(CourseMainMaterialSaveRequestDTO saveRequestDTO)
        {

            //Check if the CourseId from the saveRequestDTO is not equal to the Guid.Empty
            if (saveRequestDTO.CourseId == Guid.Empty)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "CourseId is required",
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


            //Check if the course with the CourseId exists
            var course = await _courseRepository.GetCourseById(saveRequestDTO.CourseId);
            if (course == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "The course does not exist.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the tutor is the owner of the course
            if (course.TutorId != tutor.TutorId)
            {
                return StatusCode(
                    403,
                    new
                    {
                        success = "false",
                        message = "You are not the owner of this course.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Define the maximum size of the course main materials per course
            const long maxCourseMainMaterialSize = 120 * 1024 * 1024; //120MB


            //Check if the total size of the course main materials together with the file to be uploaded is more than 120MB
            var courseMainMaterialSize = await _courseRepository.GetTotalFileSizeOfCourseMainMaterialByCourseId(course.CourseId);
            if (courseMainMaterialSize + fileSize > maxCourseMainMaterialSize)
            {
                return StatusCode(
                    413,
                    new
                    {
                        success = "false",
                        message = "Uploading this file, would exceed the maximum total size of the course main materials per course, which is 120MB, please try again with a smaller file size.",
                        data = new
                        {
                            currentCourseMainMaterialTotalFileSize = courseMainMaterialSize,
                        },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the maximum number of course main materials per course is reached (maximum number of materials per course is 15)
            var courseMainMaterialCount = await _courseRepository.GetCountOfCourseMainMaterialByCourseId(course.CourseId);
            if (courseMainMaterialCount + 1 > 15)
            {
                return StatusCode(
                    409,
                    new
                    {
                        success = "false",
                        message = "Uploading this file, would exceed the maximum total size of the number of main materials per course, which is 15 materials, please remove some files first.",
                        data = new
                        {
                            currentCourseMainMaterialCount = courseMainMaterialCount,
                        },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Create a new CourseMainMaterialDTO object
            var courseMainMaterialDTOForDatabase = new CourseMainMaterialDTO
            {

            };

            //Extract the file into a byte array
            using (var memoryStream = new MemoryStream())
            {
                await saveRequestDTO.FileToUpload.CopyToAsync(memoryStream);
                var fileData = memoryStream.ToArray();

                courseMainMaterialDTOForDatabase = new CourseMainMaterialDTO
                {
                    CourseMainMaterialId = Guid.NewGuid(),
                    CourseId = saveRequestDTO.CourseId,
                    FileName = saveRequestDTO.FileName,
                    ContentType = saveRequestDTO.FileToUpload.ContentType,
                    ContentSize = saveRequestDTO.FileToUpload.Length,
                    Data = fileData,
                    DateTimePointOfFileCreation = saveRequestDTO.DateTimePointOfFileCreation,

                };

                var saveResult = await _courseRepository.StoreFileToCourseMainMaterial(courseMainMaterialDTOForDatabase);

                if (saveResult == null)
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


            }


            return Ok(
                new
                {
                    success = "true",
                    message = "File uploaded successfully.",
                    data = new
                    {
                        CourseMainMaterialId = courseMainMaterialDTOForDatabase.CourseMainMaterialId,
                        CourseId = courseMainMaterialDTOForDatabase.CourseId,
                        FileName = courseMainMaterialDTOForDatabase.FileName,
                        ContentType = courseMainMaterialDTOForDatabase.ContentType,
                        ContentSize = courseMainMaterialDTOForDatabase.ContentSize,
                        DateTimePointOfFileCreation = courseMainMaterialDTOForDatabase.DateTimePointOfFileCreation,
                    }
                }
            );


        }


    }
}