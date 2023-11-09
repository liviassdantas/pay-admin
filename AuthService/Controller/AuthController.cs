using AuthService.DTO;
using AuthService.Service;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthServices _authServices;

        public AuthController (AuthServices authServices)
        {
            _authServices = authServices;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateNewUser(UserDTO userDTO)
        {
            await _authServices.CreateUserAsync()
        }
    }
}
