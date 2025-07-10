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
    public class ItemTypeController(IServiceManager serviceManager, IMessageService messageService) : ControllerBase
    {
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] ItemTypeRequestDto request)
        {
            var response = await serviceManager.ItemTypeService.AddItemTypeAsync(request);

            if (response.result)
            {
                return Ok(new { Message = response.message });
            }

            return BadRequest(new { Message = response.message });
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<PaginationObject<ItemTypeResponseDto>>> GetAll([FromQuery] int pageIndex = 1)
        {
            var response = await serviceManager.ItemTypeService.GetItemsTypesAsync(pageIndex);
            if (response != null)
            {
                return Ok(response);
            }
            return NotFound(messageService.GetMessage("ValueNotFound"));
        }

        [HttpGet("GetById")]
        public async Task<ActionResult> GetById([FromQuery] int itemTypeId)
        {
            var response = await serviceManager.ItemTypeService.GetItemTypeByIdAsync(itemTypeId);
            if (response != null)
                return Ok(response);

            return NotFound(new { Message = messageService.GetMessage("ValueNotFound") });
        }

        [HttpGet("search")]
        public async Task<ActionResult<PaginationObject<ItemTypeResponseDto>>> Search([FromQuery] string input, [FromQuery] int pageIndex = 1)
        {
            var response = await serviceManager.ItemTypeService.SearchItemTypeAsync(input, pageIndex);
            if (response != null)
                return Ok(response);

            return NotFound(new { Message = messageService.GetMessage("ValueNotFound") });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromQuery] int itemTypeId, [FromBody] ItemTypeRequestDto request)
        {
            var response = await serviceManager.ItemTypeService.UpdateItemTypeAsync(itemTypeId, request);
            if (response.result)
            {
                return Ok(new { Message = response.message });
            }

            return NotFound(new { Message = response.message });
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int itemTypeId)
        {
            var response = await serviceManager.ItemTypeService.DeleteItemTypeAsync(itemTypeId);

            if (response.result)
                return Ok(new { Message = response.message });
            return NotFound(new { Message = response.message });
        }
    }
}
