using Microsoft.AspNetCore.Mvc;
using SmartStore.Application.ServiceManager.Abstraction;
using SmartStore.Application.Services.ApplicationServices.Abstraction;
using SmartStore.Application.Services.ApplicationServices.Implementation;
using SmartStore.Domain.Dtos.Request;
using SmartStore.Domain.Dtos.Response;
using SmartStore.Shared.Pagination;

namespace SmartStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemUnitController(IServiceManager serviceManager, IMessageService messageService) : ControllerBase
    {
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] ItemUnitRequestDto request)
        {
            var response = await serviceManager.ItemUnitService.AddItemUnitAsync(request);

            if (response.result)
            {
                return Ok(new { Message = response.message });
            }

            return BadRequest(new { Message = response.message });
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<PaginationObject<ItemUnitResponseDto>>> GetAll([FromQuery] int pageIndex = 1)
        {
            var response = await serviceManager.ItemUnitService.GetItemsUnitsAsync(pageIndex);
            if (response != null)
            {
                return Ok(response);
            }
            return NotFound(messageService.GetMessage("ValueNotFound"));
        }

        [HttpGet("GetById")]
        public async Task<ActionResult> GetById([FromQuery] int itemUnitId)
        {
            var response = await serviceManager.ItemUnitService.GetItemUnitByIdAsync(itemUnitId);
            if (response != null)
                return Ok(response);

            return NotFound(new { Message = messageService.GetMessage("ValueNotFound") });
        }

        [HttpGet("search")]
        public async Task<ActionResult<PaginationObject<ItemUnitResponseDto>>> Search([FromQuery] string input, [FromQuery] int pageIndex = 1)
        {
            var response = await serviceManager.ItemUnitService.SearchItemUnitAsync(input, pageIndex);
            if (response != null)
                return Ok(response);

            return NotFound(new { Message = messageService.GetMessage("ValueNotFound") });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromQuery] int itemUnitId, [FromBody] ItemUnitRequestDto request)
        {
            var response = await serviceManager.ItemUnitService.UpdateItemUnitAsync(itemUnitId, request);
            if (response.result)
            {
                return Ok(new { Message = response.message });
            }

            return NotFound(new { Message = response.message });
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int itemUnitId)
        {
            var response = await serviceManager.ItemUnitService.DeleteItemUnitAsync(itemUnitId);

            if (response.result)
                return Ok(new { Message = response.message });
            return NotFound(new { Message = response.message });
        }

    }
}
