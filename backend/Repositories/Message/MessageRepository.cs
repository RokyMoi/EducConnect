using EduConnect.Data;
using EduConnect.DTOs.Messenger;
using EduConnect.Entities.Messenger;
using EduConnect.Helpers;
using EduConnect.Interfaces;

namespace EduConnect.Repositories.MessageRepository
{
    public class MessageRepository(DataContext context) : IMessageRepository
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

        public Task<PagedList<MessageDto>> GetMessageForUser()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MessageDto>> GetMessageThread(string currentEmail, string recipientEmail)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync()>0;
        }
    }
}
