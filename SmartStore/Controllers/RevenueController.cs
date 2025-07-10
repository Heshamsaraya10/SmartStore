using Microsoft.AspNetCore.Mvc;
using SmartStore.Application.ServiceManager.Abstraction;
using SmartStore.Application.Services.ApplicationServices.Abstraction;
using SmartStore.Domain.Dtos.Request;

namespace SmartStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenueController(IServiceManager serviceManager, IMessageService messageService) : ControllerBase
    {
        [HttpPost("AddRevenue")]
        public async Task<IActionResult> Add([FromBody] RevenueRequestDto request)
        {
            var response = await serviceManager.RevenueService.AddRevenueAsync(request);

            if (response.result)
            {
                return Ok(new { Message = response.message });
            }

            return BadRequest(new { Message = response.message });
        }
    }
}
