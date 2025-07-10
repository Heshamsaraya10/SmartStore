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
    public class ItemController(IServiceManager serviceManager, IMessageService messageService) : ControllerBase
    {
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] ItemRequestDto request)
        {
            var response = await serviceManager.ItemService.AddItemAsync(request);

            if (response.result)
            {
                return Ok(new { Message = response.message });
            }

            return BadRequest(new { Message = response.message });
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<PaginationObject<ItemResponseDto>>> GetAll([FromQuery] int pageIndex = 1)
        {
            var response = await serviceManager.ItemService.GetItemsAsync(pageIndex);
            if (response != null)
            {
                return Ok(response);
            }
            return NotFound(messageService.GetMessage("ValueNotFound"));
        }


        [HttpGet("GetById")]
        public async Task<ActionResult> GetById([FromQuery] int itemId)
        {
            var response = await serviceManager.ItemService.GetItemByIdAsync(itemId);
            if (response != null)
                return Ok(response);

            return NotFound(new { Message = messageService.GetMessage("ValueNotFound") });
        }

        [HttpGet("search")]
        public async Task<ActionResult<PaginationObject<ItemResponseDto>>> Search([FromQuery] string input, [FromQuery] int pageIndex = 1)
        {
            var response = await serviceManager.ItemService.SearchItemAsync(input, pageIndex);
            if (response != null)
                return Ok(response);

            return NotFound(new { Message = messageService.GetMessage("ValueNotFound") });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromQuery] int itemId, [FromBody] ItemRequestDto request)
        {
            var response = await serviceManager.ItemService.UpdateItemAsync(itemId, request);
            if (response.result)
            {
                return Ok(new { Message = response.message });
            }

            return NotFound(new { Message = response.message });
        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int itemId)
        {
            var response = await serviceManager.ItemService.DeleteItemAsync(itemId);

            if (response.result)
                return Ok(new { Message = response.message });
            return NotFound(new { Message = response.message });
        }

        [HttpGet("GetDeleted")]
        public async Task<ActionResult<PaginationObject<ItemResponseDto>>> GetDeleted([FromQuery] int pageIndex = 1)
        {
            var response = await serviceManager.ItemService.GetDeletedItemsAsync(pageIndex);
            if (response != null)
            {
                return Ok(response);
            }
            return NotFound(messageService.GetMessage("ValueNotFound"));
        }

        [HttpPut("RestoreDeleted")]
        public async Task<IActionResult> RestoreDeleted([FromQuery] int itemId)
        {
            var response = await serviceManager.ItemService.RestoreItemAsync(itemId);
            if (response.result)
            {
                return Ok(new { Message = response.message });
            }

            return NotFound(new { Message = response.message });
        }


    }
}
