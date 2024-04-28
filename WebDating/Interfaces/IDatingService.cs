using WebDating.DTOs;
using WebDating.Entities;

namespace WebDating.Interfaces
{
    public interface IDatingService
    {
        Task<ResultDto<DatingProfileVM>> InitDatingProfile(DatingProfileVM datingProfileVM, string userName);
    }
}
