using ExpenseTrackerApi.Models;

namespace ExpenseTrackerApi.Services
{
    public interface IExpenseService
    {
        Expense AddExpense(ExpenseDto expenseDto);
        Task<IEnumerable<ExpenseDto>> GetExpensesByCategoryAsync(string category);
    }
}
