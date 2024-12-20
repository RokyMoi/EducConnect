using EduConnect.DTOs.Messenger;
using EduConnect.Entities.Messenger;
using EduConnect.Helpers;

namespace EduConnect.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);

        Task<Message?> GetMessage(int messageid);
        Task<PagedList<MessageDto>> GetMessageForUser();
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentEmail,string recipientEmail);
        Task<bool> SaveAllAsync();

    }
}
