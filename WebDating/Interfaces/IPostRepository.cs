using WebDating.Entities;

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
    }
}
