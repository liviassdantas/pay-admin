using AuthService.DTO;
using AuthService.Interfaces;
using AuthService.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.ComponentModel;

namespace AuthService.Service
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly IConfiguration _configuration;

        public UserService(IOptions<DatabaseSettings> userDatabaseSettings)
        {
            var mongoClient = new MongoClient(userDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(userDatabaseSettings.Value.DatabaseName);
            _userCollection = mongoDatabase.GetCollection<User>(userDatabaseSettings.Value.CollectionName);
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

        private bool VerifyIfEmailExists(string email)
        {
            var emailExist = GetOneUserAsync(email).Result == null ? false : true;

            return emailExist;
        }
    }
}
