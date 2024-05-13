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
        Task<List<Comment>> GetByPostId(int postId);
        long GetTotalReactsByCommentId(int commentId);
        Dictionary<ReactionType, int> AggrateByComment(int commentId);

        /// <summary>
        /// Lấy tất cả comment descendants theo id comment cha
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        //Task<List<Comment>> GetCommentRecursive(int commentId);
    }
    
}
