using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace pay_admin.Model
{
    public class PaymentTransaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ObjectId { get; set; }

        [BsonElement("IdUser")]
        public string UserID { get; set; }

        [BsonElement("TransactionID")]
        public string TransactionID { get; set; }

        [BsonElement("PaymentStatus")]
        public string PaymentStatus { get; set; }

        [BsonElement("QrCode")]
        public string QrCode { get; set; }
        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        [BsonElement("UpdatedAt")]
        public DateTime UpdatedAt { get; set; }

    }
}
