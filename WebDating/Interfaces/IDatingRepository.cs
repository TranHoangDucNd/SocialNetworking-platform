using WebDating.DTOs;
using WebDating.Entities.ProfileEntities;
using WebDating.Entities.UserEntities;

namespace WebDating.Interfaces
{
    public interface IDatingRepository
    {
        Task<DatingProfileVM> Insert(DatingProfile datingProfile);
        Task InsertUserInterest(IEnumerable<UserInterest> userInterests, int datingId);
    }
}
