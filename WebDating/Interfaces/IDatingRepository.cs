using WebDating.DTOs;
using WebDating.Entities;

namespace WebDating.Interfaces
{
    public interface IDatingRepository
    {
        Task<DatingProfileVM> Insert(DatingProfile datingProfile);
        Task InsertUserInterest(IEnumerable<UserInterest> userInterests, int datingId);
    }
}
