namespace WebDating.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        ILikeRepository LikeRepository { get; }
        IMessageRepository MessageRepository { get; }
        IDatingRepository DatingRepository { get; }
        Task<bool> Complete();
        bool CompleteNotAsync();
        bool HasChanges();
    }
}
