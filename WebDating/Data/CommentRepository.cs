using Microsoft.EntityFrameworkCore;
using WebDating.Entities.PostEntities;
using WebDating.Entities.UserEntities;
using WebDating.Interfaces;

namespace WebDating.Data
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _context;

        public CommentRepository(DataContext context)
        {
            _context = context;
        }

        public void Delete(int id)
        {
            Comment comment = _context.Comments.Find(id);
            _context.Comments.Remove(comment);
        }

        public Comment GetById(int commentId)
        {
            return _context.Comments.Find(commentId);
        }

        public IEnumerable<Comment> GetByPostId(int postId)
        {
            return _context.Comments.Where(it => it.PostId == postId);
        }

        public void Insert(Comment comment)
        {
            _context.Comments.Add(comment);
        }

        public void Update(Comment comment)
        {
            _context.Comments.Update(comment);
        }

        public long GetTotalReactsByCommentId(int commentId)
        {
            return _context.ReactionLogs
               .Where(it => it.CommentId == commentId)
               .LongCount();
        }

        public Dictionary<ReactionType, int> AggrateByComment(int commentId)
        {
            IQueryable<ReactionLog> reacts = _context.ReactionLogs
                 .Where(it => it.CommentId == commentId);
            return reacts.GroupBy(it => it.ReactionType)
                 .ToDictionary(it => it.Key, it => it.Count());
        }
    }
}
