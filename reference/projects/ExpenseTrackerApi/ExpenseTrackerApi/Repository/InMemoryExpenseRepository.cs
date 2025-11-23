using ExpenseTrackerApi.Models;

namespace ExpenseTrackerApi.Repository
{
    public class InMemoryExpenseRepository : IExpenseRepository
    {
        private readonly List<Expense> _expenses = new();
        public void Add(Expense expense)
        {
            _expenses.Add(expense);
        }
        // Other methods as needed
    }
}
