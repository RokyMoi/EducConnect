using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Data;
using EduConnect.DTOs;
using EduConnect.Entities;
using EduConnect.Entities.CollaborationDocument;
using EduConnect.Interfaces;
using EduConnect.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Controllers.CollaborationDocument
{
    [ApiController]
    [Route("collaboration-document")]
    [AuthenticationGuard(IsAdmin = true, IsStudent = true, IsTutor = true)]
    public class CollaborationDocumentController(DataContext dataContext, IHttpContextAccessor httpContextAccessor, ILogger<CollaborationDocumentController> logger, ICollaborationDocumentRepository collaborationDocumentRepository) : ControllerBase
    {
        private readonly DataContext _dataContext = dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogger<CollaborationDocumentController> _logger = logger;

        private readonly ICollaborationDocumentRepository _collaborationDocumentRepository = collaborationDocumentRepository;

        [HttpPost("create")]
        public async Task<IActionResult> CreateDocument([FromBody] CreateDocumentRequest request)
        {
            var personId = Guid.Parse(_httpContextAccessor.HttpContext.Items["PersonId"].ToString());

            if (personId == Guid.Empty)
            {
                return Unauthorized(
                    ApiResponse<object>.GetApiResponse(
                        "You are not authorized to perform this action",
                        null
                    )
                );
            }

            var document = new Document
            {
                Title = request.Title,
                CreatedByPersonId = personId,

            };

            try
            {
                await _dataContext.Document.AddAsync(document);
                await _dataContext.SaveChangesAsync();

                _logger.LogInformation($"New Document created with ID: {document.DocumentId} by person ID: {personId}");
                return Ok(
                    ApiResponse<Guid>.GetApiResponse(
                        "Document created successfully",
                        document.DocumentId
                    )
                );

            }
            catch (System.Exception ex)
            {

                _logger.LogError(ex, $"An error occurred while creating the document with ID: {document.DocumentId} by person ID: {personId}");
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse("An error occurred while creating the document, please try again", null)
                );
            }
        }

        [HttpGet("check/{title}")]
        public async Task<IActionResult> CheckIsDocumentTitleTaken([FromRoute] string title)
        {
            var isTitleTaken = await _dataContext.Document.Where(x => x.Title.Equals(title.Trim())).AnyAsync();

            return Ok(
                ApiResponse<bool>.GetApiResponse($"Check returned result ${(isTitleTaken ? "taken" : "not taken")}", isTitleTaken)
            );
        }

        [HttpGet("owner/all")]
        public async Task<IActionResult> GetAllDocumentsByPersonId()
        {
            var personId = Guid.Parse(_httpContextAccessor.HttpContext.Items["PersonId"].ToString());

            if (personId == Guid.Empty)
            {
                return Unauthorized(
                    ApiResponse<object>.GetApiResponse(
                        "You are not authorized to perform this action",
                        null
                    )
                );
            }

            var documentsByPerson = await _collaborationDocumentRepository.GetAllDocumentsByCreatedByPersonId(personId);




            return Ok(
                ApiResponse<List<GetAllDocumentsByCreatedByPersonIdRepositoryResponse>>.GetApiResponse(
                    "Documents retrieved successfully",
                    documentsByPerson
                )
            );



        }

        [HttpGet("participant/all")]
        public async Task<IActionResult> GetAllDocumentsByParcipantPersonId()
        {
            var personId = Guid.Parse(_httpContextAccessor.HttpContext.Items["PersonId"].ToString());

            if (personId == Guid.Empty)
            {
                return Unauthorized(
                    ApiResponse<object>.GetApiResponse(
                        "You are not authorized to perform this action",
                        null
                    )
                );
            }

            var documentsWherePersonIsParticipant = await _collaborationDocumentRepository.GetAllDocumentsByParticipantPersonId(personId);

            return Ok(
                ApiResponse<List<GetAllDocumentsByParticipantPersonIdResponse>>.GetApiResponse(
                    "Documents retrieved successfully",
                    documentsWherePersonIsParticipant
                )
            );
        }

        [HttpGet("invitation/all")]
        public async Task<IActionResult> GetAllInvitationsForPersonId()
        {
            var personId = Guid.Parse(_httpContextAccessor.HttpContext.Items["PersonId"].ToString());

            if (personId == Guid.Empty)
            {
                return Unauthorized(
                    ApiResponse<object>.GetApiResponse(
                        "You are not authorized to perform this action",
                        null
                    )
                );
            }

            var invitations = await _collaborationDocumentRepository.GetAllInvitationsForPersonId(personId);

            return Ok(
                ApiResponse<List<GetAllInvitationsForPersonIdResponse>>.GetApiResponse(
                    "Invitations retrieved successfully",
                    invitations
                )
            );
        }

        [HttpPatch("invitation/accept/{invitationId}")]
        public async Task<IActionResult> AcceptInvitation([FromRoute] Guid invitationId)
        {
            var personId = Guid.Parse(_httpContextAccessor.HttpContext.Items["PersonId"].ToString());

            if (personId == Guid.Empty)
            {
                return Unauthorized(
                    ApiResponse<object>.GetApiResponse(
                        "You are not authorized to perform this action",
                        null
                    )
                );
            }

            var invitation = await _collaborationDocumentRepository.GetCollaborationDocumentInvitationById(invitationId);

            if (invitation == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Invitation not found",
                        null
                    )
                );
            }

            if (invitation.InvitedPersonId != personId)
            {
                return Unauthorized(
                    ApiResponse<object>.GetApiResponse(
                        "You cannot accept an invitation that is not for you",
                        null
                    )
                );
            }

            if (invitation.Status.HasValue)
            {
                return Conflict(
                    ApiResponse<object>.GetApiResponse(
                        $"Invitation already {(invitation.Status == true ? "accepted" : "rejected")}",
                        null
                ));
            }

            invitation.Status = true;
            invitation.StatusChangedAt = DateTime.UtcNow;
            invitation.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var updateResult = await _collaborationDocumentRepository.UpdateCollaborationDocumentInvitation(invitation);

            if (!updateResult)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse("An error occurred while accepting the invitation, please try again", null)
                );
            }

            var joinCollaborationResult = await _collaborationDocumentRepository.AddPersonToCollaborationDocument(invitation.DocumentId, personId, invitation.CollaborationDocumentInvitationId);
            if (!joinCollaborationResult)
            {
                invitation.Status = null;
                invitation.StatusChangedAt = null;
                invitation.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                await _collaborationDocumentRepository.UpdateCollaborationDocumentInvitation(invitation);

                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse("An error occurred while adding you to the document, please try again", null)
                );
            }

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "Invitation accepted successfully and you have been added to the document",
                    null
                )
            );




        }

        [HttpPatch("invitation/reject/{invitationId}")]
        public async Task<IActionResult> RejectInvitation([FromRoute] Guid invitationId)
        {
            var personId = Guid.Parse(_httpContextAccessor.HttpContext.Items["PersonId"].ToString());

            if (personId == Guid.Empty)
            {
                return Unauthorized(
                    ApiResponse<object>.GetApiResponse(
                        "You are not authorized to perform this action",
                        null
                    )
                );
            }

            var invitation = await _collaborationDocumentRepository.GetCollaborationDocumentInvitationById(invitationId);

            if (invitation == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Invitation not found",
                        null
                    )
                );
            }

            if (invitation.InvitedPersonId != personId)
            {
                return Unauthorized(
                    ApiResponse<object>.GetApiResponse(
                        "You cannot accept an invitation that is not for you",
                        null
                    )
                );
            }

            if (invitation.Status.HasValue)
            {
                return Conflict(
                    ApiResponse<object>.GetApiResponse(
                        $"Invitation already {(invitation.Status == true ? "accepted" : "rejected")}",
                        null
                ));
            }

            invitation.Status = false;
            invitation.StatusChangedAt = DateTime.UtcNow;
            invitation.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var updateResult = await _collaborationDocumentRepository.UpdateCollaborationDocumentInvitation(invitation);

            if (!updateResult)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse("An error occurred while rejecting the invitation, please try again", null)
                );
            }

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "Invitation rejected successfully",
                    null
                )
            );




        }

        [HttpDelete("invitation/delete/{invitationId}")]
        public async Task<IActionResult> DeleteInvitation([FromRoute] Guid invitationId)
        {
            var personId = Guid.Parse(_httpContextAccessor.HttpContext.Items["PersonId"].ToString());

            if (personId == Guid.Empty)
            {
                return Unauthorized(
                    ApiResponse<object>.GetApiResponse(
                        "You are not authorized to perform this action",
                        null
                    )
                );
            }

            var invitation = await _collaborationDocumentRepository.GetCollaborationDocumentInvitationById(invitationId);

            if (invitation == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Invitation not found",
                        null
                    )
                );
            }

            if (invitation.InvitedPersonId != personId)
            {
                return Unauthorized(
                    ApiResponse<object>.GetApiResponse(
                        "You cannot delete an invitation that is not for you",
                        null
                    )
                );
            }

            if (!invitation.Status.HasValue)
            {
                return Conflict(
                    ApiResponse<object>.GetApiResponse(
                        "Invitation that is still pending cannot be deleted",
                        null
                    )
                );
            }

            if (invitation.Status.HasValue && invitation.Status.Value)
            {
                return Conflict(
                    ApiResponse<object>.GetApiResponse(
                        "Invitation that is already accepted cannot be deleted",
                        null
                    )
                );
            }

            var deleteResult = await _collaborationDocumentRepository.DeleteCollaborationDocumentInvitation(invitationId);

            if (!deleteResult)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse("An error occurred while deleting the invitation, please try again", null)
                );
            }

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "Invitation deleted successfully",
                    null
                )
            );


        }

        [HttpPost("invite")]
        public async Task<IActionResult> InviteUserToDocument([FromBody] InviteUserToDocumentRequest request)
        {
            var personId = Guid.Parse(_httpContextAccessor.HttpContext.Items["PersonId"].ToString());

            if (personId == Guid.Empty)
            {
                return Unauthorized(
                    ApiResponse<object>.GetApiResponse(
                        "You are not authorized to perform this action",
                        null
                    )
                );
            }

            //Check if the personId is the same as the invited personId
            if (request.InvitedPersonId == personId)
            {
                return BadRequest(
                    ApiResponse<object>.GetApiResponse(
                        "You cannot invite yourself to a document",
                        null
                    )
                );
            }

            //Check if the document exists
            var document = await _collaborationDocumentRepository.GetDocumentById(request.DocumentId, true);

            if (document == null)
            {

                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Document not found",
                        null
                    )
                );
            }

            //Check if the user is the owner of the document
            if (document.CreatedByPersonId != personId)
            {
                return Unauthorized(
                    ApiResponse<object>.GetApiResponse(
                        "You cannot invite users to a document that you haven't created",
                        null
                    )
                );
            }

            //Check if the person to be invited exists
            if (!await _dataContext.Person.Where(x => x.PersonId == request.InvitedPersonId).AnyAsync())
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Invited person not found",
                        null
                    )
                );
            }

            //Check if the person to be invited is already invited
            var isInvited = await _collaborationDocumentRepository.CheckUserHasInvitation(request.DocumentId, request.InvitedPersonId);
            Console.WriteLine(isInvited);
            if (isInvited)
            {
                return Conflict(
                    ApiResponse<object>.GetApiResponse(
                        "User already invited to this document",
                        null
                    )
                );
            }


            var invitation = new CollaborationDocumentInvitation
            {
                DocumentId = request.DocumentId,
                InvitedPersonId = request.InvitedPersonId,
                InvitedByPersonId = personId,

            };

            var createResult = await _collaborationDocumentRepository.SaveCollaborationDocumentInvitation(invitation);

            if (!createResult)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse("An error occurred while creating the invitation, please try again", null)
                );
            }

            return Ok(
                ApiResponse<Guid>.GetApiResponse(
                    "Invitation created successfully",
                    invitation.CollaborationDocumentInvitationId
                )
            );



        }

        [HttpGet("invite/all")]
        public async Task<IActionResult> GetAllInvitationsSentByPersonId()
        {
            var personId = Guid.Parse(_httpContextAccessor.HttpContext.Items["PersonId"].ToString());
            if (personId == Guid.Empty)
            {
                return Unauthorized(
                    ApiResponse<object>.GetApiResponse(
                        "You are not authorized to perform this action",
                        null
                    )
                );
            }

            var invitations = await _collaborationDocumentRepository.GetAllInvitationsSentByPersonId(personId);

            return Ok(
                ApiResponse<List<GetAllInvitationsSentByPersonIdResponse>>.GetApiResponse(
                    "Invitations retrieved successfully",
                    invitations
                )
            );
        }

        [HttpDelete("invite/delete/{invitationId}")]
        public async Task<IActionResult> DeleteInviteSentByPersonByInvitationId(Guid invitationId)
        {
            var personId = Guid.Parse(_httpContextAccessor.HttpContext.Items["PersonId"].ToString());

            if (personId == Guid.Empty)
            {
                return Unauthorized(
                    ApiResponse<object>.GetApiResponse(
                        "You are not authorized to perform this action",
                        null
                    )
                );
            }

            var invitation = await _collaborationDocumentRepository.GetCollaborationDocumentInvitationById(invitationId);

            if (invitation == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Invitation not found",
                        null
                    )
                );
            }

            if (invitation.InvitedByPersonId != personId)
            {
                return Unauthorized(
                    ApiResponse<object>.GetApiResponse(
                        "You cannot delete an invitation that you did not send",
                        null
                    )
                );
            }

            var deleteResult = await _collaborationDocumentRepository.DeleteCollaborationDocumentInvitation(invitationId);

            if (!deleteResult)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse("An error occurred while deleting the invitation, please try again", null)
                );
            }

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "Invitation deleted successfully",
                    null
                )
            );
        }


    }
}