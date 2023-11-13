using AuthService.DTO;
using AuthService.Interfaces;
using AuthService.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.ComponentModel;
using System.Net.Http.Headers;

namespace AuthService.Service
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly IAuthService _authService;

        public UserService(IAuthService authService)
        {
            _authService = authService;
        }
        public UserService(IOptions<DatabaseSettings> userDatabaseSettings)
        {
            var mongoClient = new MongoClient(userDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(userDatabaseSettings.Value.DatabaseName);
            _userCollection = mongoDatabase.GetCollection<User>(userDatabaseSettings.Value.CollectionName);
        }


        public async Task CreateUserAsync(UserDTO userDTO)
        {
            User user = new User
            {
                Email = userDTO.Email,
                Password = userDTO.Password,
                IsAdmin = userDTO.IsAdmin,
            };

            await _userCollection.InsertOneAsync(user);

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
                IsAdmin = user.IsAdmin
            };
            return returnUserDTO;
        }
        public async Task<UserDTO> GetOneUserAsync(string email, string password)
        {
            var user = await _userCollection.Find<User>(u => email == u.Email && u.Password == password).FirstOrDefaultAsync();
            if (user != null)
            {
                var returnUserDTO = new UserDTO
                {
                    Email = user.Email,
                    IsAdmin = user.IsAdmin,
                    Password = user.Password
                };
                return returnUserDTO;
            }
            else
            {
                return null;
            }
        }

        public async Task<ActionResult<dynamic>> Login(string email, string password)
        {
            var user = await GetOneUserAsync(email, password);

            if (user == null)
            {
                return new NotFoundObjectResult(new { message = "User or password is invalid" });
            }
            var token = await new AuthServices().GenerateToken(user);
            user.Password = "";

            return new
            {
                token = token,
                user = user,
            };

        }

        private bool VerifyIfEmailExists(string email)
        {
            var emailExist = GetOneUserAsync(email).Result == null ? false : true;

            return emailExist;
        }
    }
}
