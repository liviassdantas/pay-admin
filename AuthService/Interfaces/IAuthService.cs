using AuthService.DTO;
using AuthService.Service;

namespace AuthService.Interfaces
{
    public interface IAuthService 
    {
        Task CreateUserAsync(UserDTO userDTO);
        Task<List<UserDTO>> GetUsersAsync();
        Task<UserDTO> GetOneUserAsync(string id);

    }
}
