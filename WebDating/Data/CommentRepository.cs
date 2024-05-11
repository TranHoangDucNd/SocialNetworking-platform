using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
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
            Comment cmt = _context.Comments.Find(commentId);
            _context.Entry(cmt).Collection(p => p.ReactionLogs).Load();
            return cmt;
        }

        public async Task<List<Comment>> GetByPostId(int postId)
        {
            return await _context.Comments
                .Include(it => it.ReactionLogs)
                .Where(it => it.PostId == postId)
                .ToListAsync();
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

        public async Task<List<Comment>> GetCommentRecursive(int commentId)
        {
            List<Comment> comments = new List<Comment>();
            List<Comment> childComments = await _context.Comments
                .Include(it => it.ReactionLogs)
                .Where(it => it.ParentId == commentId)
                .ToListAsync();
            foreach (Comment child in childComments)
            {
                comments.Add(child);
                var tmp = await GetCommentRecursive(child.Id);
                comments.AddRange(tmp);
                //comments.AddRange(_context.Comments
                //    .Where(it => it.ParentId == child.ParentId)
                //    .ToList());
            }
            return comments;
        }


    }
}
