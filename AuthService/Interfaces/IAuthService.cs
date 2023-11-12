using AuthService.DTO;
using AuthService.Service;

namespace AuthService.Interfaces
{
    public interface IAuthService 
    {
        Task CreateUserAsync(UserDTO userDTO);
        Task<List<UserDTO>> GetUsersAsync();
        Task<UserDTO> GetOneUserAsync(string email);
        Task<HttpResponseMessage> Login(HttpContext context, string email, string password);
    }
}
