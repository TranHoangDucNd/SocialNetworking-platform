using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.Design;
//using WebDating.Data.Migrations;
using WebDating.Entities.PostEntities;
using WebDating.Entities.UserEntities;
using WebDating.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebDating.Data
{
    public class ReactionLogRepository : IReactionLogRepository
    {
        private readonly DataContext _context;

        public ReactionLogRepository(DataContext context)
        {
            _context = context;
        }

        public void Insert(ReactionLog entity)
        {
            _context.ReactionLogs.Add(entity);
        }
        public void Remove(int id)
        {
            ReactionLog react = _context.ReactionLogs.Find(id);
            if (react != null)
            {
                _context.ReactionLogs.Remove(react);
            }
        }

        public void Remove(ReactionLog entity)
        {
            _context.ReactionLogs.Remove(entity);
        }

        public ReactionLog GetReactUserByComment(int userId, int commentId)
        {
            return _context.ReactionLogs
                .FirstOrDefault(it => it.UserId == userId && it.CommentId == commentId && it.Target == ReactTarget.Comment);
        }
        public ReactionLog GetReactUserByPost(int userId, int postId)
        {
            return _context.ReactionLogs
                .FirstOrDefault(it => it.UserId == userId && it.PostId == postId && it.Target == ReactTarget.Post);
        }

        public List<ReactionLog> GetByComment(int commentId)
        {
            return _context.ReactionLogs
                .Where(it => it.Target == ReactTarget.Comment && it.CommentId == commentId)
                .ToList();
        }
        public List<ReactionLog> GetByComments(IEnumerable<int> commentIds)
        {
            return _context.ReactionLogs
               .Where(it => it.Target == ReactTarget.Comment && it.CommentId.HasValue && commentIds.Contains(it.CommentId.Value))
               .ToList();
        }

        public List<ReactionLog> GetByPost(int postId)
        {
            return _context.ReactionLogs
                .Where(it => it.Target == ReactTarget.Post && it.PostId == postId)
                .ToList();
        }
        
        public Task<List<ReactionLog>> GetDetailReactionForPost(int targetId)
        {
            return _context.ReactionLogs
                .Where(it => it.PostId == targetId)
                .ToListAsync();
        }
        
        public Task<List<ReactionLog>> GetDetailReactionForComment(int targetId)
        {
            return _context.ReactionLogs
                .Where(it => it.CommentId == targetId)
                .ToListAsync();
        }
    }
}
