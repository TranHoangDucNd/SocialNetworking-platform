using AutoMapper;
using WebDating.Interfaces;

namespace WebDating.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UnitOfWork(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IUserRepository UserRepository => new UserRepository(_context, _mapper);

        public ILikeRepository LikeRepository => new LikeRepository(_context);

        public IMessageRepository MessageRepository => new MessageRepository(_context, _mapper);

        public IDatingRepository DatingRepository => new DatingRepository(_context, _mapper);

        public IPostRepository PostRepository => new PostRepository(_context);

        public async Task<bool> Complete()
        {
           return await _context.SaveChangesAsync() > 0;
        }

        public bool CompleteNotAsync()
        {
            return _context.SaveChanges() > 0;
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}
