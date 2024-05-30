using Microsoft.EntityFrameworkCore;
using WebDating.Entities.PostEntities;
using WebDating.Entities.UserEntities;
using WebDating.Helpers;
using WebDating.Interfaces;

namespace WebDating.Data
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DataContext _dataContext;

        public AdminRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void RemoveRange(IEnumerable<PostReportDetail> reports)
        {
            _dataContext.RemoveRange(reports);
        }
        public async Task<Post> GetPostByID(int postId)
        => await _dataContext
            .Posts.Where(x => x.Id == postId)
            .Include(x => x.Images)
            .Include(x => x.User)
                .ThenInclude(x => x.Photos)
            .FirstOrDefaultAsync();

        public async Task<IEnumerable<PostReportDetail>> GetPostReports()
        => await _dataContext.PostReportDetails
            .OrderByDescending(x => x.ReportDate)
            .ToListAsync();

        public async Task<List<PostReportDetail>> GetPostReports(int postId)
        => await _dataContext.PostReportDetails
          .Where(x => x.PostId == postId)
          .OrderByDescending(x => x.ReportDate)
          .ToListAsync();


    }
}
