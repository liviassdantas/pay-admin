using AuthService.DTO;

namespace AuthService.Interfaces
{
    public interface IUserService
    {
        Task CreateUserAsync(UserDTO userDTO);
        Task<List<UserDTO>> GetUsersAsync();
        Task<UserDTO> GetOneUserAsync(string email);
    }
}
