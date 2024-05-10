using WebDating.Entities.PostEntities;
using WebDating.Entities.UserEntities;

namespace WebDating.Interfaces
{
    public interface ICommentRepository
    {
        void Insert(Comment comment);
        void Update(Comment comment);
        void Delete(int id);
        Comment GetById(int commentId);
        IEnumerable<Comment> GetByPostId(int postId);
        long GetTotalReactsByCommentId(int commentId);
        Dictionary<ReactionType, int> AggrateByComment(int commentId);
    }
}
