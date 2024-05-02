using WebDating.DTOs;
using WebDating.Entities.UserEntities;
using WebDating.Helpers;

namespace WebDating.Interfaces
{
    public interface ILikeRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int tagetUserId);
        Task<AppUser> GetUserWithLike(int userId);
        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
    }
}
