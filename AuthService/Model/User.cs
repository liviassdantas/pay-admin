using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace AuthService.Model
{
    public class User
    {
        [NotNull]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [NotNull]
        [BsonElement("Email")]
        public string Email { get; set; }

        [NotNull]
        [BsonElement("Password")]
        public string Password { get; set; }

        [BsonElement("IsAdmin")]
        public bool IsAdmin { get; set; }
    }
}
