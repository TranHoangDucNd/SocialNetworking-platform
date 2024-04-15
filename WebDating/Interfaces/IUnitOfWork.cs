namespace WebDating.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        ILikeRepository LikeRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}
