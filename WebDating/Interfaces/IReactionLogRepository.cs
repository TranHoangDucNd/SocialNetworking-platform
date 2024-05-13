using WebDating.Entities.PostEntities;

namespace WebDating.Interfaces
{
    public interface IReactionLogRepository
    {
        void Insert(ReactionLog entity);
        void Remove(int id);
        void Remove(ReactionLog entity);

        ReactionLog GetReactUserByComment(int userId, int commentId);
        ReactionLog GetReactUserByPost(int userId, int postId);

        Task<List<ReactionLog>> GetDetailReaction(int targetId);

        List<ReactionLog> GetByComment(int commentId);
        List<ReactionLog> GetByComments(IEnumerable<int> commentIds);
        List<ReactionLog> GetByPost(int postId);
    }
}
