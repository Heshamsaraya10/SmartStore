using Microsoft.AspNetCore.Mvc;
using SmartStore.Application.ServiceManager.Abstraction;
using SmartStore.Application.Services.ApplicationServices.Abstraction;
using SmartStore.Domain.Dtos.Request;

namespace SmartStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockAdjustmentController(IServiceManager serviceManager, IMessageService messageService) : ControllerBase
    {
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] StockAdjustmentRequestDto request)
        {
            var response = await serviceManager.StockAdjustmentService.AddStockAdjustmentAsync(request);

            if (response.result)
            {
                return Ok(new { Message = response.message });
            }

            return BadRequest(new { Message = response.message });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromQuery] int stockAdjustmentId, [FromBody] StockAdjustmentRequestDto request)
        {
            var response = await serviceManager.StockAdjustmentService.UpdateStockAdjustmentAsync(stockAdjustmentId, request);
            if (response.result)
            {
                return Ok(new { Message = response.message });
            }

            return NotFound(new { Message = response.message });
        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int stockAdjustmentId)
        {
            var response = await serviceManager.StockAdjustmentService.DeleteStockAdjustmentAsync(stockAdjustmentId);

            if (response.result)
                return Ok(new { Message = response.message });
            return NotFound(new { Message = response.message });
        }
    }
}
