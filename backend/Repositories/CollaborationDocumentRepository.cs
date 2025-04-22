using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EduConnect.Data;
using EduConnect.DTOs;
using EduConnect.Entities.CollaborationDocument;
using EduConnect.Entities.Person;
using EduConnect.Helpers;
using EduConnect.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Repositories
{
    public class CollaborationDocumentRepository(DataContext dataContext, ILogger<CollaborationDocumentRepository> logger) : ICollaborationDocumentRepository
    {
        private readonly DataContext _dataContext = dataContext;
        private readonly ILogger<CollaborationDocumentRepository> _logger = logger;

        public async Task<Document?> GetDocumentById(Guid documentId, bool includeOtherEntities = false)
        {
            var document = _dataContext.Document.AsQueryable();

            if (includeOtherEntities)
            {
                document = document
                .Include(x => x.Person).AsQueryable();


            }

            return await document.Where(x => x.DocumentId == documentId).FirstOrDefaultAsync();
        }

        public async Task<bool> CheckUserHasInvitation(Guid documentId, Guid personId)
        {
            return await _dataContext.CollaborationDocumentInvitation
            .Where(x => x.DocumentId == documentId && x.InvitedPersonId == personId && x.Status != false).AnyAsync();


        }

        public async Task<bool> SaveCollaborationDocumentInvitation(CollaborationDocumentInvitation collaborationDocumentInvitation)
        {
            try
            {
                await _dataContext.CollaborationDocumentInvitation.AddAsync(collaborationDocumentInvitation);
                await _dataContext.SaveChangesAsync();
                _logger.LogInformation($"CollaborationDocumentInvitation created with ID: {collaborationDocumentInvitation.CollaborationDocumentInvitationId} by person ID: {collaborationDocumentInvitation.InvitedPersonId}");
                return true;
            }
            catch (System.Exception ex)
            {

                _logger.LogError(ex, message: $"An error occurred while creating the CollaborationDocumentInvitation with ID: {collaborationDocumentInvitation.CollaborationDocumentInvitationId}");
                return false;
            }
        }

        public async Task<List<GetAllDocumentsByCreatedByPersonIdRepositoryResponse>> GetAllDocumentsByCreatedByPersonId(Guid personId)
        {
            var documents = await _dataContext.Document
       .Where(doc => doc.CreatedByPersonId == personId)
       .Select(doc => new
       {
           doc.DocumentId,
           doc.Title,
           doc.CreatedAt
       })
       .ToListAsync();

            var documentIds = documents.Select(d => d.DocumentId).ToList();

            // Fetch invitations in one go
            var invitations = await _dataContext.CollaborationDocumentInvitation
                .Where(i => documentIds.Contains(i.DocumentId))
                .ToListAsync();

            // Fetch participants in one go
            var participants = await _dataContext.CollaborationDocumentParticipant
                .Where(p => documentIds.Contains(p.DocumentId))
                .ToListAsync();




            var result = documents.Select(doc =>
   {
       var docInvitations = invitations.Where(i => i.DocumentId == doc.DocumentId).ToList();

       return new GetAllDocumentsByCreatedByPersonIdRepositoryResponse
       {
           DocumentId = doc.DocumentId,
           Title = doc.Title,
           CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(doc.CreatedAt).DateTime,

           NumberOfParticipants = participants.Count(p => p.DocumentId == doc.DocumentId),
           TotalNumberOfInvitedUsers = docInvitations.Count,
           NumberOfAcceptedInvitations = docInvitations.Count(i => i.Status == true),
           NumberOfRejectedInvitations = docInvitations.Count(i => i.Status == false),
           NumberOfPendingInvitations = docInvitations.Count(i => i.Status == null)
       };
   }).ToList();

            return result;

        }

        public async Task<List<GetAllDocumentsByParticipantPersonIdResponse>> GetAllDocumentsByParticipantPersonId(Guid personId)
        {
            return await _dataContext.CollaborationDocumentParticipant
            .Include(x => x.Document)
            .Include(x => x.Document.Person)
            .Include(x => x.Document.Person.PersonDetails)
            .Include(x => x.Document.Person.PersonEmail)
            .Where(x => x.ParticipantPersonId == personId)
            .Select(
                x => new GetAllDocumentsByParticipantPersonIdResponse
                {
                    DocumentId = x.DocumentId,
                    Title = x.Document.Title,
                    CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(x.Document.CreatedAt).DateTime,
                    CreatedByIdentificationData = IdentificationDataGetter.GetIdentificationData
                    (x.Document.Person.PersonEmail, x.Document.Person.PersonDetails),
                    NumberOfParticipants = _dataContext.CollaborationDocumentParticipant.Where(x => x.DocumentId == x.DocumentId).Count(),
                    JoinedAt = DateTimeOffset.FromUnixTimeMilliseconds(x.CreatedAt).DateTime,
                }
            ).ToListAsync();
        }

        public async Task<List<GetAllInvitationsForPersonIdResponse>> GetAllInvitationsForPersonId(Guid personId)
        {
            return await _dataContext
            .CollaborationDocumentInvitation
            .Include(x => x.Document)
            .Include(x => x.Document.Person)
            .Include(x => x.Document.Person.PersonDetails)
            .Include(x => x.Document.Person.PersonEmail)
            .Where(x => x.InvitedPersonId == personId)
            .Select(
                x => new GetAllInvitationsForPersonIdResponse
                {
                    CollaborationDocumentInvitationId = x.CollaborationDocumentInvitationId,
                    DocumentId = x.DocumentId,
                    Title = x.Document.Title,
                    DocumentCreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(x.Document.CreatedAt).DateTime,
                    InvitationSentByPersonIdentificationData = IdentificationDataGetter.GetIdentificationData(x.Document.Person.PersonEmail, x.Document.Person.PersonDetails),
                    InvitationStatus = x.Status,
                    InvitationStatusChangedAt = x.StatusChangedAt,
                    InvitationSentAt = DateTimeOffset.FromUnixTimeMilliseconds(x.CreatedAt).DateTime

                }
            ).ToListAsync();
        }

        public async Task<CollaborationDocumentInvitation?> GetCollaborationDocumentInvitationById(Guid collaborationDocumentInvitationId)
        {
            return await _dataContext.CollaborationDocumentInvitation
            .Where(x => x.CollaborationDocumentInvitationId == collaborationDocumentInvitationId)
            .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateCollaborationDocumentInvitation(CollaborationDocumentInvitation collaborationDocumentInvitation)
        {
            try
            {
                _dataContext.CollaborationDocumentInvitation.Update(collaborationDocumentInvitation);
                await _dataContext.SaveChangesAsync();
                _logger.LogInformation($"CollaborationDocumentInvitation updated with ID: {collaborationDocumentInvitation.CollaborationDocumentInvitationId} by person ID: {collaborationDocumentInvitation.InvitedPersonId}");
                return true;
            }
            catch (System.Exception ex)
            {

                _logger.LogError(ex, message: $"An error occurred while updating the CollaborationDocumentInvitation with ID: {collaborationDocumentInvitation.CollaborationDocumentInvitationId}");
                return false;
            }
        }

        public async Task<bool> AddPersonToCollaborationDocument(Guid documentId, Guid personId, Guid invitationId)
        {
            var collaborationDocumentParticipant = new CollaborationDocumentParticipant
            {
                DocumentId = documentId,
                ParticipantPersonId = personId,
                CollaborationDocumentInvitationId = invitationId,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            try
            {
                await _dataContext.CollaborationDocumentParticipant.AddAsync(collaborationDocumentParticipant);
                await _dataContext.SaveChangesAsync();
                _logger.LogInformation($"Person ${personId} added as collaborator on document ${documentId} with invitation ID: {invitationId}, collaborationDocumentParticipant ID: {collaborationDocumentParticipant.CollaborationDocumentParticipantId}");
                return true;
            }
            catch (System.Exception ex)
            {

                _logger.LogError(ex, message: $"An error occurred while adding person ${personId} as collaborator on document ${documentId} with invitation ID: {invitationId}");
                return false;

            }
        }

        public async Task<bool> DeleteCollaborationDocumentInvitation(Guid collaborationDocumentInvitationId)
        {
            var collaborationDocumentInvitation = await _dataContext.CollaborationDocumentInvitation.Where(x => x.CollaborationDocumentInvitationId == collaborationDocumentInvitationId).FirstOrDefaultAsync();

            if (collaborationDocumentInvitation == null)
            {
                _logger.LogWarning($"CollaborationDocumentInvitation with ID: {collaborationDocumentInvitationId} not found.");
                return false;
            }

            try
            {
                _dataContext.CollaborationDocumentInvitation.Remove(collaborationDocumentInvitation);
                await _dataContext.SaveChangesAsync();
                _logger.LogInformation($"CollaborationDocumentInvitation with ID: {collaborationDocumentInvitationId} deleted.");
                return true;
            }
            catch (System.Exception ex)
            {

                _logger.LogError(ex, message: $"An error occurred while deleting the CollaborationDocumentInvitation with ID: {collaborationDocumentInvitationId}");
                return false;
            }
        }

        public async Task<List<GetAllInvitationsSentByPersonIdResponse>> GetAllInvitationsSentByPersonId(Guid personId)
        {
            return await _dataContext
            .CollaborationDocumentInvitation
            .Include(x => x.Document)
            .Include(x => x.InvitedPerson)
            .Include(x => x.InvitedPerson.PersonDetails)
            .Include(x => x.InvitedPerson.PersonEmail)
            .Where(x => x.InvitedByPersonId == personId)
            .Select(
                x => new GetAllInvitationsSentByPersonIdResponse
                {
                    CollaborationDocumentInvitationId = x.CollaborationDocumentInvitationId,
                    DocumentId = x.DocumentId,
                    Title = x.Document.Title,
                    DocumentCreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(x.Document.CreatedAt).DateTime,
                    InvitationSentToPersonIdentificationData = IdentificationDataGetter.GetIdentificationData(x.InvitedPerson.PersonEmail, x.InvitedPerson.PersonDetails),
                    InvitationStatus = x.Status,
                    InvitationStatusChangedAt = x.StatusChangedAt,
                    InvitationSentAt = DateTimeOffset.FromUnixTimeMilliseconds(x.CreatedAt).DateTime
                }
            ).ToListAsync();
        }

        public async Task<GetCollaborationDocumentInviteInfoResponse?> GetCollaborationDocumentInviteInfoByDocumentId(Guid documentId)
        {
            return await _dataContext
            .Document
            .Where(x => x.DocumentId == documentId)
            .Select(
                x => new GetCollaborationDocumentInviteInfoResponse
                {
                    DocumentId = x.DocumentId,
                    Title = x.Title,
                    CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(x.CreatedAt).DateTime,
                    NumberOfParticipants = _dataContext.CollaborationDocumentParticipant.Where(x => x.DocumentId == x.DocumentId).Count(),
                    TotalNumberOfInvitations = _dataContext.CollaborationDocumentInvitation.Where(x => x.DocumentId == x.DocumentId).Count(),
                    NumberOfAcceptedInvitations = _dataContext.CollaborationDocumentInvitation.Where(x => x.DocumentId == x.DocumentId && x.Status == true).Count(),
                    NumberOfRejectedInvitations = _dataContext.CollaborationDocumentInvitation.Where(x => x.DocumentId == x.DocumentId && x.Status == false).Count(),
                    NumberOfPendingInvitations = _dataContext.CollaborationDocumentInvitation.Where(x => x.DocumentId == x.DocumentId && x.Status == null).Count(),
                }
            ).FirstOrDefaultAsync();
        }
    }
}