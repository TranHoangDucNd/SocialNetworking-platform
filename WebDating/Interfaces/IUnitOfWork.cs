namespace WebDating.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        ILikeRepository LikeRepository { get; }
        IMessageRepository MessageRepository { get; }
        IDatingRepository DatingRepository { get; }
        IPostRepository PostRepository { get; }
        IAdminRepository AdminRepository { get; }
        ICommentRepository CommentRepository { get; }
        IReactionLogRepository ReactionLogRepository { get; }
        Task<bool> Complete();
        bool CompleteNotAsync();
        bool HasChanges();
    }
}
