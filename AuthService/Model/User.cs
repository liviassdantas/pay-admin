using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace AuthService.Model
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("Password")]
        public string Password { get; set; }

        [BsonElement("IsAdmin")]
        public bool IsAdmin { get; set; }
    }
}
