using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.DTOs;
using EduConnect.Entities.CollaborationDocument;

namespace EduConnect.Interfaces
{
    public interface ICollaborationDocumentRepository
    {
        public Task<Document?> GetDocumentById(Guid documentId, bool includeOtherEntities = false);
        public Task<bool> CheckUserHasInvitation(Guid documentId, Guid personId);
        public Task<bool> SaveCollaborationDocumentInvitation(CollaborationDocumentInvitation collaborationDocumentInvitation);
        public Task<List<GetAllDocumentsByCreatedByPersonIdRepositoryResponse>> GetAllDocumentsByCreatedByPersonId(Guid personId);
        public Task<List<GetAllDocumentsByParticipantPersonIdResponse>> GetAllDocumentsByParticipantPersonId(Guid personId);

        public Task<List<GetAllInvitationsForPersonIdResponse>> GetAllInvitationsForPersonId(Guid personId);

        public Task<CollaborationDocumentInvitation?> GetCollaborationDocumentInvitationById(Guid collaborationDocumentInvitationId);

        public Task<bool> UpdateCollaborationDocumentInvitation(CollaborationDocumentInvitation collaborationDocumentInvitation);

        public Task<bool> AddPersonToCollaborationDocument(Guid documentId, Guid personId, Guid invitationId);

        public Task<bool> DeleteCollaborationDocumentInvitation(Guid collaborationDocumentInvitationId);

        public Task<List<GetAllInvitationsSentByPersonIdResponse>> GetAllInvitationsSentByPersonId(Guid personId);
        public Task<GetCollaborationDocumentInviteInfoResponse?> GetCollaborationDocumentInviteInfoByDocumentId(Guid documentId);

        public Task<List<SearchUsersToInviteResponse>> GetUsersBySearchQuery(string? searchQuery, Guid documentId);

        public Task UpdateUserActiveStatus(Guid documentId, Guid personId, bool isActive);
        public Task<List<GetAllActiveCollaboratorsByDocumentId>> GetAllActiveCollaboratorsByDocumentId(Guid documentId);
        public Task<UpdateDocumentContentResponse?> UpdateDocumentContent(Guid documentId, Guid personId, string content);
        public Task<UpdateDocumentContentResponse?> GetDocumentContent(Guid documentId);
        public Task<UpdateDocumentContentResponse?> ApplyDocumentUpdate(DocumentUpdateRepositoryRequest update);
    }
}