using WebDating.DTOs;
using WebDating.Entities;
using WebDating.Helpers;

namespace WebDating.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
        Task<AppUser> GetUserByIdAsync(int id);
        Task<MemberDto> GetMemberAsync(string username);
        Task<string> GetUserGender(string userName);
        void UpdateUser(AppUser user);
    }
}
