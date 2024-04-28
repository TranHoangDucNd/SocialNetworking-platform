using AutoMapper;
using WebDating.DTOs;
using WebDating.Entities;
using WebDating.Interfaces;

namespace WebDating.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public DatingRepository( DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<DatingProfileVM> Insert(DatingProfile datingProfile)
        {
            await _context.DatingProfiles.AddAsync(datingProfile);
            return _mapper.Map<DatingProfileVM>(datingProfile);
        }

        public async Task InsertUserInterest(IEnumerable<UserInterest> userInterests, int datingId)
        {
            List<UserInterest> interests = userInterests.ToList();
            foreach (var item in interests)
            {
                var userInterest = new UserInterest()
                {
                    DatingProfileId = datingId,
                    InterestName = item.InterestName
                };
                await _context.UserInterests.AddRangeAsync(userInterest);
                await _context.SaveChangesAsync();
            }
        }
    }
}
