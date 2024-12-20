using EduConnect.Entities.Messenger;

namespace EduConnect.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);

        Task<Message?> GetMessage(int messageid);

    }
}
