using Microsoft.EntityFrameworkCore;
using WebDating.DTOs;
using WebDating.Entities;
using WebDating.Extensions;
using WebDating.Helpers;
using WebDating.Interfaces;

namespace WebDating.Data
{
    public class LikeRepository : ILikeRepository
    {
        private readonly DataContext _context;

        public LikeRepository(DataContext context)
        {
            _context = context;
        }
        ////Mục đích lấy sourceuser và tagetuser ừ y/c về để check dữ liệu trong Db
        //public async Task<UserLike> GetUserLike(int sourceUserId, int tagetUserId)
        //{
        //    return await _context.Likes
        //        .FirstOrDefaultAsync(x => x.SourceUserId == sourceUserId && x.TargetUserId == tagetUserId);
        //}

        ////Mục đích lấy ra sourceUser 
        //public async Task<AppUser> GetUserWithLike(int userId)
        //{
        //    return await _context.Users
        //        .Include(l => l.LikedUsers)
        //        .FirstOrDefaultAsync(x => x.Id == userId);
        //}

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


        //public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        //{
        //    var likes = _context.Likes.AsQueryable();
        //    var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();

        //    if (likesParams.Predicate == "liked")
        //    {
        //        likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
        //        users = likes.Select(like => like.TargetUser);
        //    }

        //    if (likesParams.Predicate == "likedBy")
        //    {
        //        likes = likes.Where(like => like.TargetUserId == likesParams.UserId);
        //        users = likes.Select(like => like.SourceUser);
        //    }

        //    if (likesParams.Predicate == "mutualLike")
        //    {
        //        var likedUsers = likes.Where(like => like.SourceUserId == likesParams.UserId)
        //            .Select(like => like.TargetUserId);

        //        var likedByUsers = likes.Where(like => like.TargetUserId == likesParams.UserId)
        //            .Select(like => like.SourceUserId);

        //        var mutualUserIds = likedUsers.Intersect(likedByUsers);

        //        users = users.Where(u => mutualUserIds.Contains(u.Id));
        //    }

        //    var result = users.Select(user => new LikeDto
        //    {
        //        UserName = user.UserName,
        //        KnownAs = user.KnownAs,
        //        Age = user.DateOfBirth.CaculateAge(),
        //        PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url,
        //        City = user.City,
        //        Id = user.Id
        //    });

        //    return await PagedList<LikeDto>.CreateAsync(result, likesParams.PageNumber, likesParams.PageSize);
        //}

        public Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
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

            if(likesParams.Predicate == "mutualLike")
            {
                var likedUsers = likes.Where(x => x.SourceUserId == likesParams.UserId)
                    .Select(x => x.TargetUserId);

                var likedByUser = likes.Where(x => x.TargetUserId == likesParams.UserId)
                    .Select(x => x.SourceUserId);

                var mutualLike = likedUsers.Intersect(likedByUser);

                users = users.Where(x => mutualLike.Contains(x.Id));
                
            }

            var result = users.Select(user => new LikeDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Age = user.DateOfBirth.CaculateAge(),
                City = user.City,
                KnownAs = user.KnownAs,
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url
            });

            return PagedList<LikeDto>.CreateAsync(result, likesParams.PageNumber, likesParams.PageSize);
        }

    }
}
