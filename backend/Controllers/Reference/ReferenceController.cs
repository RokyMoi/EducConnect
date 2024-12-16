using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Reference.Tutor;
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


    }
}