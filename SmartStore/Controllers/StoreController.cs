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
    public class StoreController(IServiceManager serviceManager, IMessageService messageService) : ControllerBase
    {
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] StoreRequestDto request)
        {
            var response = await serviceManager.StoreService.AddStoreAsync(request);

            if (response.result)
            {
                return Ok(new { Message = response.message });
            }

            return BadRequest(new { Message = response.message });
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<PaginationObject<StoreResponseDto>>> GetAll([FromQuery] int pageIndex = 1)
        {
            var response = await serviceManager.StoreService.GetStoresAsync(pageIndex);
            if (response != null)
            {
                return Ok(response);
            }
            return NotFound(messageService.GetMessage("ValueNotFound"));
        }
        [HttpGet("GetById")]
        public async Task<ActionResult> GetById([FromQuery] int storeId)
        {
            var response = await serviceManager.StoreService.GetStoreByIdAsync(storeId);
            if (response != null)
                return Ok(response);

            return NotFound(new { Message = messageService.GetMessage("ValueNotFound") });
        }
        [HttpGet("search")]
        public async Task<ActionResult<PaginationObject<StoreResponseDto>>> Search([FromQuery] string input, [FromQuery] int pageIndex = 1)
        {
            var response = await serviceManager.StoreService.SearchStoreAsync(input, pageIndex);
            if (response != null)
                return Ok(response);

            return NotFound(new { Message = messageService.GetMessage("ValueNotFound") });
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromQuery] int storeId, [FromBody] StoreRequestDto request)
        {
            var response = await serviceManager.StoreService.UpdateStoreAsync(storeId, request);
            if (response.result)
            {
                return Ok(new { Message = response.message });
            }

            return NotFound(new { Message = response.message });
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int storeId)
        {
            var response = await serviceManager.StoreService.DeleteStoreAsync(storeId);

            if (response.result)
                return Ok(new { Message = response.message });
            return NotFound(new { Message = response.message });
        }

    }
}
