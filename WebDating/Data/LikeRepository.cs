using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebDating.DTOs;
using WebDating.Entities.UserEntities;
using WebDating.Extensions;
using WebDating.Helpers;
using WebDating.Interfaces;

namespace WebDating.Data
{
    public class LikeRepository : ILikeRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public LikeRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AppUser> GetUserWithLike(int id)
        {
            return await _context.Users
                .Include(x => x.LikedUsers)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<UserLike> GetUserLike(int sourceUserId, int targetUserId)
        {
            return await _context.Likes.FirstOrDefaultAsync(x => x.SourceUserId == sourceUserId && x.TargetUserId == targetUserId);
        }

        public async Task<PagedList<MemberDto>> GetUserLikes(LikesParams likesParams)
        {
            var likes = _context.Likes.AsQueryable();
            var users = _context.Users.OrderBy(x => x.UserName).AsQueryable();

            if (likesParams.Predicate == "liked")
            {
                likes = likes.Where(x => x.SourceUserId == likesParams.UserId);
                users = likes.Select(x => x.TargetUser);
            }

            if (likesParams.Predicate == "likedBy")
            {
                likes = likes.Where(x => x.TargetUserId == likesParams.UserId);
                users = likes.Select(x => x.SourceUser);
            }

            if (likesParams.Predicate == "mutualLike")
            {
                var likedUsers = likes.Where(x => x.SourceUserId == likesParams.UserId)
                    .Select(x => x.TargetUserId);

                var likedByUser = likes.Where(x => x.TargetUserId == likesParams.UserId)
                    .Select(x => x.SourceUserId);

                var mutualLike = likedUsers.Intersect(likedByUser);

                users = users.Where(x => mutualLike.Contains(x.Id));

            }

            //var result = users.Select(user => new LikeDto
            //{
            //    Id = user.Id,
            //    UserName = user.UserName,
            //    Age = user.DateOfBirth.CaculateAge(),
            //    City = user.City,
            //    KnownAs = user.KnownAs,
            //    PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url
            //});

            var result = await PagedList<MemberDto>
                .CreateAsync(users.AsNoTracking()
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider), likesParams.PageNumber, likesParams.PageSize);
            foreach (var member in result)
            {
                member.DatingProfile = await GetDatingProfile(member.Id);
            }

            return result;
        }

        public async Task<List<int>> GetAllFollowerId(int userId)
        {
            return await _context.Likes.Where(x => x.TargetUserId == userId)
                .Select(x => x.SourceUserId)
                .ToListAsync();

        }

        public async Task<DatingProfileDto> GetDatingProfile(int id)
            => await _context.DatingProfiles.Where(d => d.UserId == id)
            .ProjectTo<DatingProfileDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();

    }
}
