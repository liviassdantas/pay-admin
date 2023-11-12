using AuthService.Model;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using AuthService.DTO;
using AuthService.Interfaces;
using System.ComponentModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AuthService.Service
{
    public class AuthServices : IAuthService
    {
        public AuthServices() { }

        private readonly IMongoCollection<User> _userCollection;

        public AuthServices(IOptions<AuthDatabaseSettings> authDatabaseSettings)
        {
            var mongoClient = new MongoClient(authDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(authDatabaseSettings.Value.DatabaseName);
            _userCollection = mongoDatabase.GetCollection<User>(authDatabaseSettings.Value.CollectionName);
        }

        public async Task CreateUserAsync(UserDTO userDTO)
        {
            var emailExists = VerifyIfEmailExists(userDTO.Email);

            if (!emailExists)
            {
                User user = new User
                {
                    Email = userDTO.Email,
                    Password = userDTO.Password,
                    IsAdmin = userDTO.IsAdmin,
                };

                await _userCollection.InsertOneAsync(user);
            }
            else
            {
                throw new WarningException("This email already exists in our database. Please, try another one.");
            }


        }

        public async Task<List<UserDTO>> GetUsersAsync()
        {
            var listUserDTO = new List<UserDTO>();
            var listUser = await _userCollection.Find(user => true).ToListAsync();

            listUser.ForEach(user => listUserDTO.Add(new UserDTO
            {
                Email = user.Email,
                IsAdmin = user.IsAdmin
            }));

            return listUserDTO;
        }

        public async Task<UserDTO> GetOneUserAsync(string email)
        {
            var user = await _userCollection.Find<User>(user => email == user.Email).FirstOrDefaultAsync();
            var returnUserDTO = new UserDTO
            {
                Email = user.Email,
                IsAdmin = user.IsAdmin,
            };
            return returnUserDTO;
        }

        public async Task<HttpResponseMessage> Login(HttpContext httpContext, string email, string password)
        {
            try
            {
                var user = await _userCollection.Find<User>(user => email == user.Email && password == user.Password).FirstOrDefaultAsync();

                var claims = new List<Claim>
                {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.IsAdmin == true ? "Administrator" : "Seller")
                };
                var claimsIdentity = new ClaimsIdentity(claims,
                                         CookieAuthenticationDefaults.AuthenticationScheme);
                await httpContext.SignInAsync(
                      CookieAuthenticationDefaults.AuthenticationScheme,
                      new ClaimsPrincipal(claimsIdentity),
                      new AuthenticationProperties { IsPersistent = true });

                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                throw new HttpRequestException(ex.Message);
            }
        }

        private bool VerifyIfEmailExists(string email)
        {
            var emailExist = GetOneUserAsync(email).Result == null ? false : true;

            return emailExist;
        }

    }
}