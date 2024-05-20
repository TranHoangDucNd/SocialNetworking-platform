using WebDating.Entities.PostEntities;
using WebDating.Entities.UserEntities;

namespace WebDating.Interfaces
{
    public interface IPostRepository : IBaseDeleteRepository<Post>
        , IBaseUpdateRepository<Post>, IBaseGetByIdRepository<Post>,
        IBaseInsertRepository<Post>, IBaseGetAllRepository<Post>
    {
        void DeleteImages(ICollection<ImagePost> images);
        Task<AppUser> FindByNameAsync(string name);
        Task<IEnumerable<Post>> GetMyPost(int id);
        Task InsertImagePost(ImagePost imagePost);
        Task<IEnumerable<PostReportDetail>> GetAllReport();
        Task InsertPostReport(PostReportDetail report);
        Task<PostReportDetail> GetReport(int userId, int postId);
        void UpdatePostReport(PostReportDetail check);
    }
}
