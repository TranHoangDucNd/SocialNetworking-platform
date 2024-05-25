using WebDating.DTOs;
using WebDating.Entities.UserEntities;
using WebDating.Helpers;

namespace WebDating.Interfaces
{
    public interface IUserRepository : IBaseGetAllRepository<AppUser>
    {
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<AppUser> GetUserByUsernameAsync(int userId);
        Task UpdateUser(MemberUpdateDto memberUpdateDto);
        Task<IEnumerable<AppUser>> GetAllUserWithPhotosAsync();
        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
        Task<AppUser> GetUserByIdAsync(int id);
        Task<MemberDto> GetMemberAsync(string username);
        Task<string> GetUserGender(string userName);
        void UpdateUser(AppUser user);
        Task<DatingProfileDto> GetDatingProfile(int id);


        Task<PagedList<MemberDto>> GetBestMatch(UserParams userParams);

        Task<List<AppUser>> GetMany(IEnumerable<int> ids);

        Task<AppUser> GetFullInfoByIdAsync(int id);
    }
}
