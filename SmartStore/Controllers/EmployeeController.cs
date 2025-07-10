using Azure;
using Microsoft.AspNetCore.Mvc;
using SmartStore.Application.ServiceManager.Abstraction;
using SmartStore.Application.Services.ApplicationServices.Abstraction;
using SmartStore.Application.Services.ApplicationServices.Implementation;
using SmartStore.Application.Services.BusinessServices.Abstraction;
using SmartStore.Domain.Dtos.Request;
using SmartStore.Domain.Dtos.Response;
using SmartStore.Shared.Pagination;

namespace SmartStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController(IServiceManager serviceManager , IMessageService messageService) : ControllerBase
    {
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] EmployeeRequestDto request)
        {
            var employee = await serviceManager.EmployeeService.AddEmployeeAsync(request);

            if (employee.result)
            {
                return Ok(new { Message = employee.message});
            }

            return BadRequest(new { Message = employee.message });
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<PaginationObject<EmployeeResponseDto>>> GetAll([FromQuery] int pageIndex = 1)
        {
            var response = await serviceManager.EmployeeService.GetEmployeeAsync(pageIndex);

            if (response != null)
            {
                return Ok(response);
            }

            return NotFound(messageService.GetMessage("ValueNotFound"));
        }

        [HttpGet("GetById")]
        public async Task<ActionResult> GetById([FromQuery] int employeeId)
        {
            var response = await serviceManager.EmployeeService.GetEmployeeByIdAsync(employeeId);
            if (response != null)
                return Ok(response);

            return NotFound(new { Message = messageService.GetMessage("ValueNotFound") });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromQuery] int employeeId, [FromBody] EmployeeRequestDto request)
        {
            var response = await serviceManager.EmployeeService.UpdateEmployeeAsync(employeeId, request);
            if (response.result)
            {
                return Ok(new { Message = response.message });
            }

            return NotFound(new { Message = response.message });
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int itemId)
        {
            var response = await serviceManager.EmployeeService.DeleteEmployeeAsync(itemId);

            if (response.result)
                return Ok(new { Message = response.message });
            return NotFound(new { Message = response.message });
        }

        [HttpGet("search")]
        public async Task<ActionResult<PaginationObject<EmployeeResponseDto>>> Search([FromQuery] string input, [FromQuery] int pageIndex = 1)
        {
            var response = await serviceManager.EmployeeService.SearchEmployeeAsync(input, pageIndex);
            if (response != null)
                return Ok(response);

            return NotFound(new { Message = messageService.GetMessage("ValueNotFound") });
        }
    }
}
