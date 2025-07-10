using Microsoft.AspNetCore.Mvc;
using SmartStore.Application.ServiceManager.Abstraction;
using SmartStore.Application.Services.ApplicationServices.Abstraction;
using SmartStore.Domain.Dtos.Request;

namespace SmartStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DamegedItemController(IServiceManager serviceManager, IMessageService messageService) : ControllerBase
    {
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] DamegedItemRequestDto request)
        {
            var response = await serviceManager.DamegedItemService.AddDamagedItemAsync(request);

            if (response.result)
            {
                return Ok(new { Message = response.message });
            }

            return BadRequest(new { Message = response.message });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromQuery] int damegedItemId, [FromBody] DamegedItemRequestDto request)
        {
            var response = await serviceManager.DamegedItemService.UpdateDamagedItemAsync(damegedItemId, request);
            if (response.result)
            {
                return Ok(new { Message = response.message });
            }

            return NotFound(new { Message = response.message });
        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int damegedItemId)
        {
            var response = await serviceManager.DamegedItemService.DeleteDamagedItemAsync(damegedItemId);

            if (response.result)
                return Ok(new { Message = response.message });
            return NotFound(new { Message = response.message });
        }
    }
}
