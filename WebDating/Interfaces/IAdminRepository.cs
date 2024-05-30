using WebDating.Entities.PostEntities;
using WebDating.Entities.UserEntities;
using WebDating.Helpers;

namespace WebDating.Interfaces
{
    public interface IAdminRepository
    {
        Task<IEnumerable<PostReportDetail>> GetPostReports();
        Task<Post> GetPostByID(int postId);
        Task<List<PostReportDetail>> GetPostReports(int postId);
        void RemoveRange(IEnumerable<PostReportDetail> reports);
    }
}
