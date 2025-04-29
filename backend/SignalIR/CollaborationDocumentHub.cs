using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EduConnect.Data;
using EduConnect.DTOs;
using EduConnect.Helpers;
using EduConnect.Interfaces;
using EduConnect.Middleware;
using EduConnect.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace EduConnect.SignalIR
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CollaborationDocumentHub(DataContext dataContext, IHttpContextAccessor httpContextAccessor, ILogger<CollaborationDocumentHub> logger, ICollaborationDocumentRepository collaborationDocumentRepository) : Hub
    {
        private readonly DataContext _dataContext = dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogger<CollaborationDocumentHub> _logger = logger;
        private readonly ICollaborationDocumentRepository _collaborationDocumentRepository = collaborationDocumentRepository;

        private readonly ConcurrentDictionary<string, Guid> _connections = new();
        private readonly ConcurrentDictionary<Guid, List<Guid>> _documentGroups = new();

        public override async Task OnConnectedAsync()
        {

            var personId = GetPersonIdFromToken();

            _connections.TryAdd(Context.ConnectionId, personId);
            _logger.LogInformation($"User {personId} connected with connection ID {Context.ConnectionId}");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string connectionId = Context.ConnectionId;

            if (_connections.TryRemove(connectionId, out var personId))
            {
                _logger.LogInformation($"User {personId} disconnected with connection ID {connectionId}");

                var documentIds = _documentGroups
                    .Where(x => x.Value.Contains(personId))
                    .Select(x => x.Key)
                    .ToList();

                foreach (var documentId in documentIds)
                {
                    _documentGroups[documentId].Remove(personId);
                    await _collaborationDocumentRepository.UpdateUserActiveStatus(documentId, personId, false);
                    await Clients.Group(documentId.ToString()).SendAsync("UserLeft", $"User {connectionId} left the group {documentId}");
                    await this.GetActiveDocumentCollaborators(documentId);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
        public async Task JoinDocumentGroup(Guid documentId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, documentId.ToString());
            _logger.LogInformation($"User {Context.ConnectionId} joined the group {documentId}");
            var personId = GetPersonIdFromToken();
            await _collaborationDocumentRepository.UpdateUserActiveStatus(documentId, personId, true);
            _documentGroups.AddOrUpdate(documentId, new List<Guid> { personId }, (key, list) =>
            {
                if (!list.Contains(personId))
                {
                    list.Add(personId);
                }
                return list;
            });

            _logger.LogInformation($"User {personId} joined the group {documentId} and added to the list of active users for this document.");

            foreach (var documentGroup in _documentGroups)
            {
                _logger.LogInformation($"Document ID: {documentGroup.Key}, Active Users: {string.Join(", ", documentGroup.Value)}");
            }

            await Clients.Group(documentId.ToString()).SendAsync("UserJoined", $"User {Context.ConnectionId} joined the group {documentId}");

            var document = await _collaborationDocumentRepository.GetDocumentByIdForHub(documentId);

            await Clients.Caller.SendAsync("GetInitialDocumentContent", document);

        }

        public async Task LeaveDocumentGroup(Guid documentId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, documentId.ToString());
            _logger.LogInformation($"User {Context.ConnectionId} left the group {documentId}");
            var personId = GetPersonIdFromToken();

            await _collaborationDocumentRepository.UpdateUserActiveStatus(documentId, personId, false);

            _documentGroups.AddOrUpdate(documentId, new List<Guid> { personId }, (key, list) =>
            {
                if (list.Contains(personId))
                {
                    list.Remove(personId);
                }
                return list;
            });
            _logger.LogInformation($"User {personId} left the group {documentId} and removed from the list of active users for this document.");

            foreach (var documentGroup in _documentGroups)
            {
                _logger.LogInformation($"Document ID: {documentGroup.Key}, Active Users: {string.Join(", ", documentGroup.Value)}");
            }

            await Clients.Group(documentId.ToString()).SendAsync("UserLeft", $"User {Context.ConnectionId} left the group {documentId}");

            await this.GetActiveDocumentCollaborators(documentId);
        }

        public async Task GetActiveDocumentCollaborators(Guid documentId)
        {
            var activeCollaborators = await _collaborationDocumentRepository.GetAllActiveCollaboratorsByDocumentId(documentId);
            await Clients.Group(documentId.ToString()).SendAsync("ActiveCollaborators", activeCollaborators);
        }

        public async Task UpdateDocumentContent(Guid documentId, string content)
        {
            var personId = GetPersonIdFromToken();

            if (!await this.ValidateAccessRights(documentId, personId))
            {
                throw new HubException("You do not have access to this document.");
            }


            var result = await _collaborationDocumentRepository.UpdateDocumentContent(documentId, personId, content);

            _logger.LogInformation($"Sending document update to group {documentId}");

            await Clients.GroupExcept(documentId.ToString(), Context.ConnectionId).SendAsync("GetDocumentUpdate", content);
        }




        private Guid GetPersonIdFromToken()
        {

            var user = Context.User;
            if (user == null)
            {
                _logger.LogError("User context is null");
                throw new HubException("User context is null");
            }
            PrintObjectUtility.PrintObjectProperties(Context.User);
            Console.WriteLine("User claims:");
            PrintObjectUtility.PrintObjectProperties(Context.User.Claims);
            var publicPersonId = Guid.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var person = _dataContext.Person.Where(x => x.PersonPublicId == publicPersonId).FirstOrDefault();

            if (person == null)
            {
                _logger.LogError($"Person not found for public ID: {publicPersonId}");
                throw new HubException("Person not found");

            }

            return person.PersonId;
        }

        private async Task<bool> ValidateAccessRights(Guid documentId, Guid personId)
        {
            return await _dataContext
            .CollaborationDocumentParticipant
            .Include(x => x.Document)
            .Where(x => x.Document.DocumentId == documentId && (x.ParticipantPersonId == personId || x.Document.CreatedByPersonId == personId))
            .AnyAsync();
        }


    }
}