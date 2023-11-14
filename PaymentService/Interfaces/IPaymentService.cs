using pay_admin.DTO;

namespace pay_admin.Interfaces
{
    public interface IPaymentService
    {
        Task<HttpResponseMessage> CreateNewBilling(HttpContext context);
        Task<HttpResponseMessage> CancelABilling(HttpContext context);
    }
}
