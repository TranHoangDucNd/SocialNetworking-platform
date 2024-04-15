using WebDating.DTOs;
using WebDating.Entities;
using WebDating.Helpers;

namespace WebDating.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        //void DeleteMessageParams(MessageDeleteParams messageDeleteParams);
        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientUserName);
    }
}
