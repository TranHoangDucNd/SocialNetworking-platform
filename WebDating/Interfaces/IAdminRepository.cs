using WebDating.Entities.PostEntities;
using WebDating.Entities.UserEntities;
using WebDating.Helpers;

namespace WebDating.Interfaces
{
    public interface IAdminRepository : IBaseDeleteRepository<Post>
    {
        Task<IEnumerable<PostReportDetail>> GetPostReports();
        Task<Post> GetPostByID(int postId);

    }
}
