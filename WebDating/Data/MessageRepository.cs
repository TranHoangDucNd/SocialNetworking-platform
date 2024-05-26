using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebDating.DTOs;
using WebDating.Entities.MessageEntities;
using WebDating.Helpers;
using WebDating.Interfaces;


namespace WebDating.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
     

        public MessageRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public async Task DeleteAllMessageByUserId(string currentUsername, string otherUsername)
        {
            var messages = await _context.Messages
                .Where(mes => (mes.SenderUsername == currentUsername && mes.RecipientUsername == otherUsername)
                || (mes.SenderUsername == otherUsername && mes.RecipientUsername == currentUsername)).ToListAsync();

            foreach(Message message in messages)
            {
                if(message.SenderUsername == currentUsername)
                {
                    message.SenderDeleted = true;
                }
                if(message.RecipientUsername == currentUsername)
                {
                    message.RecipientDeleted = true;
                }
            }
            var messageToDelete = messages.Where(m => m.SenderDeleted && m.RecipientDeleted).ToList();
            _context.Messages.RemoveRange(messageToDelete);
           //bool success =  await _uow.Complete();
           // return success ? new SuccessResult<string>("Successfully deleted message") : new ErrorResult<string>("Delete message failed");

        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _context.Connections.FindAsync(connectionId);
        }

        public async Task<Group> GetGroupForConnection(string connectionId)
        {
            return await _context.Groups
                .Include(x => x.Connections)
                .Where(x => x.Connections.Any(c => c.ConnectionId == connectionId))
                .FirstOrDefaultAsync();
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await _context.Groups
                .Include(x => x.Connections)
                .FirstOrDefaultAsync(x => x.Name == groupName);
        }

        public List<MessageSummaryDto> GetMessages(string currentUsername)
        {
            var messages = _context.Messages
                 .Include(m => m.Sender.Photos)
                 .Include(m => m.Recipient.Photos)
                 .Where(m => m.RecipientUsername == currentUsername || m.SenderUsername == currentUsername)
                 .OrderByDescending(m => m.MessageSent)
                 .ToList() // Fetch data from database first
                 .GroupBy(m => m.SenderUsername == currentUsername ? m.RecipientUsername : m.SenderUsername)
                 .Select(g => g.OrderByDescending(m => m.MessageSent).FirstOrDefault())
                 .Select(m => new MessageSummaryDto
                 {
                     Username = m.SenderUsername == currentUsername
                         ? m.RecipientUsername
                         : m.SenderUsername,
                     LastMessageContent = m.Content,
                     LastMessageSent = m.MessageSent,
                     Url = m.SenderUsername == currentUsername
                         ? m.Recipient?.Photos?.FirstOrDefault(p => p.IsMain)?.Url
                         : m.Sender?.Photos?.FirstOrDefault(p => p.IsMain)?.Url,
                     KnownAs = m.SenderUsername == currentUsername ? m.Recipient.KnownAs : m.Sender.KnownAs,
                     DateRead = m.RecipientUsername == currentUsername && m.DateRead == null
                 }).ToList();
            return messages;
        }


        //Get các tin nhắn đã gửi, đã nhận, chưa đọc của current user với other users
        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messages
                .OrderByDescending(m => m.MessageSent)
                .AsQueryable();


            switch (messageParams.Container)
            {
                case "Inbox":
                    var latestMessagesInbox = query
                        .Where(m => m.RecipientUsername == messageParams.Username && m.RecipientDeleted == false)
                        .GroupBy(m => m.SenderUsername)
                        .Select(g => g.OrderByDescending(m => m.MessageSent).FirstOrDefault());
                    query = query.Where(m => latestMessagesInbox.Any(lm => lm.Id == m.Id));
                    break;
                case "Outbox":
                    var latestMessagesOutbox = query
                        .Where(m => m.SenderUsername == messageParams.Username && m.SenderDeleted == false)
                        .GroupBy(m => m.RecipientUsername)
                        .Select(g => g.OrderByDescending(m => m.MessageSent).FirstOrDefault());

                    query = query.Where(m => latestMessagesOutbox.Any(lm => lm.Id == m.Id)); // Chỉ lấy ra các tin nhắn nằm trong danh sách tin nhắn cuối cùng
                    break;
                default:
                    query = query.Where(m => m.RecipientUsername == messageParams.Username
                        && m.RecipientDeleted == false && m.DateRead == null);
                    break;
            }



            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

            return await PagedList<MessageDto>
                .CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);

        }


        //Các message từ current user tới người dùng họ đang xem
        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientUserName)
        {
            var query = _context.Messages
                .Where(
                    m => m.RecipientUsername == currentUserName && m.RecipientDeleted == false &&
                    m.SenderUsername == recipientUserName ||
                    m.RecipientUsername == recipientUserName && m.SenderDeleted == false &&
                    m.SenderUsername == currentUserName
                )
                .OrderBy(m => m.MessageSent)
                .AsQueryable();

            //ds message chưa đọc
            var unreadMessages = query
                .Where(m => m.DateRead == null && m.RecipientUsername == currentUserName)
                .ToList();
            //ý tưởng khi 1 người vào mục tin nhắn của họ vs ng kia thì 
            //các tin nhắn chưa đọc lúc này sẽ update lại thời gian đọc lúc nào
            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.DateRead = DateTime.UtcNow;
                }

            }

            return await query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }

    }
}
