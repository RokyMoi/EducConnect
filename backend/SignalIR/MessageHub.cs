using AutoMapper;
using EduConnect.Data;
using EduConnect.DTOs.Messenger;
using EduConnect.Entities.Messenger;
using EduConnect.Helpers;
using EduConnect.Interfaces;
using MailKit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EduConnect.SignalIR
{
    public class MessageHub : Hub
    {
        private readonly IMessageRepository messageRepository;
        private readonly DataContext context;
        private readonly IMapper mapper;

        public MessageHub(IMessageRepository messageRepository, DataContext context, IMapper mapper)
        {
            this.messageRepository = messageRepository;
            this.context = context;
            this.mapper = mapper;
        }

        public async override Task OnConnectedAsync()
        {
            var HttpContext = Context.GetHttpContext();
            var OtherUser = HttpContext.Request.Query["user"];
            if (Context.User == null || string.IsNullOrEmpty(OtherUser)) throw new Exception("Cannot join group");

            var groupName = GetGroupName(Context.User?.FindFirst(ClaimTypes.Email)?.Value, OtherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var messages = await messageRepository.GetMessageThread(Context.User?.FindFirst(ClaimTypes.Email)?.Value, OtherUser!);
            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            if (string.IsNullOrWhiteSpace(createMessageDto.Content))
            {
                throw new HubException("Message content cannot be empty");
            }

            Caller caller = new Caller(Context.GetHttpContext());
            var callerEmail = caller.Email?.ToLower();

            var senderMail = await context.PersonEmail.FirstOrDefaultAsync(x => x.Email == callerEmail);
            if (senderMail == null)
            {
                throw new HubException("Sender email not found.");
            }

            var recipientMail = await context.PersonEmail.FirstOrDefaultAsync(x => x.Email == createMessageDto.RecipientEmail.ToLower());
            if (recipientMail == null)
            {
                throw new HubException("Recipient email not found.");
            }

            if (string.Equals(senderMail.Email, createMessageDto.RecipientEmail, StringComparison.OrdinalIgnoreCase))
            {
                throw new HubException("You cannot send a message to yourself.");
            }

            var sender = await context.Person.FirstOrDefaultAsync(x => x.PersonId == senderMail.PersonId);
            var recipient = await context.Person.FirstOrDefaultAsync(x => x.PersonId == recipientMail.PersonId);

            if (sender == null || recipient == null)
            {
                throw new HubException("Sender or recipient not found");
            }

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderEmail = senderMail.Email,
                RecipientEmail = recipientMail.Email,
                Content = createMessageDto.Content,
                MessageSent = DateTime.UtcNow
            };

            messageRepository.AddMessage(message);
            var saveSuccess = await messageRepository.SaveAllAsync();

            if (saveSuccess)
            {
                var group = GetGroupName(senderMail.Email, recipientMail.Email);
                await Clients.Group(group).SendAsync("NewMessage", mapper.Map<MessageDto>(message));
            }
            else
            {
                throw new HubException("Failed to save the message");
            }
        }

        private string GetGroupName(string? caller, string? other)
        {
            var stringCompare = String.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
}
