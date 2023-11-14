using AuthService.DTO;
using AuthService.Interfaces;
using AuthService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userServices;

        public UserController (IUserService userServices)
        {
            _userServices = userServices;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateNewUser(UserDTO userDTO)
        {
            try
            {
                await _userServices.CreateUserAsync(userDTO);

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
               return await _userServices.GetUsersAsync();
            }catch(Exception ex)
            {
                throw new HttpRequestException(ex.Message, ex.InnerException);
            }
        }


        [HttpPost("Login")]
        public async Task<ActionResult<dynamic>> Login(string email, string password)
        {
            try
            {
                return await _userServices.Login(email, password);
            }
            catch (Exception ex)
            {
                throw new HttpRequestException(ex.Message, ex.InnerException);
            }
        }
    }
}
