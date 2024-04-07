using WebDating.DTOs;
using WebDating.Entities;

namespace WebDating.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<IEnumerable<MemberDto>> GetMembersAsync();
        Task<MemberDto> GetUserByIdAsync(int id);
        Task<MemberDto> GetMemberAsync(string username);
    }
}
