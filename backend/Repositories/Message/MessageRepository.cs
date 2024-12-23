using AutoMapper;
using AutoMapper.QueryableExtensions;
using EduConnect.Data;
using EduConnect.DTOs.Messenger;
using EduConnect.Entities.Messenger;
using EduConnect.Helpers;
using EduConnect.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Repositories.MessageRepository
{
    public class MessageRepository(DataContext context,IMapper mapper) : IMessageRepository
    {
        public void AddMessage(Message message)
        {
            context.Message.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            context.Message.Remove(message);
        }

        public async Task<Message?> GetMessage(int messageid)
        {
           return await context.Message.FindAsync(messageid);
        }

        public async Task<PagedList<MessageDto>> GetMessageForUser(MessageParams messageParams)
            //Get the last message sent - > We filter that by MessageSent(Date)
        {
            var query = context.Message.OrderByDescending(x => x.MessageSent).AsQueryable();
        
            query = messageParams.Container switch
            {
                "Inbox" => query.Where(x => x.Recipient.PersonEmail.Email == messageParams.Email),
                "Outbox" => query.Where(x => x.Sender.PersonEmail.Email == messageParams.Email),
                _ => query.Where(x => x.Recipient.PersonEmail.Email == messageParams.Email && x.DateRead == null)
            };

            // Loguj trenutni upit za debag
            Console.WriteLine($"Query for container {messageParams.Container}: {query.ToQueryString()}");

            // Nastavi sa mapiranjem
            var messages = query.ProjectTo<MessageDto>(mapper.ConfigurationProvider);

            return await PagedList<MessageDto>.CreateASync(messages,messageParams.PageNumber,messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentEmail, string recipientEmail)
        {
      
            var messages = await context.Message
                .Include(x => x.Sender)  
                      .ThenInclude(x => x.PersonEmail)
                   .Include(x=> x.Sender)
                   .ThenInclude(x=> x.PersonPhoto)
                   
                .Include(x => x.Recipient)  
                    .ThenInclude(recipient => recipient.PersonEmail)
                     .Include(x => x.Recipient)
                   .ThenInclude(x => x.PersonPhoto)

                .Where(x => (x.Sender.PersonEmail.Email == currentEmail && x.Recipient.PersonEmail.Email == recipientEmail) ||
                            (x.Sender.PersonEmail.Email == recipientEmail && x.Recipient.PersonEmail.Email == currentEmail))
                .OrderBy(x => x.MessageSent)  
                .ToListAsync();

            var unreadCount = messages.Where(x => x.RecipientEmail == currentEmail && !x.DateRead.HasValue).ToList();

          

            if(unreadCount.Count > 0)
            {
                unreadCount.ForEach(x => x.DateRead = DateTime.Now);
                await context.SaveChangesAsync();
            }

            return messages.Select(x => new MessageDto
            {
                Id = x.Id,
                SenderId = x.Sender.PersonId,
                SenderEmail = x.Sender.PersonEmail.Email,
                SenderPhotoUrl = x.Sender.PersonPhoto.FirstOrDefault(photo => photo.PersonId == x.Sender.PersonId)?.Url ?? "No User photo",

                RecipientId = x.Recipient.PersonId,
                RecipientEmail = x.Recipient.PersonEmail.Email,
                RecipientPhotoUrl = x.Recipient.PersonPhoto.FirstOrDefault(photo => photo.PersonId == x.Recipient.PersonId)?.Url ?? "No User photo",
                DateRead = x.DateRead,
                Content = x.Content,
                MessageSent = x.MessageSent
            }).ToList();

        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync()>0;
        }
    }
}
