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
            return await _context.Users
                .Include(x => x.Photos)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }
        
        public async Task<AppUser> GetUserByUsernameAsync(int userId)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.Id == userId);
        }
        
        public async Task UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.UserName == memberUpdateDto.Username);
            
            if (user == null) return;
            
            var profile = await _context.DatingProfiles
                .Include(it => it.UserInterests)
                .Include(it => it.Occupations)
                .SingleOrDefaultAsync(x => x.UserId == user.Id);
            
            user.Introduction = memberUpdateDto.Introduction;
            user.City = memberUpdateDto.City;
            user.Height = memberUpdateDto.Height;
            user.Weight = memberUpdateDto.Weight;
           
            profile.DatingObject = memberUpdateDto.DatingObject;
            profile.HeightFrom = memberUpdateDto.HeightFrom;
            profile.HeightTo = memberUpdateDto.HeightTo;
            profile.WeightFrom = memberUpdateDto.WeightFrom;
            profile.WeightTo = memberUpdateDto.WeightTo;
            profile.DatingAgeFrom = memberUpdateDto.DatingAgeFrom;
            profile.DatingAgeTo = memberUpdateDto.DatingAgeTo;
            profile.WhereToDate = memberUpdateDto.WhereToDate;


            profile.UserInterests = memberUpdateDto.DatingProfile.UserInterests.Select(it => new UserInterest
            {
                DatingProfileId = profile.Id,
                InterestName = it.InterestNameCode,
                DatingProfile = profile,
                InterestType = it.InterestType
            }).DistinctBy(it => it.InterestName).ToList();

            profile.Occupations = memberUpdateDto.DatingProfile.Occupations.Select(it => new Occupations
            {
                DatingProfileId = profile.Id,
                OccupationName = it.OccupationNameCode,
                DatingProfile = profile,
                OccupationType = it.OccupationType
            }).DistinctBy(it => it.OccupationName).ToList();

            _context.Users.Update(user);
            _context.DatingProfiles.Update(profile);
        }
        
        public async Task<IEnumerable<AppUser>> GetAllUserWithPhotosAsync()
        {
            return await _context.Users.Include(p => p.Photos).ToListAsync();
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
            //IEnumerable<int> listInterest = _context.UserInterests.Where(it => it.DatingProfileId == profile.Id)
            //    .Select(it => (int)it.InterestName);



            IEnumerable<int> listInterest = _context.UserInterests
                .Where(it => it.DatingProfileId == profile.Id && it.InterestType == InterestType.DesiredInterest)
                .Select(it => (int)it.InterestName);

            IEnumerable<int> listOccupation = _context.Occupations
                .Where(it => it.DatingProfileId == profile.Id && it.OccupationType == OccupationType.DesiredOccupation)
                .Select(it => (int)it.OccupationName);

           

            var minAge = userParams.MinAge > 0 ? userParams.MinAge : profile.DatingAgeFrom;
            var maxAge = userParams.MaxAge > 0 ? userParams.MaxAge : profile.DatingAgeTo;

            var minHeight = userParams.MinHeight > 0 ? userParams.MinHeight : profile.HeightFrom;
            var maxHeight = userParams.MaxHeight > 0 ? userParams.MaxHeight : profile.HeightTo;

            var minWeight = userParams.MinWeight > 0 ? userParams.MinWeight : profile.WeightFrom;
            var maxWeight = userParams.MaxWeight > 0 ? userParams.MaxWeight : profile.WeightTo;


            if (userParams.Gender == Gender.MatchGender)
            {
                userParams.Gender = (Gender)profile.DatingObject;
            }

            string @interest = string.Join(",", listInterest);
            string @occupation = string.Join(",", listOccupation);
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@MinHeight", minHeight),
                new SqlParameter("@MaxHeight",  maxHeight),
                new SqlParameter("@Gender", userParams.Gender == Gender.EveryOne ? DBNull.Value : userParams.Gender == Gender.Female ? "female" : "male"),
                new SqlParameter("@MinAge", minAge),
                new SqlParameter("@MaxAge", maxAge),
                new SqlParameter("@City", userParams.Province is <= 0 ? DBNull.Value : userParams.Province),
                new SqlParameter("@Interest", @interest),
                new SqlParameter("@MinWeight", minWeight),
                new SqlParameter("@MaxWeight", maxWeight),
                new SqlParameter("@Occupation", @occupation), 
            };

            List<int> listTds = _context.Database
                .SqlQueryRaw<int>("EXEC Search_Best_Match @MinHeight, @MaxHeight, @Gender,@MinAge,@MaxAge,@City,@Interest,@MinWeight,@MaxWeight,@Occupation ", sqlParams)
                .ToList();

            var query = _context.Users.AsQueryable();
            query = query.Where(it => listTds.Contains(it.Id));
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

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(x => x.Created),
                _ => query.OrderByDescending(x => x.LastActive)
            };

            return result;
        }

        public async Task<List<AppUser>> GetMany(IEnumerable<int> ids)
        {
            return await _context.Users
                .Where(it => ids.Contains(it.Id))
                .ToListAsync();
        }

        public async Task<AppUser> GetFullInfoByIdAsync(int id)
        {
            return await _context.Users
                .Include(x => x.Photos)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

    }
}
