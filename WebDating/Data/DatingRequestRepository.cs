using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.Design;
using WebDating.Data.Migrations;
using WebDating.Entities.PostEntities;
using WebDating.Entities.ProfileEntities;
using WebDating.Entities.UserEntities;
using WebDating.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebDating.Data
{
    public class DatingRequestRepository : IDatingRequestRepository
    {
        private readonly DataContext _context;

        public DatingRequestRepository(DataContext context)
        {
            _context = context;
        }

        public void Insert(DatingRequest entity)
        {
            _context.DatingRequests.Add(entity);
        }
        
        public int InsertAndGetId(DatingRequest entity)
        {
            _context.DatingRequests.Add(entity);
            _context.SaveChanges();
            return entity.Id;
        }
        
        public void Remove(int id)
        {
            DatingRequest react = _context.DatingRequests.Find(id);
            if (react != null)
            {
                _context.DatingRequests.Remove(react);
            }
        }

        public void Remove(DatingRequest entity)
        {
            _context.DatingRequests.Remove(entity);
        }

        public async Task<List<DatingRequest>> GetBySender(int senderId)
        {
            return await _context.DatingRequests
                .Where(it => it.SenderId == senderId)
                .ToListAsync();
        }

        public DatingRequest Get(int id)
        {
            return _context.DatingRequests.Find(id);
        }

        public async Task<DatingRequest> GetIfInConfirmedRelationship(int userId)
        {
            return await _context.DatingRequests
                .Where(it => (it.SenderId == userId || it.CrushId == userId) && it.Status == DatingStatus.Confirmed)
                 .FirstOrDefaultAsync();


        }

        public async Task<DatingRequest> GetIfExistWaiting(int userId1, int userId2)
        {
            return await _context.DatingRequests
                .Where(it => ((it.SenderId == userId2 && it.CrushId == userId1)
                || (it.SenderId == userId1 && it.CrushId == userId2))
                && it.Status == DatingStatus.Waiting)
                 .FirstOrDefaultAsync();
        }

        public DatingRequest GetIfDeniedBefore(int userId)
        {
            return _context.DatingRequests
                .AsEnumerable()
              .Where(it => (it.SenderId == userId || it.CrushId == userId) && it.Status == DatingStatus.Denied && new TimeSpan(DateTime.Now.Ticks - it.ConfirmedDate.Ticks).TotalDays < 7)
               .FirstOrDefault();
        }

        public async Task<DatingRequest> GetWaitingRequest(int senderId, int crushId)
        {
            return await _context.DatingRequests
             .Where(it => it.SenderId == senderId && it.CrushId == crushId && it.Status == DatingStatus.Waiting)
              .FirstOrDefaultAsync();
        }

        public void RemoveAllWaitingRequest(int userId, int excludeId)
        {
            IEnumerable<DatingRequest> waitingRequest = _context.DatingRequests
                  .Where(it => it.Id != excludeId && (it.SenderId == userId || it.CrushId == userId) && it.Status == DatingStatus.Waiting);

            _context.DatingRequests.RemoveRange(waitingRequest);


        }

        public void RemoveIfMeDenied(int userId, int crushId)
        {
            IEnumerable<DatingRequest> deniedRequest = _context.DatingRequests
                 .Where(it => it.SenderId == userId && it.CrushId == crushId && it.Status == DatingStatus.Denied);

            _context.DatingRequests.RemoveRange(deniedRequest);
        }
    }
}
