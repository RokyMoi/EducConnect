using System;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EduConnect.Data;
using EduConnect.DTOs.Messenger;
using EduConnect.Entities.Messenger;
using EduConnect.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EduConnect.SignalIR
{
 
    public class MessageHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<MessageHub> _logger;
        private static readonly ConcurrentDictionary<string, string> _connectionEmails = new ConcurrentDictionary<string, string>();

        public MessageHub(
            IMessageRepository messageRepository,
            DataContext context,
            IMapper mapper,
            ILogger<MessageHub> logger)
        {
            _messageRepository = messageRepository;
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var callerEmail = GetEmailFromToken();
                var otherUser = GetOtherUserFromQuery();

                _logger.LogInformation("User {CallerEmail} connecting to chat with {OtherUser}", callerEmail, otherUser);

                // Validate emails
                if (string.IsNullOrEmpty(callerEmail) || string.IsNullOrEmpty(otherUser))
                {
                    throw new HubException("Invalid user information");
                }

                // Store email for this connection
                _connectionEmails[Context.ConnectionId] = callerEmail;

                // Create group name and add user to group
                var groupName = GetGroupName(callerEmail, otherUser);
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                _logger.LogInformation("Added user to group: {GroupName}", groupName);

                // Get and send message thread
                var messages = await _messageRepository.GetMessageThread(callerEmail, otherUser);
                await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
                _logger.LogInformation("Sent message thread with {MessageCount} messages to group {GroupName}",
                    messages?.Count() ?? 0, groupName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in OnConnectedAsync");
                throw new HubException($"Error connecting to chat: {ex.Message}");
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                // Clean up stored email when disconnected
                if (_connectionEmails.TryRemove(Context.ConnectionId, out var email))
                {
                    _logger.LogInformation("User {Email} disconnected from message hub", email);
                }

                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in OnDisconnectedAsync");
                // Don't rethrow during disconnection
            }
        }

        public async Task SendMessage(CreateMessageDto dto)
        {
            try
            {
                // Input validation
                if (dto == null)
                {
                    throw new HubException("Message data is required");
                }

                // Get email from stored connection
                if (!_connectionEmails.TryGetValue(Context.ConnectionId, out var callerEmail))
                {
                    // Fallback if not found in dictionary
                    callerEmail = GetEmailFromToken();
                    _connectionEmails[Context.ConnectionId] = callerEmail;
                }

                // Validate message content
                if (string.IsNullOrWhiteSpace(dto.Content))
                {
                    throw new HubException("Message content cannot be empty");
                }

                // Validate recipient email
                if (string.IsNullOrWhiteSpace(dto.RecipientEmail))
                {
                    throw new HubException("Recipient email is required");
                }

                _logger.LogInformation("User {CallerEmail} sending message to {RecipientEmail}",
                    callerEmail, dto.RecipientEmail);

                // Load sender and recipient from database with proper error handling
                var senderMail = await _context.PersonEmail
                    .FirstOrDefaultAsync(x => x.Email.ToLower() == callerEmail.ToLower());
                if (senderMail == null)
                {
                    _logger.LogWarning("Sender email not found: {CallerEmail}", callerEmail);
                    throw new HubException("Sender email not found.");
                }

                var recipientMail = await _context.PersonEmail
                    .FirstOrDefaultAsync(x => x.Email.ToLower() == dto.RecipientEmail.ToLower());
                if (recipientMail == null)
                {
                    _logger.LogWarning("Recipient email not found: {RecipientEmail}", dto.RecipientEmail);
                    throw new HubException("Recipient email not found.");
                }

                if (string.Equals(senderMail.Email, recipientMail.Email, StringComparison.OrdinalIgnoreCase))
                {
                    throw new HubException("You cannot send a message to yourself.");
                }

                var sender = await _context.Person
                    .FirstOrDefaultAsync(x => x.PersonId == senderMail.PersonId);
                var recipient = await _context.Person
                    .FirstOrDefaultAsync(x => x.PersonId == recipientMail.PersonId);
                if (sender == null || recipient == null)
                {
                    _logger.LogWarning("Sender or recipient not found");
                    throw new HubException("Sender or recipient not found.");
                }

                // Create and save message with proper sanitization
                var message = new Message
                {
                    Sender = sender,
                    Recipient = recipient,
                    SenderEmail = senderMail.Email,
                    RecipientEmail = recipientMail.Email,
                    Content = dto.Content.Trim(),
                    MessageSent = DateTime.UtcNow
                };

                _messageRepository.AddMessage(message);
                if (!await _messageRepository.SaveAllAsync())
                {
                    _logger.LogError("Failed to save message to database");
                    throw new HubException("Failed to save the message");
                }

                // Broadcast message to group
                var group = GetGroupName(senderMail.Email, recipientMail.Email);
                await Clients.Group(group).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
                _logger.LogInformation("Message sent successfully to group {Group}", group);
            }
            catch (HubException)
            {
                // Let HubExceptions bubble up as they contain user-friendly messages
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message");
                throw new HubException("An error occurred sending the message. Please try again.");
            }
        }

        private string GetEmailFromToken()
        {
            try
            {
                var httpContext = Context.GetHttpContext()
                    ?? throw new HubException("No HttpContext available");

                // First try to get from query string for WebSocket connections
                var rawToken = httpContext.Request.Query["access_token"].ToString();

                // If not found in query, try Authorization header
                if (string.IsNullOrEmpty(rawToken))
                {
                    var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
                    if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                    {
                        rawToken = authHeader.Substring("Bearer ".Length).Trim();
                    }
                }

                if (string.IsNullOrEmpty(rawToken))
                {
                    _logger.LogWarning("Access token not found in request");
                    throw new HubException("Access token not found");
                }

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(rawToken); // just parse, don't validate

                var email = jwt.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "email")
                    ?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogWarning("Email claim not found in token");
                    throw new HubException("Email claim not found in token");
                }

                return email;
            }
            catch (HubException)
            {
                throw; // Let HubExceptions bubble up as they contain user-friendly messages
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to extract email from token");
                throw new HubException("Error authenticating user");
            }
        }

        private string GetOtherUserFromQuery()
        {
            try
            {
                var httpContext = Context.GetHttpContext()
                    ?? throw new HubException("No HttpContext available");

                var otherUser = httpContext.Request.Query["user"].ToString();
                if (string.IsNullOrEmpty(otherUser))
                {
                    _logger.LogWarning("Other user not specified in query");
                    throw new HubException("Other user not specified");
                }

                return otherUser;
            }
            catch (HubException)
            {
                throw; // Let HubExceptions bubble up as they contain user-friendly messages
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get other user from query");
                throw new HubException("Error identifying chat recipient");
            }
        }

        private static string GetGroupName(string caller, string other)
        {
            // Ensure consistent group naming regardless of who connects first
            return string.CompareOrdinal(caller, other) < 0
                ? $"{caller}-{other}"
                : $"{other}-{caller}";
        }
    }
}