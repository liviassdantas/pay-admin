using AuthService.DTO;
using AuthService.Service;

namespace AuthService.Interfaces
{
    public interface IAuthService 
    {
        Task<string> GenerateToken(UserDTO user);
    }
}
