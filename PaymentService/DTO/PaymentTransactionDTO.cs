﻿
namespace pay_admin.DTO
{
    public class PaymentTransactionDTO
    {
        public string UserID { get; set; }

        public string TransactionID { get; set; }

        public string PaymentStatus { get; set; }

        public string QrCode { get; set; }
    }
}
