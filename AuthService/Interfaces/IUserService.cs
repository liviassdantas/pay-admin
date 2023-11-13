using AuthService.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Interfaces
{
    public interface IUserService
    {
        Task CreateUserAsync(UserDTO userDTO);
        Task<List<UserDTO>> GetUsersAsync();
        Task<UserDTO> GetOneUserAsync(string email);
        Task<ActionResult<dynamic>> Login(string email, string password);
    }
}
