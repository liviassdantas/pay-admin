using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using pay_admin.Model.ValueObjects.Enums;

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
        public EPaymentStatus PaymentStatus { get; set; }

        [BsonElement("Value")]
        public double Value { get; set; }
        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        [BsonElement("UpdatedAt")]
        public DateTime UpdatedAt { get; set; }

    }
}
