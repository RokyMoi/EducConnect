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
    public class MessageHub(IMessageRepository messageRepository,DataContext context,IMapper mapper):Hub
    {
        public async override Task OnConnectedAsync()
        {
            var HttpContext = Context.GetHttpContext();
            var OtherUser = HttpContext.Request.Query["user"];
            if (Context.User == null || string.IsNullOrEmpty(OtherUser)) throw new Exception("cannot join group");
            var groupName = GetGroupName(Context.User?.FindFirst(ClaimTypes.Email)?.Value, OtherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var messages = await messageRepository.GetMessageThread(Context.User?.FindFirst(ClaimTypes.Email)?.Value, OtherUser!);

            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
            Console.WriteLine($"Group: {groupName}, Messages: {messages.Count()}");

        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            if (string.IsNullOrWhiteSpace(createMessageDto.Content))
            {
                throw new HubException("Message content cannot be empty");
            }

            Caller caller = new Caller(Context.GetHttpContext());
            var callerEmail = caller.Email?.ToLower();


            if (callerEmail == createMessageDto.RecipientEmail.ToLower())
            {
                throw new HubException("You cant message yourself");
            }


            var senderMail = await context.PersonEmail.FirstOrDefaultAsync(x => x.Email == callerEmail);
            var recipientMail = await context.PersonEmail.FirstOrDefaultAsync(x => x.Email == createMessageDto.RecipientEmail.ToLower());


            if (senderMail == null || recipientMail == null)
            {
                throw new HubException("Sender or recipient email not found, cannot send message.");


            }


            var sender = await context.Person.FirstOrDefaultAsync(x => x.PersonId == senderMail.PersonId);
            var recipient = await context.Person.FirstOrDefaultAsync(x => x.PersonId == recipientMail.PersonId);


            if (sender == null || recipient == null)
            {
                throw new HubException("Sender or recipient not found");
            }


            var senderPhotoUrl = sender?.PersonPhoto?.FirstOrDefault()?.Url ?? "No User photo";
            var recipientPhotoUrl = recipient?.PersonPhoto?.FirstOrDefault()?.Url ?? "No User photo";


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


        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
        private string GetGroupName(string? caller, string? other)
        {
            var stringCompare = String.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
 
}

