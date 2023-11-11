using AuthService.DTO;
using AuthService.Interfaces;
using AuthService.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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

        [HttpGet("GetAllUsers")]
        public async Task<List<UserDTO>> GetAllUsers()
        {
            try
            {
               return await _authServices.GetUsersAsync();
            }catch(Exception ex)
            {
                throw new HttpRequestException(ex.Message, ex.InnerException);
            }
        }

        [HttpGet("GetOneUser")]
        public async Task<UserDTO> GetOneUser(string email)
        {
            try
            {
                return await _authServices.GetOneUserAsync(email);
            }catch(Exception ex)
            {
                throw new HttpRequestException(ex.Message, ex.InnerException);
            }
        }

        [HttpGet("Login")]
        public async Task<UserDTO> Login(string email, string password)
        {
            try
            {
                return await _authServices.Login(email, password);
            }
            catch (Exception ex)
            {
                throw new HttpRequestException(ex.Message, ex.InnerException);
            }
        }
    }
}
