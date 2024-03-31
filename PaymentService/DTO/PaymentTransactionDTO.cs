
using pay_admin.Model.ValueObjects.Enums;

namespace pay_admin.DTO
{
    public class PaymentTransactionDTO
    {
        public string UserID { get; set; }

        public string TransactionID { get; set; }

        public EPaymentStatus PaymentStatus { get; set; }

        public double Value { get; set; }
        public bool IsUserAdmin { get; set; }
    }
}
