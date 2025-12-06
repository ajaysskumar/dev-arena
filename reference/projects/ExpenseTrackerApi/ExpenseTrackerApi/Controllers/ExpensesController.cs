using ExpenseTrackerApi.Models;
using ExpenseTrackerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _service;

        public ExpensesController(IExpenseService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult AddExpense([FromBody] ExpenseDto expenseDto)
        {
            // Implementation will be added via TDD
            throw new NotImplementedException();
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetExpensesByCategory(string category)
        {
            var expenses = await _service.GetExpensesByCategoryAsync(category);
            return Ok(expenses);
        }
    }
}
