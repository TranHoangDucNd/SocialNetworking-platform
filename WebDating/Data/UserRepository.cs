using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebDating.DTOs;
using WebDating.Entities.ProfileEntities;
using WebDating.Entities.UserEntities;
using WebDating.Helpers;
using WebDating.Interfaces;

namespace WebDating.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppUser>> GetAll()
        => await _context.Users.ToListAsync();

        public async Task<DatingProfileDto> GetDatingProfile(int id)
        => await _context.DatingProfiles.Where(d => d.UserId == id)
            .ProjectTo<DatingProfileDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();


        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = _context.Users.AsQueryable();

            query = query.Where(x => x.UserName.ToLower() != userParams.CurrentUserName.ToLower());
            //query = query.Where(x => x.Gender == userParams.Gender);

            var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
            var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(x => x.Created),
                _ => query.OrderByDescending(x => x.LastActive)
            };

            query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);



            var result = await PagedList<MemberDto>
                .CreateAsync
                (query.AsNoTracking()
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider),
                userParams.PageNumber,
                userParams.PageSize);

            foreach (var member in result)
            {
                member.DatingProfile = await GetDatingProfile(member.Id);
            }

            return result;
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<string> GetUserGender(string userName)
        {
            return await _context.Users
                .Where(x => x.UserName == userName)
                .Select(x => x.Gender).FirstOrDefaultAsync();
        }

        public void UpdateUser(AppUser user)
        {
            _context.Users.Update(user);
        }


        public async Task<PagedList<MemberDto>> GetBestMatch(UserParams userParams)
        {
            MemberDto currentUser = await GetMemberAsync(userParams.CurrentUserName);
            if (currentUser == null)
            {
                return new PagedList<MemberDto>(Enumerable.Empty<MemberDto>(), 0, userParams.PageNumber,
                    userParams.PageSize);
            }

            var profile = _context.DatingProfiles.FirstOrDefault(it => it.UserId == currentUser.Id);
            if (profile is null)
            {
                return new PagedList<MemberDto>(Enumerable.Empty<MemberDto>(), 0, userParams.PageNumber, userParams.PageSize);
            }
            IEnumerable<int> listInterest = _context.UserInterests.Where(it => it.DatingProfileId == profile.Id)
                .Select(it => (int)it.InterestName);

            string @interest = string.Join(",", listInterest);
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@MinHeight", userParams.MinHeight),
                new SqlParameter("@MaxHeight",  userParams.MaxHeight),
                new SqlParameter("@Gender", userParams.Gender == Gender.EveryOne ? DBNull.Value : userParams.Gender == Gender.Female ? "female" : "male"),
                new SqlParameter("@MinAge", userParams.MinAge),
                new SqlParameter("@MaxAge", userParams.MaxAge),
                new SqlParameter("@City", userParams.Province is <= 0 ? DBNull.Value : userParams.Province),
                new SqlParameter("@Interest",@interest),
            };

            List<int> listTds = _context.Database.SqlQueryRaw<int>("EXEC Search_Best_Match @MinHeight, @MaxHeight, @Gender,@MinAge,@MaxAge, @City, @Interest", sqlParams).ToList();
            var query = _context.Users.AsQueryable();
            query = query.Where(it => listTds.Contains(it.Id) && it.UserName != userParams.CurrentUserName);
            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(x => x.Created),
                _ => query.OrderByDescending(x => x.LastActive)
            };

            var result = await PagedList<MemberDto>
                .CreateAsync
                (query.AsNoTracking()
                        .ProjectTo<MemberDto>(_mapper.ConfigurationProvider),
                    userParams.PageNumber,
                    userParams.PageSize);

            foreach (var member in result)
            {
                member.DatingProfile = await GetDatingProfile(member.Id);
            }

            return result;
        }

        public async Task<List<AppUser>> GetMany(IEnumerable<int> ids)
        {
            return await _context.Users
                .Where(it => ids.Contains(it.Id))
                .ToListAsync();
        }

    }
}
