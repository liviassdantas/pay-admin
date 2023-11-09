using AuthService.Model;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using AuthService.DTO;

namespace AuthService.Service
{
    public class AuthServices
    {
        private readonly IMongoCollection<User> _userCollection;

        public AuthServices(IOptions<AuthDatabaseSettings> authDatabaseSettings)
        {
            var mongoClient = new MongoClient(authDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(authDatabaseSettings.Value.DatabaseName);
            _userCollection = mongoDatabase.GetCollection<User>(authDatabaseSettings.Value.CollectionName);
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
            var ListUserDTO = new List<UserDTO>();
            var ListUser = await _userCollection.Find(user => true).ToListAsync();

            ListUser.ForEach(user => ListUserDTO.Add(new UserDTO {
                Email = user.Email, 
                Password = user.Password, 
                IsAdmin = user.IsAdmin }));

            return ListUserDTO;
        }

        public async Task<User> GetOneUserAsync(string id) =>
            await _userCollection.Find<User>(user => id == user.Id).FirstOrDefaultAsync();

    }
}