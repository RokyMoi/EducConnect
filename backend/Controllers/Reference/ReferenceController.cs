using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.LearningCategory;
using backend.DTOs.Reference.EmploymentType;
using backend.DTOs.Reference.IndustryClassification;
using backend.DTOs.Reference.LearningSubcategory;
using backend.DTOs.Reference.Tutor;
using backend.DTOs.Reference.WorkType;
using backend.Entities.Reference;
using backend.Interfaces.Reference;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Reference
{
    [ApiController]
    [Route("reference")]
    public class ReferenceController(IReferenceRepository _referenceRepository) : ControllerBase
    {
        [HttpGet("tutor-registration-status/all")]
        public async Task<IActionResult> getTutorRegistrationStatus()
        {

            //Get all the data from the table TutorRegistrationStatus
            var tutorRegistrationStatus = await _referenceRepository.GetAllTutorRegistrationStatusesAsync();

            if (tutorRegistrationStatus == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "No tutor registration status found",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            List<TutorRegistrationStatusResponseDTO> tutorRegistrationStatusResponseDTOs = new List<TutorRegistrationStatusResponseDTO>();
            foreach (var item in tutorRegistrationStatus)
            {
                tutorRegistrationStatusResponseDTOs.Add(new TutorRegistrationStatusResponseDTO
                {
                    TutorRegistrationStatusId = item.TutorRegistrationStatusId,
                    Name = item.Name,
                    Description = item.Description,
                    IsSkippable = item.IsSkippable,
                });
            }
            return Ok(
                new
                {
                    success = "true",
                    message = "Tutor registration status retrieved successfully",
                    data = new
                    {
                        tutorRegistrationStatus = tutorRegistrationStatusResponseDTOs
                    },
                    timestamp = DateTime.Now
                }
            );
        }

        [HttpGet("employment-type/all")]
        public async Task<IActionResult> getAllEmploymentTypes()
        {

            var employmentTypes = await _referenceRepository.GetAllEmploymentTypesAsync();

            if (employmentTypes == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "No employment types found",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            List<EmploymentTypeResponseDTO> employmentTypeResponseDTOs = [];

            foreach (var item in employmentTypes)
            {
                employmentTypeResponseDTOs.Add(new EmploymentTypeResponseDTO
                {
                    EmploymentTypeId = item.EmploymentTypeId,
                    Name = item.Name,
                    Description = item.Description,
                });
            }
            return Ok(
                new
                {
                    success = "true",
                    message = "Employment types retrieved successfully",
                    data = new
                    {
                        employmentType = employmentTypeResponseDTOs
                    },
                    timestamp = DateTime.Now
                });
        }

        [HttpGet("industry-classification/all")]
        public async Task<IActionResult> GetAllIndustryClassifications()
        {
            var industryClassifications = await _referenceRepository.GetAllIndustryClassificationsAsync();

            if (industryClassifications == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "No industry classifications found",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            List<IndustryClassificationResponseDTO> responseDTOList = new List<IndustryClassificationResponseDTO>();
            foreach (IndustryClassification item in industryClassifications)
            {
                responseDTOList.Add(
                    new IndustryClassificationResponseDTO
                    {
                        IndustryClassificationId = item.IndustryClassificationId,
                        Industry = item.Industry,
                        Sector = item.Sector,
                    }
                );
            }

            return Ok(
                new
                {
                    success = "true",
                    message = $"Found {responseDTOList.Count} records of industry classifications",
                    data = new
                    {
                        industryClassification = responseDTOList,
                    },
                    timestamp = DateTime.Now,
                }
                );
        }

        [HttpGet("work-type/all")]
        public async Task<IActionResult> GetAllWorkTypes()
        {
            var workTypes = await _referenceRepository.GetAllWorkTypesAsync();
            if (workTypes == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "No work types found",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            List<WorkTypeResponseDTO> workTypeResponseDTOs = [];
            foreach (WorkType item in workTypes)
            {
                workTypeResponseDTOs.Add(new WorkTypeResponseDTO
                {
                    WorkTypeId = item.WorkTypeId,
                    Name = item.Name,
                    Description = item.Description,
                });

            }

            return Ok(
                new
                {
                    success = "true",
                    message = "Work types retrieved successfully",
                    data = new
                    {
                        workType = workTypeResponseDTOs
                    },
                    timestamp = DateTime.Now
                });
        }

        [HttpGet("tutor-teaching-style-type/all")]
        public async Task<IActionResult> GetAllTutorTeachingStyleTypes()
        {
            var tutorTeachingStyleTypes = await _referenceRepository.GetAllTutorTeachingStyleTypesAsync();
            if (tutorTeachingStyleTypes == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "No tutor teaching style types found",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }
            return Ok(
                new
                {
                    success = "true",
                    message = $"Found {tutorTeachingStyleTypes.Count} of tutor  teaching style types",
                    data = new
                    {
                        tutorTeachingStyleType = tutorTeachingStyleTypes,
                    },
                    timestamp = DateTime.Now

                }
            );
        }

        [HttpGet("communication-type/all")]
        public async Task<IActionResult> GetAllCommunicationTypes()
        {

            var communicationTypesList = await _referenceRepository.GetAllCommunicationTypesAsync();

            if (communicationTypesList == null || communicationTypesList.Count == 0)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "No communication types found",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            return Ok(
                new
                {
                    success = "true",
                    message = $"Found {communicationTypesList.Count} communication types",
                    data = new
                    {
                        communicationType = communicationTypesList
                    },
                    timestamp = DateTime.Now


                });
        }

        [HttpGet("engagement-method/all")]
        public async Task<IActionResult> GetAllEngagementMethods()
        {
            var engagementMethodsList = await _referenceRepository.GetAllEngagementMethodsAsync();

            if (engagementMethodsList == null || engagementMethodsList.Count == 0)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "No engagement methods found",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            return Ok(
                new
                {
                    success = "true",
                    message = $"Found {engagementMethodsList.Count} engagement methods",
                    data = new
                    {
                        engagementMethod = engagementMethodsList
                    },
                    timestamp = DateTime.Now
                }
            );
        }

        [HttpGet("learning-category-subcategory/all")]
        public async Task<IActionResult> GetAllLearningCategoriesAndSubcategories()
        {

            var learningCategoriesAndSubcategories = await _referenceRepository.GetAllLearningCategoriesAndSubcategories();


            if (learningCategoriesAndSubcategories == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "No learning categories and subcategories found",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );

            }

            var learningCategoriesDTOList = new List<LearningCategoryDTO>();
            foreach (var category in learningCategoriesAndSubcategories.LearningCategoriesList)
            {
                learningCategoriesDTOList.Add(
                    new LearningCategoryDTO
                    {
                        LearningCategoryId = category.LearningCategoryId,
                        LearningCategoryName = category.LearningCategoryName,
                        LearningCategoryDescription = category.LearningCategoryDescription
                    }
                );
            }

            var learningSubcategoriesDTOList = new List<LearningSubcategoryDTO>();
            foreach (var subcategory in learningCategoriesAndSubcategories.LearningSubcategoriesList)
            {
                learningSubcategoriesDTOList.Add(
                    new LearningSubcategoryDTO
                    {
                        LearningSubcategoryId = subcategory.LearningSubcategoryId,
                        LearningSubcategoryName = subcategory.LearningSubcategoryName,
                        LearningCategoryId = subcategory.LearningCategoryId,
                        Description = subcategory.Description
                    }
                );
            }

            return Ok(
                new
                {
                    success = "true",
                    message = $"Found {learningCategoriesAndSubcategories.LearningCategoriesList.Count} learning categories and {learningCategoriesAndSubcategories.LearningSubcategoriesList} subcategories",
                    data = new
                    {
                        learningCategories = learningCategoriesDTOList,
                        learningSubcategories = learningSubcategoriesDTOList
                    },
                    timestamp = DateTime.Now
                }
            );
        }

        [HttpGet("learning-difficulty-level/all")]
        public async Task<IActionResult> GetAllLearningDifficultyLevels()
        {
            var learningDifficultyLevels = await _referenceRepository.GetAllLearningDifficultyLevelsAsync();
            if (learningDifficultyLevels == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "No learning difficulty levels found",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }
            return Ok(
                new
                {
                    success = "true",
                    message = $"Found {learningDifficultyLevels.Count} learning difficulty levels",
                    data = new
                    {
                        learningDifficultyLevel = learningDifficultyLevels
                    },
                    timestamp = DateTime.Now,
                }
            );
        }

        [HttpGet("course-type/all")]
        public async Task<IActionResult> GetAllCourseTypes()
        {
            var courseTypes = await _referenceRepository.GetAllCourseTypesAsync();

            if (courseTypes == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "No course types found",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }
            return Ok(
                new
                {
                    success = "true",
                    message = $"Found {courseTypes.Count} course types",
                    data = new
                    {
                        courseType = courseTypes
                    },
                    timestamp = DateTime.Now,
                }
            );

        }

        [HttpGet("language/all")]
        public async Task<IActionResult> GetAllLanguages()
        {
            var languages = await _referenceRepository.GetAllLanguagesAsync();
            if (languages == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "No languages found",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            return Ok(
                new
                {
                    success = "true",
                    message = $"Found {languages.Count} languages",
                    data = new
                    {
                        language = languages
                    },
                    timestamp = DateTime.Now,
                }
            );
        }
    }
}