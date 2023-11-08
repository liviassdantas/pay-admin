using AuthService.Model;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

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

        public async Task CreateUserAsync(User user) =>
            await _userCollection.InsertOneAsync(user);

        public async Task<List<User>> GetUsersAsync() =>
            await _userCollection.Find(user => true).ToListAsync();

        public async Task<User> GetOneUserAsync(string id) =>
            await _userCollection.Find<User>(user => id == user.Id).FirstOrDefaultAsync();

    }
}