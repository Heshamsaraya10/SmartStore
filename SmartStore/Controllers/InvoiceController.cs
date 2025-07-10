using Microsoft.AspNetCore.Mvc;
using SmartStore.Application.ServiceManager.Abstraction;
using SmartStore.Application.Services.ApplicationServices.Abstraction;
using SmartStore.Application.Services.ApplicationServices.Implementation;
using SmartStore.Domain.Dtos.Request;

namespace SmartStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController(IServiceManager serviceManager, IMessageService messageService) : ControllerBase
    {
        [HttpPost("SaleInvoice")]
        public async Task<IActionResult> AddSaleInvoice([FromBody] InvoiceRequestDto request)
        {
            var response = await serviceManager.InvoiceService.AddSaleInvoiceAsync(request);

            if (response != null)
            {
                return Ok(response);
            }

            return BadRequest(new { Message = messageService.GetMessage("FailedToRegisterInvoice") });
        }

        [HttpPost("SalesReturnInvoice")]
        public async Task<IActionResult> AddSalesReturnInvoice([FromBody] InvoiceRequestDto request)
        {
            var response = await serviceManager.InvoiceService.AddSalesReturnInvoiceAsync(request);

            if (response != null)
            {
                return Ok(response);
            }

            return BadRequest(new { Message = messageService.GetMessage("FailedToRegisterInvoice") });
        }
        [HttpPost("PurchaseInvoice")]
        public async Task<IActionResult> AddPurchaseInvoice([FromBody] InvoiceRequestDto request)
        {
            var response = await serviceManager.InvoiceService.AddPurchaseInvoiceAsync(request);

            if (response != null)
            {
                return Ok(response);
            }

            return BadRequest(new { Message = messageService.GetMessage("FailedToRegisterInvoice") });
        }
        [HttpPost("PurchaseReturnInvoice")]
        public async Task<IActionResult> AddPurchaseReturnInvoice([FromBody] InvoiceRequestDto request)
        {
            var response = await serviceManager.InvoiceService.AddPurchaseReturnInvoiceAsync(request);

            if (response != null)
            {
                return Ok(response);
            }

            return BadRequest(new { Message = messageService.GetMessage("FailedToRegisterInvoice") });
        }
    }
}
