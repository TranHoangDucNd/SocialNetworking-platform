using WebDating.Entities.UserEntities;

namespace WebDating.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
