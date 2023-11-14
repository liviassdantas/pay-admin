using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pay_admin.DTO;
using pay_admin.Interfaces;

namespace pay_admin.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentTransactionController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentTransactionController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("CreateNewBilling")]
        public async Task<IActionResult> CreateNewBilling()
        {
            try
            {
                await _paymentService.CreateNewBilling(this.HttpContext);

                return CreatedAtAction("CreateNewBilling", StatusCodes.Status201Created);

            }
            catch (Exception ex)
            {
                throw new HttpRequestException(ex.Message, ex.InnerException);
            }

        }
    }
}
