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
    public class ItemCategoryController(IServiceManager serviceManager, IMessageService messageService) : ControllerBase
    {
        [HttpPost("Add")]
        public async Task<ActionResult> Add([FromBody] ItemCategoryRequestDto request)
        {
            var response = await serviceManager.itemCategoryService.AddItemCategoryAsync(request);

            if (response.result)
            {
                return Ok(new { Message = response.message });
            }

            return BadRequest(new { Message = response.message });
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<PaginationObject<ItemCategoryResponseDto>>> GetAll([FromQuery] int pageIndex = 1)
        {
            var response = await serviceManager.itemCategoryService.GetItemsCategoriesAsync(pageIndex);
            if (response != null)
            {
                return Ok(response);
            }
            return NotFound(messageService.GetMessage("ValueNotFound"));
        }

        [HttpGet("GetById")]
        public async Task<ActionResult> GetById([FromQuery] int itemCategoryId)
        {
            var response = await serviceManager.itemCategoryService.GetItemCategoryByIdAsync(itemCategoryId);
            if (response != null)
                return Ok(response);

            return NotFound(new { Message = messageService.GetMessage("ValueNotFound") });
        }

        [HttpGet("search")]
        public async Task<ActionResult<PaginationObject<ItemCategoryResponseDto>>> Search([FromQuery] string input, [FromQuery] int pageIndex = 1)
        {
            var response = await serviceManager.itemCategoryService.SearchItemCategoryAsync(input, pageIndex);
            if (response != null)
                return Ok(response);

            return NotFound(new { Message = messageService.GetMessage("ValueNotFound") });
        }

        [HttpPut("Update")]
        public async Task<ActionResult> Update([FromQuery] int itemCategoryId, [FromBody] ItemCategoryRequestDto request)
        {
            var response = await serviceManager.itemCategoryService.UpdateItemCategoryAsync(itemCategoryId, request);
            if (response.result)
            {
                return Ok(new { Message = response.message });
            }

            return NotFound(new { Message = response.message });
        }


        [HttpDelete("Delete")]
        public async Task<ActionResult> Delete([FromQuery] int itemCategoryId)
        {
            var response = await serviceManager.itemCategoryService.DeleteItemCategoryAsync(itemCategoryId);

            if (response.result)
                return Ok(new { Message = response.message });
            return NotFound(new { Message = response.message });
        }
    }
}
