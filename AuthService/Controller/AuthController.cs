using AuthService.DTO;
using AuthService.Interfaces;
using AuthService.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authServices;

        public AuthController (IAuthService authServices)
        {
            _authServices = authServices;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateNewUser(UserDTO userDTO)
        {
            try
            {
                await _authServices.CreateUserAsync(userDTO);

                return new ObjectResult(userDTO)
                {
                    StatusCode = StatusCodes.Status201Created
                };
                
            } catch (Exception ex)
            {
                throw new HttpRequestException(ex.Message, ex.InnerException);
            }
            
        }
    }
}
