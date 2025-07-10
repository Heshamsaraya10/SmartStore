using Microsoft.AspNetCore.Mvc;
using SmartStore.Application.ServiceManager.Abstraction;
using SmartStore.Application.Services.ApplicationServices.Abstraction;
using SmartStore.Application.Services.ApplicationServices.Implementation;
using SmartStore.Domain.Dtos.Request;
using SmartStore.Domain.Dtos.Response;
using SmartStore.Shared.Pagination;

namespace SmartStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SafeController(IServiceManager serviceManager, IMessageService messageService) : ControllerBase
    {
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] SafeRequestDto request)
        {
            var response = await serviceManager.SafeService.AddSafeAsync(request);

            if (response.result)
            {
                return Ok(new { Message = response.message });
            }

            return BadRequest(new { Message = response.message });
        }


        [HttpGet("GetAll")]
        public async Task<ActionResult<PaginationObject<SafeResponseDto>>> GetAll([FromQuery] int pageIndex = 1)
        {
            var response = await serviceManager.SafeService.GetSafesAsync(pageIndex);
            if (response != null)
            {
                return Ok(response);
            }
            return NotFound(messageService.GetMessage("ValueNotFound"));
        }

        [HttpGet("GetById")]
        public async Task<ActionResult> GetById([FromQuery] int safeId)
        {
            var response = await serviceManager.SafeService.GetSafeByIdAsync(safeId);
            if (response != null)
                return Ok(response);

            return NotFound(new { Message = messageService.GetMessage("ValueNotFound") });
        }
        [HttpGet("search")]
        public async Task<ActionResult<PaginationObject<SafeResponseDto>>> Search([FromQuery] string input, [FromQuery] int pageIndex = 1)
        {
            var response = await serviceManager.SafeService.SearchSafeAsync(input, pageIndex);
            if (response != null)
                return Ok(response);

            return NotFound(new { Message = messageService.GetMessage("ValueNotFound") });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromQuery] int safeId, [FromBody] SafeRequestDto request)
        {
            var response = await serviceManager.SafeService.UpdateSafeAsync(safeId, request);
            if (response.result)
            {
                return Ok(new { Message = response.message });
            }

            return NotFound(new { Message = response.message });
        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int safeId)
        {
            var response = await serviceManager.SafeService.DeleteSafeAsync(safeId);

            if (response.result)
                return Ok(new { Message = response.message });
            return NotFound(new { Message = response.message });
        }
    }
}
