using Microsoft.AspNetCore.Mvc;
using SmartStore.Application.ServiceManager.Abstraction;
using SmartStore.Application.Services.ApplicationServices.Abstraction;
using SmartStore.Domain.Dtos.Request;

namespace SmartStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController(IServiceManager serviceManager, IMessageService messageService) : ControllerBase
    {
        [HttpPost("AddExpense")]
        public async Task<IActionResult> Add([FromBody] ExpenseRequestDto request)
        {
            var response = await serviceManager.expenseService.AddExpenseAsync(request);

            if (response.result)
            {
                return Ok(new { Message = response.message });
            }

            return BadRequest(new { Message = response.message });
        }
    }
}
