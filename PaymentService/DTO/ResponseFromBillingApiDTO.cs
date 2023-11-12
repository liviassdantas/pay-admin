namespace pay_admin.DTO
{
    public class ResponseFromBillingApiDTO
    {
        public string message { get; set; }
        public string ID { get; set; }
        public DateTime timestamp { get; set; }
        public string status { get; set; }
        public string qrCode { get; set; }
    }
}
