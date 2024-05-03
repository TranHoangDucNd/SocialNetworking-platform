using Microsoft.EntityFrameworkCore;
using WebDating.Entities.PostEntities;
using WebDating.Entities.UserEntities;
using WebDating.Interfaces;

namespace WebDating.Data
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _context;

        public PostRepository(DataContext context)
        {
            _context = context;
        }
        public void Delete(Post entity)
        => _context.Posts.Remove(entity);

        public void DeleteImages(ICollection<ImagePost> images)
        {
            _context.ImagePosts.RemoveRange(images);
        }

        public async Task<AppUser> FindByNameAsync(string name)
        => await _context.Users.Where(x => x.Equals(name)).Include(x => x.Photos).FirstOrDefaultAsync();

        public async Task<IEnumerable<Post>> GetAll()
        => await _context.Posts
            .Include(x => x.PostLikes)
            .Include(x => x.Images)
            .Include(x => x.User)
            .ThenInclude(x => x.Photos)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();



        public async Task<Post> GetById(int id)
        => await _context.Posts.Where(x => x.Id == id).Include(x => x.Images).FirstOrDefaultAsync();


        public async Task<IEnumerable<Post>> GetMyPost(int id)
        => await _context.Posts.Where(x => x.User.Id == id)
            .OrderByDescending(x => x.CreatedAt)
            .Include(x => x.PostLikes)
            .Include(x => x.Images)
            .Include(x => x.User)
            .ThenInclude(x => x.Photos)
            .ToListAsync();


        public async Task<Post> Insert(Post entity)
        {
            await _context.Posts.AddAsync(entity);
            return entity;
        }


        public async Task InsertImagePost(ImagePost imagePost)
        => await _context.ImagePosts.AddAsync(imagePost);

        public void Update(Post entity)
        => _context.Posts.Update(entity);


        public async Task InsertComment(PostComment comment)
        => await _context.PostsComments.AddAsync(comment);


        public async Task<PostComment> GetCommentById(int id)
        => await _context.PostsComments.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<PostComment>> GetCommentsByPostId(int id)
        => await _context.PostsComments.Where(x => x.PostId == id)
                .Include(x => x.PostSubComments)
                .Include(x => x.User)
                    .ThenInclude(x => x.Photos)
                .OrderByDescending(x => x.Id)
                .ToListAsync();

        public void UpdateComment(PostComment postComment)
        => _context.PostsComments.Update(postComment);

        public void DeleteComment(PostComment comment)
        => _context.PostsComments.Remove(comment);

    }
}
