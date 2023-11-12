using Microsoft.AspNetCore.Mvc;
using pay_admin.DTO;
using pay_admin.Interfaces;

namespace pay_admin.Controller
{
    public class PaymentTransactionController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentTransactionController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("CreateNewBilling")]
        public async Task<IActionResult> CreateNewBilling(PaymentTransactionDTO paymentTransactionDTO)
        {
            try
            {
                await _paymentService.CreateNewBilling(this.HttpContext, paymentTransactionDTO);

                return new ObjectResult(paymentTransactionDTO)
                {
                    StatusCode = StatusCodes.Status201Created
                };

            }
            catch (Exception ex)
            {
                throw new HttpRequestException(ex.Message, ex.InnerException);
            }

        }
    }
}
