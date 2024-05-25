using Microsoft.EntityFrameworkCore;
using WebDating.Entities.UserEntities;
using WebDating.Interfaces;

namespace WebDating.Data
{
    public class DatingRequestRepository : IDatingRequestRepository
    {
        private readonly DataContext _context;

        public DatingRequestRepository(DataContext context)
        {
            _context = context;
        }
        public DatingRequest Get(int id)
        {
            return _context.DatingRequests.Find(id);
        }

        public async Task<List<DatingRequest>> GetBySender(int senderId)
        {
            return await _context.DatingRequests
                .Where(x => x.SenderId == senderId)
                .ToListAsync();
        }

        public async Task<DatingRequest> GetIfConfirmedRelationship(int userId)
        {
            return await _context.DatingRequests
                .Where(it => (it.SenderId == userId || it.CrushId == userId) && it.Status == DatingStatus.Confirmed)
                .FirstOrDefaultAsync();
        }

        public DatingRequest GetIfDeniedBefore(int userId)
        {
            return _context.DatingRequests
                .AsEnumerable() //lấy ra d/s từ db rồi mới xét điều kiện lấy
                .Where(it => (it.SenderId == userId || it.CrushId == userId) 
                    && it.Status == DatingStatus.Denied && new TimeSpan(DateTime.Now.Ticks - it.ConfirmedDate.Ticks).TotalDays < 7)
                .FirstOrDefault();
        }
        public int InsertAndGetId(DatingRequest entity)
        {
            _context.DatingRequests.Add(entity);
            _context.SaveChanges();
            return entity.Id;
        }

        public async Task<DatingRequest> GetIfExistWaiting(int userId1, int userId2)
        {
            return await _context.DatingRequests
                .Where(it => ((it.SenderId == userId2 && it.CrushId == userId1)
                    || (it.SenderId == userId1 && it.CrushId == userId2))
                    && it.Status == DatingStatus.Waiting)
                .FirstOrDefaultAsync();
        }

        public async Task<DatingRequest> GetWaitingRequest(int senderId, int crushId)
        {
            return await _context.DatingRequests
                .Where(it => it.SenderId == senderId && it.CrushId == crushId && it.Status == DatingStatus.Waiting)
                .FirstOrDefaultAsync();
        }

        public void Insert(DatingRequest entity)
        {
            _context.DatingRequests.Add(entity);
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
