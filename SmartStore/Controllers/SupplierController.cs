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
    public class SupplierController(IServiceManager serviceManager, IMessageService messageService) : ControllerBase
    {
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] SupplierRequestDto request)
        {
            var response = await serviceManager.SupplierService.AddSupplierAsync(request);

            if (response.result)
            {
                return Ok(new { Message = response.message });
            }

            return BadRequest(new { Message = response.message });
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<PaginationObject<SupplierResponseDto>>> GetAll([FromQuery] int pageIndex = 1)
        {
            var response = await serviceManager.SupplierService.GetSuppliersAsync(pageIndex);
            if (response != null)
            {
                return Ok(response);
            }
            return NotFound(messageService.GetMessage("ValueNotFound"));
        }

        [HttpGet("GetById")]
        public async Task<ActionResult> GetById([FromQuery] int supplierId)
        {
            var response = await serviceManager.SupplierService.GetSupplierByIdAsync(supplierId);
            if (response != null)
                return Ok(response);

            return NotFound(new { Message = messageService.GetMessage("ValueNotFound") });
        }

        [HttpGet("search")]
        public async Task<ActionResult<PaginationObject<SupplierResponseDto>>> Search([FromQuery] string input, [FromQuery] int pageIndex = 1)
        {
            var response = await serviceManager.SupplierService.SearchSupplierAsync(input, pageIndex);
            if (response != null)
                return Ok(response);

            return NotFound(new { Message = messageService.GetMessage("ValueNotFound") });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromQuery] int supplierId, [FromBody] SupplierRequestDto request)
        {
            var response = await serviceManager.SupplierService.UpdateSupplierAsync(supplierId, request);
            if (response.result)
            {
                return Ok(new { Message = response.message });
            }

            return NotFound(new { Message = response.message });
        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int supplierId)
        {
            var response = await serviceManager.SupplierService.DeleteSupplierAsync(supplierId);

            if (response.result)
                return Ok(new { Message = response.message });
            return NotFound(new { Message = response.message });
        }
    }
}
