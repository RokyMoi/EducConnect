using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Tutor;
using backend.Interfaces.Person;
using backend.Interfaces.Reference;
using backend.Interfaces.Tutor;
using backend.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Tutor
{
    [ApiController]
    [Route("tutor/teaching")]
    [CheckPersonLoginSignup]
    public class TutorTeachingInformationController(IPersonRepository _personRepository, ITutorRepository _tutorRepository, IReferenceRepository _referenceRepository) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> CreateTutorTeachingStyleInformation(TutorTeachingInformationSaveRequestDTO saveRequestDTO)
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
            Console.WriteLine("Is person a tutor: " + tutor != null);
            //If tutor is not null, check the TutorRegistrationStatus is below 6 (Email Verification, status before)
            if (tutor != null && tutor.TutorRegistrationStatusId < 7)
            {
                return UnprocessableEntity(
                    new
                    {

                        success = "false",
                        message = "It looks like you haven't completed your tutor registration yet. Please complete it to continue.",
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

            //If person is not tutor, return unauthorized access 
            if (tutor == null)
            {
                return Unauthorized(
                    new
                    {
                        success = "false",
                        message = "You must be tutor to access this",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }



            //Check it the TutorId already exists within TutorTeachingInformation table
            var tutorTeachingInformation = await _tutorRepository.GetTutorTeachingInformationByTutorId(tutor.TutorId);

            if (tutorTeachingInformation != null)
            {

                return Conflict(new
                {
                    success = "false",
                    message = "Cannot add tutor teaching information, tutor already has teaching information",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }
            //Check it the provided values for foreign keys are valid 

            //Check TutorTeachingStyleType 
            Console.WriteLine("Requested tutor teaching style type id: " + saveRequestDTO.TeachingStyleTypeId);
            var tutorTeachingStyleType = await _referenceRepository.GetTutorTeachingStyleTypeByIdAsync(saveRequestDTO.TeachingStyleTypeId);

            if (tutorTeachingStyleType == null)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "Tutor teaching style type not found",
                    data = new { },
                    timestamp = DateTime.Now
                });

            }


            //Check PrimaryCommunicationTypeId 
            var primaryCommunicationTypeId = await _referenceRepository.GetCommunicationTypeByIdAsync(saveRequestDTO.PrimaryCommunicationTypeId);
            if (primaryCommunicationTypeId == null)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "Communication type not found",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }


            //Check SecondaryCommunicationTypeId
            if (saveRequestDTO.SecondaryCommunicationTypeId.HasValue)
            {
                if (saveRequestDTO.SecondaryCommunicationTypeId == saveRequestDTO.PrimaryCommunicationTypeId)
                {
                    return BadRequest(
                        new
                        {
                            success = "false",
                            message = "Primary and secondary communication type cannot be the same",
                            data = new { },
                            timestamp = DateTime.Now
                        }
                    );
                }
                var secondaryCommunicationTypeId = await _referenceRepository.GetCommunicationTypeByIdAsync(saveRequestDTO.SecondaryCommunicationTypeId.Value);
                if (secondaryCommunicationTypeId == null)
                {
                    return NotFound(new
                    {
                        success = "false",
                        message = "Communication type not found",
                        data = new { },
                        timestamp = DateTime.Now
                    });
                }
            }

            //Check PrimaryEngagementMethodId
            var primaryEngagementMethodId = await _referenceRepository.GetEngagementMethodByIdAsync(saveRequestDTO.PrimaryEngagementMethodId);

            if (primaryEngagementMethodId == null)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "Engagement method not found",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            //Check SecondaryEngagementMethodId
            if (saveRequestDTO.SecondaryEngagementMethodId.HasValue)
            {
                if (saveRequestDTO.SecondaryEngagementMethodId == saveRequestDTO.PrimaryEngagementMethodId)
                {
                    return BadRequest(
                        new
                        {
                            success = "false",
                            message = "Primary and secondary engagement method cannot be the same",
                            data = new { },
                            timestamp = DateTime.Now
                        }
                    );
                }
                var secondaryEngagementMethod = await _referenceRepository.GetEngagementMethodByIdAsync(saveRequestDTO.SecondaryCommunicationTypeId.Value);
                if (secondaryEngagementMethod == null)
                {
                    return NotFound(new
                    {
                        success = "false",
                        message = "Engagement method not found",
                        data = new { },
                        timestamp = DateTime.Now
                    });
                }


            }




            //Update TutorRegistrationStatus to 7
            var tutorUpdateStatusResult = await _tutorRepository.UpdateTutorRegistrationStatus(personId, 10);

            if (tutorUpdateStatusResult == null)
            {
                return StatusCode(
                    500,
                    new
                    {
                        success = "false",
                        message = "We failed to update tutor registration status, please try again later",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }
            //Save the data to the database


            var saveResult = await _tutorRepository.CreateTutorTeachingInformation(new TutorTeachingInformationDTO
            {
                TutorId = tutor.TutorId,
                Description = saveRequestDTO.Description,
                TeachingStyleTypeId = saveRequestDTO.TeachingStyleTypeId,
                PrimaryCommunicationTypeId = saveRequestDTO.PrimaryCommunicationTypeId,
                SecondaryCommunicationTypeId = saveRequestDTO.SecondaryCommunicationTypeId,
                PrimaryEngagementMethodId = saveRequestDTO.PrimaryEngagementMethodId,
                SecondaryEngagementMethodId = saveRequestDTO.SecondaryEngagementMethodId,
                ExpectedResponseTime = saveRequestDTO.ExpectedResponseTime,
                SpecialConsiderations = saveRequestDTO.SpecialConsiderations
            });
            if (saveResult == null)
            {
                return StatusCode(
                    500,
                    new
                    {
                        success = "false",
                        message = "We failed to save tutor teaching information, please try again later",
                        data = new { },
                        timestamp = DateTime.Now
                    });
            }



            return Ok(
                new
                {
                    success = "true",
                    message = "Tutor teaching information saved successfully",
                    data = new
                    {
                        tutorTeachingInformation = saveResult
                    },
                    timestamp = DateTime.Now
                }
            );
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTutorTeachingInformation(TutorTeachingInformationUpdateRequestDTO updateRequestDTO)
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
            Console.WriteLine("Is person a tutor: " + tutor != null);
            //If tutor is not null, check the TutorRegistrationStatus is below 6 (Email Verification, status before)
            if (tutor != null && tutor.TutorRegistrationStatusId < 6)
            {
                return UnprocessableEntity(
                    new
                    {

                        success = "false",
                        message = "It looks like you haven't completed your tutor registration yet. Please complete it to continue.",
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

            //If person is not tutor, return unauthorized access 
            if (tutor == null)
            {
                return Unauthorized(
                    new
                    {
                        success = "false",
                        message = "You must be tutor to access this",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }

            //Check if the TutorId exists in the TutorTeachingInformation table
            var tutorTeachingInformation = await _tutorRepository.GetTutorTeachingInformationByTutorId(tutor.TutorId);

            if (tutorTeachingInformation == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "Tutor teaching information not found, you first have to add it",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Data validation

            //Add flag isUpdated to check if there are values to update
            bool isUpdated = false;

            //Check for Description
            if (updateRequestDTO.UpdateDescription && updateRequestDTO.Description != tutorTeachingInformation.Description)
            {
                tutorTeachingInformation.Description = updateRequestDTO.Description;
                isUpdated = true;
            }
            //Check for TeachingStyleTypeId

            if (updateRequestDTO.TeachingStyleTypeId.HasValue && updateRequestDTO.TeachingStyleTypeId.Value != tutorTeachingInformation.TeachingStyleTypeId)
            {
                //Check if the TeachingStyleTypeId exists
                var teachingStyleType = await _referenceRepository.GetTutorTeachingStyleTypeByIdAsync(updateRequestDTO.TeachingStyleTypeId.Value);

                if (teachingStyleType == null)
                {
                    return NotFound(new
                    {
                        success = "false",
                        message = "Tutor teaching style type not found",
                        data = new { },
                        timestamp = DateTime.Now
                    });
                }

                tutorTeachingInformation.TeachingStyleTypeId = updateRequestDTO.TeachingStyleTypeId.Value;
                isUpdated = true;


            }

            //Check for PrimaryCommunicationTypeId
            Console.WriteLine("PrimaryCommunicationTypeId: " + updateRequestDTO.PrimaryCommunicationTypeId.HasValue);
            if (updateRequestDTO.PrimaryCommunicationTypeId.HasValue && updateRequestDTO.PrimaryCommunicationTypeId.Value != tutorTeachingInformation.PrimaryCommunicationTypeId)
            {
                //Check if the PrimaryCommunicationTypeId exists
                var primaryCommunicationType = await _referenceRepository.GetCommunicationTypeByIdAsync(updateRequestDTO.PrimaryCommunicationTypeId.Value);

                if (primaryCommunicationType == null)
                {
                    return NotFound(new
                    {
                        success = "false",
                        message = "Communication type not found",
                        data = new { },
                        timestamp = DateTime.Now
                    });
                }

                tutorTeachingInformation.PrimaryCommunicationTypeId = updateRequestDTO.PrimaryCommunicationTypeId.Value;
                isUpdated = true;
            }


            //Check for SecondaryCommunicationTypeId
            if (updateRequestDTO.UpdateSecondaryCommunicationTypeId)
            {

                //Check if the SecondaryCommunicationTypeId is not null, and if it is not equal to the current SecondaryCommunicationTypeId
                if (updateRequestDTO.SecondaryCommunicationTypeId.HasValue && updateRequestDTO.SecondaryCommunicationTypeId.Value != tutorTeachingInformation.SecondaryCommunicationTypeId)
                {
                    var secondaryCommunicationTypeId = await _referenceRepository.GetCommunicationTypeByIdAsync(updateRequestDTO.SecondaryEngagementMethodId.Value);

                    //Check if the SecondaryCommunicationTypeId exists
                    if (secondaryCommunicationTypeId == null)
                    {
                        return NotFound(new
                        {
                            success = "false",
                            message = "Communication type not found",
                            data = new { },
                            timestamp = DateTime.Now
                        });
                    }

                    tutorTeachingInformation.PrimaryCommunicationTypeId = updateRequestDTO.SecondaryEngagementMethodId.Value;
                    isUpdated = true;
                }
                if (!updateRequestDTO.SecondaryCommunicationTypeId.HasValue && updateRequestDTO.SecondaryCommunicationTypeId.Value != tutorTeachingInformation.SecondaryCommunicationTypeId)
                {
                    tutorTeachingInformation.SecondaryCommunicationTypeId = null;
                    isUpdated = true;
                }





            }

            //Check for PrimaryEngagementMethodId
            if (updateRequestDTO.PrimaryEngagementMethodId.HasValue && updateRequestDTO.PrimaryEngagementMethodId.Value != tutorTeachingInformation.PrimaryEngagementMethodId)
            {
                //Check if the PrimaryEngagementMethodId exists
                var primaryEngagementMethod = await _referenceRepository.GetEngagementMethodByIdAsync(updateRequestDTO.PrimaryEngagementMethodId.Value);
                if (primaryEngagementMethod == null)
                {
                    return NotFound(new
                    {
                        success = "false",
                        message = "Engagement method not found",
                        data = new { },
                        timestamp = DateTime.Now
                    });
                }

                tutorTeachingInformation.PrimaryEngagementMethodId = updateRequestDTO.PrimaryEngagementMethodId.Value;
                isUpdated = true;
            }

            //Check for SecondaryEngagementMethodId
            if (updateRequestDTO.UpdateSecondaryEngagementMethodId && updateRequestDTO.SecondaryEngagementMethodId.Value != tutorTeachingInformation.SecondaryEngagementMethodId)
            {

                //Check if the SecondaryEngagementMethodId is not null, and if it is not, check does it exist
                if (updateRequestDTO.SecondaryEngagementMethodId.HasValue)
                {
                    var secondaryEngagementMethod = await _referenceRepository.GetEngagementMethodByIdAsync(updateRequestDTO.SecondaryEngagementMethodId.Value);
                    if (secondaryEngagementMethod == null)
                    {
                        return NotFound(new
                        {
                            success = "false",
                            message = "Engagement method not found",
                            data = new { },
                            timestamp = DateTime.Now
                        });
                    }
                }

                tutorTeachingInformation.SecondaryEngagementMethodId = updateRequestDTO.SecondaryEngagementMethodId.Value;
                isUpdated = true;
            }

            //Check for ExpectedResponseTime
            if (updateRequestDTO.UpdateExpectedResponseTime && updateRequestDTO.ExpectedResponseTime != tutorTeachingInformation.ExpectedResponseTime)
            {
                tutorTeachingInformation.ExpectedResponseTime = updateRequestDTO.ExpectedResponseTime;
                isUpdated = true;
            }

            //Check for SpecialConsiderations
            if (updateRequestDTO.UpdateSpecialConsiderations && updateRequestDTO.SpecialConsiderations != tutorTeachingInformation.SpecialConsiderations)
            {
                tutorTeachingInformation.SpecialConsiderations = updateRequestDTO.SpecialConsiderations;
                isUpdated = true;
            }


            if (!isUpdated)
            {
                return BadRequest(new
                {
                    success = "false",
                    message = "No new values were provided to update",
                    data = new { },
                    timestamp = DateTime.Now
                });

            }

            //Attempt to update the tutor teaching information
            var updatedTutorTeachingInformation = await _tutorRepository.UpdateTutorTeachingInformation(tutorTeachingInformation);
            if (updatedTutorTeachingInformation == null)
            {
                return StatusCode(
                    500,
                    new
                    {
                        success = "error",
                        message = "We are unable to update the tutor teaching information, please try again later",
                        data = new { },
                        timestamp = DateTime.Now
                    });
            }
            return Ok(
                new
                {
                    success = "true",
                    message = "Tutor teaching information updated successfully",
                    data = new
                    {
                        tutorTeachingInformation = updatedTutorTeachingInformation
                    },
                    timestamp = DateTime.Now
                }
            );

        }

        [HttpGet]
        public async Task<IActionResult> GetTutorTeachingInformation()
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
            Console.WriteLine("Is person a tutor: " + tutor != null);
            //If tutor is not null, check the TutorRegistrationStatus is below 6 (Email Verification, status before)
            if (tutor != null && tutor.TutorRegistrationStatusId < 6)
            {
                return UnprocessableEntity(
                    new
                    {

                        success = "false",
                        message = "It looks like you haven't completed your tutor registration yet. Please complete it to continue.",
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

            //If person is not tutor, return unauthorized access 
            if (tutor == null)
            {
                return Unauthorized(
                    new
                    {
                        success = "false",
                        message = "You must be tutor to access this",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }



            //Check it the TutorId already exists within TutorTeachingInformation table
            var tutorTeachingInformation = await _tutorRepository.GetTutorTeachingInformationByTutorId(tutor.TutorId);

            if (tutorTeachingInformation == null)
            {

                return NotFound(new
                {
                    success = "false",
                    message = "No tutor information found",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            return Ok(
                new
                {
                    success = "true",
                    message = "Tutor teaching information retrieved successfully",
                    data = new
                    {
                        tutorTeachingInformation = tutorTeachingInformation
                    },
                    timestamp = DateTime.Now
                }
            );
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetTutorTeachingInformationByTutorId([FromQuery] Guid tutorId)
        {
            if (tutorId == Guid.Empty)
            {
                return BadRequest(new
                {
                    success = "false",
                    message = "TutorId is required",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            var tutorTeachingInformation = await _tutorRepository.GetTutorTeachingInformationByTutorId(tutorId);

            if (tutorTeachingInformation == null)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "No tutor information found",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            return Ok(
                new
                {
                    success = "true",
                    message = "Tutor teaching information retrieved successfully",
                    data = new
                    {
                        tutorTeachingInformation = tutorTeachingInformation
                    },
                    timestamp = DateTime.Now
                }
            );
        }
    }
}