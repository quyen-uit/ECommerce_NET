using Core.Entities.Identity;

namespace Core.Interfaces.Services
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
